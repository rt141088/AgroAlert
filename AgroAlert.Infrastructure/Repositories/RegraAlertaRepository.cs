using AgroAlert.Domain.Entities;
using AgroAlert.Domain.Interfaces;
using AgroAlert.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AgroAlert.Infrastructure.Repositories;

public class RegraAlertaRepository : IRegraAlertaRepository
{
    private readonly AgroAlertDbContext _ctx;
    public RegraAlertaRepository(AgroAlertDbContext ctx) => _ctx = ctx;

    public async Task<IEnumerable<RegraAlerta>> GetAllAsync()
        => await _ctx.RegrasAlerta.ToListAsync();

    public async Task<IEnumerable<RegraAlerta>> GetByPropriedadeIdAsync(int propriedadeId)
        => await _ctx.RegrasAlerta.Where(r => r.PropriedadeId == propriedadeId).ToListAsync();

    public async Task<RegraAlerta?> GetByIdAsync(int id)
        => await _ctx.RegrasAlerta.FindAsync(id);

    public async Task<RegraAlerta> AddAsync(RegraAlerta r)
    {
        _ctx.RegrasAlerta.Add(r);
        await _ctx.SaveChangesAsync();
        return r;
    }

    public async Task DeleteAsync(int id)
    {
        var r = await _ctx.RegrasAlerta.FindAsync(id);
        if (r != null) { _ctx.RegrasAlerta.Remove(r); await _ctx.SaveChangesAsync(); }
    }
}
