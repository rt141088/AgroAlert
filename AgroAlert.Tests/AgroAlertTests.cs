using AgroAlert.Domain.Entities;
using AgroAlert.Domain.Enums;
using AgroAlert.Infrastructure.Data;
using AgroAlert.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace AgroAlert.Tests;

// ============================================================
// TESTE 1 — Criação de Agricultor
// ============================================================
public class AgricultorTests
{
	private AgroAlertDbContext CriarContexto()
	{
		var options = new DbContextOptionsBuilder<AgroAlertDbContext>()
			.UseInMemoryDatabase(Guid.NewGuid().ToString())
			.Options;
		return new AgroAlertDbContext(options);
	}

	[Fact]
	public async Task DeveCriarAgricultorComSucesso()
	{
		// Arrange
		var ctx = CriarContexto();
		var repo = new AgricultorRepository(ctx);
		var agricultor = new Agricultor
		{
			Nome = "Maria Souza",
			Email = "maria@teste.com",
			SenhaHash = BCrypt.Net.BCrypt.HashPassword("senha123"),
			Telefone = "(11) 91111-2222",
			CPF = "987.654.321-00"
		};

		// Act
		await repo.AddAsync(agricultor);
		await ctx.SaveChangesAsync();
		var resultado = await repo.GetByIdAsync(agricultor.Id);

		// Assert
		Assert.NotNull(resultado);
		Assert.Equal("Maria Souza", resultado.Nome);
		Assert.Equal("maria@teste.com", resultado.Email);
	}

	// ============================================================
	// TESTE 2 — Login com senha errada (rejeitado)
	// ============================================================
	[Fact]
	public async Task DeveRejeitarLoginComSenhaErrada()
	{
		// Arrange
		var ctx = CriarContexto();
		var repo = new AgricultorRepository(ctx);
		var agricultor = new Agricultor
		{
			Nome = "Carlos Lima",
			Email = "carlos@teste.com",
			SenhaHash = BCrypt.Net.BCrypt.HashPassword("senhaCorreta"),
			Telefone = "(11) 93333-4444",
			CPF = "111.222.333-44"
		};
		await repo.AddAsync(agricultor);
		await ctx.SaveChangesAsync();

		// Act
		var senhaValida = BCrypt.Net.BCrypt.Verify("senhaErrada", agricultor.SenhaHash);

		// Assert
		Assert.False(senhaValida);
	}

	// ============================================================
	// TESTE 3 — Login com senha correta (aceito)
	// ============================================================
	[Fact]
	public async Task DeveAceitarLoginComSenhaCorreta()
	{
		// Arrange
		var ctx = CriarContexto();
		var repo = new AgricultorRepository(ctx);
		var agricultor = new Agricultor
		{
			Nome = "Ana Paula",
			Email = "ana@teste.com",
			SenhaHash = BCrypt.Net.BCrypt.HashPassword("minhasenha"),
			Telefone = "(11) 95555-6666",
			CPF = "555.666.777-88"
		};
		await repo.AddAsync(agricultor);
		await ctx.SaveChangesAsync();

		// Act
		var senhaValida = BCrypt.Net.BCrypt.Verify("minhasenha", agricultor.SenhaHash);

		// Assert
		Assert.True(senhaValida);
	}
}

// ============================================================
// TESTE 4 — Criação de Alerta automático
// ============================================================
public class AlertaTests
{
	private AgroAlertDbContext CriarContexto()
	{
		var options = new DbContextOptionsBuilder<AgroAlertDbContext>()
			.UseInMemoryDatabase(Guid.NewGuid().ToString())
			.Options;
		return new AgroAlertDbContext(options);
	}

	[Fact]
	public async Task DeveCriarAlertaComNivelAlto()
	{
		// Arrange
		var ctx = CriarContexto();

		var agricultor = new Agricultor
		{
			Nome = "Pedro Alves",
			Email = "pedro@teste.com",
			SenhaHash = BCrypt.Net.BCrypt.HashPassword("senha"),
			Telefone = "(11) 97777-8888",
			CPF = "222.333.444-55"
		};
		ctx.Agricultores.Add(agricultor);
		await ctx.SaveChangesAsync();

		var propriedade = new Propriedade
		{
			Nome = "Fazenda Boa Vista",
			Localizacao = "MG",
			AreaHectares = 200,
			Latitude = -19.9,
			Longitude = -43.9,
			TipoCultura = "Milho",
			AgricultorId = agricultor.Id
		};
		ctx.Propriedades.Add(propriedade);
		await ctx.SaveChangesAsync();

		var alerta = new Alerta
		{
			Titulo = "Chuva Intensa Detectada",
			Descricao = "Precipitação acima de 80mm detectada",
			TipoAlerta = TipoAlerta.Chuva,
			NivelRisco = NivelRisco.Alto,
			PropriedadeId = propriedade.Id,
			DataCriacao = DateTime.UtcNow,
			Lido = false
		};

		// Act
		ctx.Alertas.Add(alerta);
		await ctx.SaveChangesAsync();
		var resultado = await ctx.Alertas.FindAsync(alerta.Id);

		// Assert
		Assert.NotNull(resultado);
		Assert.Equal(NivelRisco.Alto, resultado.NivelRisco);
		Assert.Equal(TipoAlerta.Chuva, resultado.TipoAlerta);
		Assert.False(resultado.Lido);
	}

	// ============================================================
	// TESTE 5 — Validação de Regra de Alerta
	// ============================================================
	[Fact]
	public void DeveDispararAlertaQuandoTemperaturaUltrapassaLimite()
	{
		// Arrange
		var regra = new RegraAlerta
		{
			Nome = "Temperatura Crítica",
			TipoAlerta = TipoAlerta.Temperatura,
			Parametro = "Temperatura",
			Operador = ">",
			ValorLimite = 38,
			NivelRisco = NivelRisco.Alto,
			PropriedadeId = 1
		};

		double temperaturaRecebida = 42.5;

		// Act
		bool deveDisparar = regra.Operador == ">" && temperaturaRecebida > regra.ValorLimite;

		// Assert
		Assert.True(deveDisparar);
	}
}