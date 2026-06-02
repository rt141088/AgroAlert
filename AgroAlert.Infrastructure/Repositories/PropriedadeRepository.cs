using AgroAlert.Domain.Entities;
using AgroAlert.Domain.Interfaces;
using AgroAlert.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AgroAlert.Infrastructure.Repositories;

public class PropriedadeRepository : IPropriedadeRepository
{
    private readonly AgroAlertDbContext _ctx;
    public PropriedadeRepository(AgroAlertDbContext ctx) => _ctx = ctx;

    public async Task<IEnumerable<Propriedade>> GetAllAsync()
        => await _ctx.Propriedades.Include(p => p.Agricultor).ToListAsync();

    public async Task<IEnumerable<Propriedade>> GetByAgricultorIdAsync(int agricultorId)
        => await _ctx.Propriedades.Where(p => p.AgricultorId == agricultorId).ToListAsync();

    public async Task<Propriedade?> GetByIdAsync(int id)
        => await _ctx.Propriedades.Include(p => p.Agricultor).FirstOrDefaultAsync(p => p.Id == id);

    public async Task<Propriedade> AddAsync(Propriedade p)
    {
        _ctx.Propriedades.Add(p);
        await _ctx.SaveChangesAsync();
        return p;
    }

    public async Task<Propriedade> UpdateAsync(Propriedade p)
    {
        _ctx.Propriedades.Update(p);
        await _ctx.SaveChangesAsync();
        return p;
    }

    public async Task DeleteAsync(int id)
    {
        var p = await _ctx.Propriedades.FindAsync(id);
        if (p != null) { _ctx.Propriedades.Remove(p); await _ctx.SaveChangesAsync(); }
    }
}
