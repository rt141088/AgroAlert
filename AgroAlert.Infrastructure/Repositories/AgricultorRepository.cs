using AgroAlert.Domain.Entities;
using AgroAlert.Domain.Interfaces;
using AgroAlert.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AgroAlert.Infrastructure.Repositories;

public class AgricultorRepository : IAgricultorRepository
{
    private readonly AgroAlertDbContext _ctx;
    public AgricultorRepository(AgroAlertDbContext ctx) => _ctx = ctx;

    public async Task<IEnumerable<Agricultor>> GetAllAsync()
        => await _ctx.Agricultores.Where(a => a.Ativo).ToListAsync();

    public async Task<Agricultor?> GetByIdAsync(int id)
        => await _ctx.Agricultores.FindAsync(id);

    public async Task<Agricultor?> GetByEmailAsync(string email)
        => await _ctx.Agricultores.FirstOrDefaultAsync(a => a.Email == email);

    public async Task<Agricultor> AddAsync(Agricultor a)
    {
        _ctx.Agricultores.Add(a);
        await _ctx.SaveChangesAsync();
        return a;
    }

    public async Task<Agricultor> UpdateAsync(Agricultor a)
    {
        _ctx.Agricultores.Update(a);
        await _ctx.SaveChangesAsync();
        return a;
    }

    public async Task DeleteAsync(int id)
    {
        var a = await _ctx.Agricultores.FindAsync(id);
        if (a != null) { a.Ativo = false; await _ctx.SaveChangesAsync(); }
    }
}
