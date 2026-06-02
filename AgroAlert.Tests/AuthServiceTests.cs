using AgroAlert.Application.DTOs;
using AgroAlert.Application.Services;
using AgroAlert.Domain.Entities;
using AgroAlert.Infrastructure.Data;
using AgroAlert.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace AgroAlert.Tests;

public class AuthServiceTests
{
    private AgroAlertDbContext CriarContexto(string nome)
    {
        var options = new DbContextOptionsBuilder<AgroAlertDbContext>()
            .UseInMemoryDatabase(nome).Options;
        return new AgroAlertDbContext(options);
    }

    private IConfiguration CriarConfig() =>
        new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Jwt:Key"] = "AgroAlertSecretKey2024!@#$%^&*()_+FIAP",
                ["Jwt:Issuer"] = "AgroAlert",
                ["Jwt:Audience"] = "AgroAlertApp"
            }).Build();

    // TESTE 6 - Login com credenciais válidas retorna token
    [Fact]
    public async Task LoginComCredenciaisValidasRetornaToken()
    {
        // Arrange
        using var ctx = CriarContexto("auth1");
        var ag = new Agricultor
        {
            Nome = "Maria", Email = "maria@test.com",
            SenhaHash = BCrypt.Net.BCrypt.HashPassword("senha123")
        };
        ctx.Agricultores.Add(ag);
        await ctx.SaveChangesAsync();

        var repo = new AgricultorRepository(ctx);
        var service = new AuthService(repo, CriarConfig());

        // Act
        var result = await service.LoginAsync(new LoginRequest { Email = "maria@test.com", Senha = "senha123" });

        // Assert
        Assert.NotNull(result);
        Assert.False(string.IsNullOrEmpty(result.Token));
        Assert.Equal("Maria", result.Nome);
    }

    // TESTE 7 - Login com senha errada retorna null
    [Fact]
    public async Task LoginComSenhaErradaRetornaNull()
    {
        // Arrange
        using var ctx = CriarContexto("auth2");
        var ag = new Agricultor
        {
            Nome = "Pedro", Email = "pedro@test.com",
            SenhaHash = BCrypt.Net.BCrypt.HashPassword("senhaCorreta")
        };
        ctx.Agricultores.Add(ag);
        await ctx.SaveChangesAsync();

        var repo = new AgricultorRepository(ctx);
        var service = new AuthService(repo, CriarConfig());

        // Act
        var result = await service.LoginAsync(new LoginRequest { Email = "pedro@test.com", Senha = "senhaErrada" });

        // Assert
        Assert.Null(result);
    }

    // TESTE 8 - Registro com email duplicado retorna null
    [Fact]
    public async Task RegistroComEmailDuplicadoRetornaNull()
    {
        // Arrange
        using var ctx = CriarContexto("auth3");
        var ag = new Agricultor
        {
            Nome = "Ana", Email = "ana@test.com",
            SenhaHash = BCrypt.Net.BCrypt.HashPassword("senha")
        };
        ctx.Agricultores.Add(ag);
        await ctx.SaveChangesAsync();

        var repo = new AgricultorRepository(ctx);
        var service = new AuthService(repo, CriarConfig());

        // Act
        var result = await service.RegisterAsync(new RegisterRequest
        {
            Nome = "Ana Duplicada", Email = "ana@test.com", Senha = "outraSenha"
        });

        // Assert
        Assert.Null(result);
    }
}
