using AgroAlert.Application.DTOs;
using AgroAlert.Application.Interfaces;
using AgroAlert.Domain.Entities;
using AgroAlert.Domain.Interfaces;

namespace AgroAlert.Application.Services;

public class AgricultorService : IAgricultorService
{
    private readonly IAgricultorRepository _repo;

    public AgricultorService(IAgricultorRepository repo) => _repo = repo;

    public async Task<IEnumerable<AgricultorDTO>> GetAllAsync()
    {
        var lista = await _repo.GetAllAsync();
        return lista.Select(ToDTO);
    }

    public async Task<AgricultorDTO?> GetByIdAsync(int id)
    {
        var a = await _repo.GetByIdAsync(id);
        return a == null ? null : ToDTO(a);
    }

    public async Task<AgricultorDTO> CreateAsync(CreateAgricultorRequest request)
    {
        var entity = new Agricultor
        {
            Nome = request.Nome,
            Email = request.Email,
            SenhaHash = BCrypt.Net.BCrypt.HashPassword(request.Senha),
            Telefone = request.Telefone,
            CPF = request.CPF
        };
        var result = await _repo.AddAsync(entity);
        return ToDTO(result);
    }

    public async Task<AgricultorDTO?> UpdateAsync(int id, UpdateAgricultorRequest request)
    {
        var entity = await _repo.GetByIdAsync(id);
        if (entity == null) return null;
        entity.Nome = request.Nome;
        entity.Telefone = request.Telefone;
        var result = await _repo.UpdateAsync(entity);
        return ToDTO(result);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _repo.GetByIdAsync(id);
        if (entity == null) return false;
        await _repo.DeleteAsync(id);
        return true;
    }

    private static AgricultorDTO ToDTO(Agricultor a) => new()
    {
        Id = a.Id, Nome = a.Nome, Email = a.Email,
        Telefone = a.Telefone, CPF = a.CPF,
        DataCadastro = a.DataCadastro, Ativo = a.Ativo
    };
}
