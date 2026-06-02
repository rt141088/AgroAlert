using AgroAlert.Domain.Entities;
using AgroAlert.Domain.Enums;
using AgroAlert.Domain.Interfaces;
using AgroAlert.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AgroAlert.Infrastructure.Repositories;

public class AlertaRepository : IAlertaRepository
{
    private readonly AgroAlertDbContext _ctx;
    public AlertaRepository(AgroAlertDbContext ctx) => _ctx = ctx;

    public async Task<IEnumerable<Alerta>> GetAllAsync()
        => await _ctx.Alertas.Include(a => a.Propriedade).ToListAsync();

    public async Task<IEnumerable<Alerta>> GetByPropriedadeIdAsync(int propriedadeId)
        => await _ctx.Alertas.Include(a => a.Propriedade)
            .Where(a => a.PropriedadeId == propriedadeId).ToListAsync();

    public async Task<IEnumerable<Alerta>> GetByNivelRiscoAsync(NivelRisco nivel)
        => await _ctx.Alertas.Include(a => a.Propriedade)
            .Where(a => a.NivelRisco == nivel).ToListAsync();

    public async Task<Alerta?> GetByIdAsync(int id)
        => await _ctx.Alertas.Include(a => a.Propriedade).FirstOrDefaultAsync(a => a.Id == id);

    public async Task<Alerta> AddAsync(Alerta a)
    {
        _ctx.Alertas.Add(a);
        await _ctx.SaveChangesAsync();
        return a;
    }

    public async Task<Alerta> UpdateAsync(Alerta a)
    {
        _ctx.Alertas.Update(a);
        await _ctx.SaveChangesAsync();
        return a;
    }
}
