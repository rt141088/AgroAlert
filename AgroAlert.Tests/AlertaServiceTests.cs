using AgroAlert.Application.DTOs;
using AgroAlert.Application.Services;
using AgroAlert.Domain.Entities;
using AgroAlert.Domain.Enums;
using AgroAlert.Infrastructure.Data;
using AgroAlert.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace AgroAlert.Tests;

public class AlertaServiceTests
{
    private AgroAlertDbContext CriarContexto(string nome)
    {
        var options = new DbContextOptionsBuilder<AgroAlertDbContext>()
            .UseInMemoryDatabase(nome).Options;
        return new AgroAlertDbContext(options);
    }

    // TESTE 1 - Criação de alerta quando regra é disparada
    [Fact]
    public async Task DeveGerarAlertaQuandoTemperaturaUltrapassaLimite()
    {
        // Arrange
        using var ctx = CriarContexto("teste1");
        var propriedade = new Propriedade { Id = 1, Nome = "Fazenda Teste", AgricultorId = 1 };
        var regra = new RegraAlerta
        {
            PropriedadeId = 1, Nome = "Temp Alta",
            Parametro = "Temperatura", Operador = ">", ValorLimite = 35,
            NivelRisco = NivelRisco.Alto, TipoAlerta = TipoAlerta.Temperatura, Ativa = true
        };
        ctx.Propriedades.Add(propriedade);
        ctx.RegrasAlerta.Add(regra);
        await ctx.SaveChangesAsync();

        var alertaRepo = new AlertaRepository(ctx);
        var regraRepo = new RegraAlertaRepository(ctx);
        var service = new AlertaService(alertaRepo, regraRepo);

        var dado = new DadoClimaticoDTO { PropriedadeId = 1, Temperatura = 40 };

        // Act
        await service.ProcessarDadosClimaticosAsync(dado);

        // Assert
        var alertas = await alertaRepo.GetByPropriedadeIdAsync(1);
        Assert.Single(alertas);
        Assert.Equal(NivelRisco.Alto, alertas.First().NivelRisco);
    }

    // TESTE 2 - Não deve gerar alerta quando temperatura está abaixo do limite
    [Fact]
    public async Task NaoDeveGerarAlertaQuandoTemperaturaAbaixoDoLimite()
    {
        // Arrange
        using var ctx = CriarContexto("teste2");
        var propriedade = new Propriedade { Id = 1, Nome = "Fazenda Teste", AgricultorId = 1 };
        var regra = new RegraAlerta
        {
            PropriedadeId = 1, Nome = "Temp Alta",
            Parametro = "Temperatura", Operador = ">", ValorLimite = 35,
            NivelRisco = NivelRisco.Alto, TipoAlerta = TipoAlerta.Temperatura, Ativa = true
        };
        ctx.Propriedades.Add(propriedade);
        ctx.RegrasAlerta.Add(regra);
        await ctx.SaveChangesAsync();

        var alertaRepo = new AlertaRepository(ctx);
        var regraRepo = new RegraAlertaRepository(ctx);
        var service = new AlertaService(alertaRepo, regraRepo);

        var dado = new DadoClimaticoDTO { PropriedadeId = 1, Temperatura = 25 };

        // Act
        await service.ProcessarDadosClimaticosAsync(dado);

        // Assert
        var alertas = await alertaRepo.GetByPropriedadeIdAsync(1);
        Assert.Empty(alertas);
    }

    // TESTE 3 - Regra inativa não deve gerar alerta
    [Fact]
    public async Task RegraInativaNaoDeveGerarAlerta()
    {
        // Arrange
        using var ctx = CriarContexto("teste3");
        var propriedade = new Propriedade { Id = 1, Nome = "Fazenda Teste", AgricultorId = 1 };
        var regra = new RegraAlerta
        {
            PropriedadeId = 1, Nome = "Regra Desativada",
            Parametro = "Temperatura", Operador = ">", ValorLimite = 10,
            NivelRisco = NivelRisco.Baixo, TipoAlerta = TipoAlerta.Temperatura,
            Ativa = false  // INATIVA
        };
        ctx.Propriedades.Add(propriedade);
        ctx.RegrasAlerta.Add(regra);
        await ctx.SaveChangesAsync();

        var alertaRepo = new AlertaRepository(ctx);
        var regraRepo = new RegraAlertaRepository(ctx);
        var service = new AlertaService(alertaRepo, regraRepo);

        var dado = new DadoClimaticoDTO { PropriedadeId = 1, Temperatura = 999 };

        // Act
        await service.ProcessarDadosClimaticosAsync(dado);

        // Assert
        var alertas = await alertaRepo.GetByPropriedadeIdAsync(1);
        Assert.Empty(alertas);
    }

    // TESTE 4 - Múltiplas regras geram múltiplos alertas
    [Fact]
    public async Task MultiplasRegrasDevemGerarMultiplosAlertas()
    {
        // Arrange
        using var ctx = CriarContexto("teste4");
        var propriedade = new Propriedade { Id = 1, Nome = "Fazenda Teste", AgricultorId = 1 };
        ctx.Propriedades.Add(propriedade);
        ctx.RegrasAlerta.AddRange(
            new RegraAlerta { PropriedadeId = 1, Nome = "Temp Alta", Parametro = "Temperatura", Operador = ">", ValorLimite = 35, NivelRisco = NivelRisco.Alto, TipoAlerta = TipoAlerta.Temperatura, Ativa = true },
            new RegraAlerta { PropriedadeId = 1, Nome = "Chuva Forte", Parametro = "Precipitacao", Operador = ">", ValorLimite = 30, NivelRisco = NivelRisco.Critico, TipoAlerta = TipoAlerta.Chuva, Ativa = true }
        );
        await ctx.SaveChangesAsync();

        var alertaRepo = new AlertaRepository(ctx);
        var regraRepo = new RegraAlertaRepository(ctx);
        var service = new AlertaService(alertaRepo, regraRepo);

        var dado = new DadoClimaticoDTO { PropriedadeId = 1, Temperatura = 40, Precipitacao = 60 };

        // Act
        await service.ProcessarDadosClimaticosAsync(dado);

        // Assert
        var alertas = await alertaRepo.GetByPropriedadeIdAsync(1);
        Assert.Equal(2, alertas.Count());
    }

    // TESTE 5 - Alerta com NivelRisco correto conforme regra
    [Fact]
    public async Task AlertaDeveTerNivelRiscoCorreto()
    {
        // Arrange
        using var ctx = CriarContexto("teste5");
        var propriedade = new Propriedade { Id = 1, Nome = "Fazenda Teste", AgricultorId = 1 };
        var regra = new RegraAlerta
        {
            PropriedadeId = 1, Nome = "Geada Critica",
            Parametro = "Temperatura", Operador = "<", ValorLimite = 0,
            NivelRisco = NivelRisco.Critico, TipoAlerta = TipoAlerta.Geada, Ativa = true
        };
        ctx.Propriedades.Add(propriedade);
        ctx.RegrasAlerta.Add(regra);
        await ctx.SaveChangesAsync();

        var alertaRepo = new AlertaRepository(ctx);
        var regraRepo = new RegraAlertaRepository(ctx);
        var service = new AlertaService(alertaRepo, regraRepo);

        var dado = new DadoClimaticoDTO { PropriedadeId = 1, Temperatura = -3 };

        // Act
        await service.ProcessarDadosClimaticosAsync(dado);

        // Assert
        var alerta = (await alertaRepo.GetByPropriedadeIdAsync(1)).First();
        Assert.Equal(NivelRisco.Critico, alerta.NivelRisco);
        Assert.Equal(TipoAlerta.Geada, alerta.TipoAlerta);
    }
}
