using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AgroAlert.Application.DTOs;
using AgroAlert.Application.Interfaces;
using AgroAlert.Domain.Entities;
using AgroAlert.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace AgroAlert.Application.Services;

public class AuthService : IAuthService
{
    private readonly IAgricultorRepository _repo;
    private readonly IConfiguration _config;

    public AuthService(IAgricultorRepository repo, IConfiguration config)
    {
        _repo = repo;
        _config = config;
    }

    public async Task<LoginResponse?> LoginAsync(LoginRequest request)
    {
        var agricultor = await _repo.GetByEmailAsync(request.Email);
        if (agricultor == null || !BCrypt.Net.BCrypt.Verify(request.Senha, agricultor.SenhaHash))
            return null;

        var token = GerarToken(agricultor);
        return new LoginResponse
        {
            Token = token,
            Nome = agricultor.Nome,
            Email = agricultor.Email,
            Expiracao = DateTime.UtcNow.AddHours(8)
        };
    }

    public async Task<AgricultorDTO?> RegisterAsync(RegisterRequest request)
    {
        var existe = await _repo.GetByEmailAsync(request.Email);
        if (existe != null) return null;

        var agricultor = new Agricultor
        {
            Nome = request.Nome,
            Email = request.Email,
            SenhaHash = BCrypt.Net.BCrypt.HashPassword(request.Senha),
            Telefone = request.Telefone,
            CPF = request.CPF
        };
        var result = await _repo.AddAsync(agricultor);
        return new AgricultorDTO
        {
            Id = result.Id, Nome = result.Nome, Email = result.Email,
            Telefone = result.Telefone, CPF = result.CPF,
            DataCadastro = result.DataCadastro, Ativo = result.Ativo
        };
    }

    private string GerarToken(Agricultor agricultor)
    {
        var jwtKey = _config["Jwt:Key"] ?? "AgroAlertSecretKey2024!@#$%^&*()_+";
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, agricultor.Id.ToString()),
            new Claim(ClaimTypes.Name, agricultor.Nome),
            new Claim(ClaimTypes.Email, agricultor.Email),
            new Claim("agricultorId", agricultor.Id.ToString())
        };

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"] ?? "AgroAlert",
            audience: _config["Jwt:Audience"] ?? "AgroAlertApp",
            claims: claims,
            expires: DateTime.UtcNow.AddHours(8),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
