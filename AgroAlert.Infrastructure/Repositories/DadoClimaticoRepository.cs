using AgroAlert.Domain.Entities;
using AgroAlert.Domain.Interfaces;
using AgroAlert.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AgroAlert.Infrastructure.Repositories;

public class DadoClimaticoRepository : IDadoClimaticoRepository
{
    private readonly AgroAlertDbContext _ctx;
    public DadoClimaticoRepository(AgroAlertDbContext ctx) => _ctx = ctx;

    public async Task<IEnumerable<DadoClimatico>> GetByPropriedadeIdAsync(int propriedadeId)
        => await _ctx.DadosClimaticos.Where(d => d.PropriedadeId == propriedadeId)
            .OrderByDescending(d => d.DataHora).Take(100).ToListAsync();

    public async Task<DadoClimatico> AddAsync(DadoClimatico d)
    {
        _ctx.DadosClimaticos.Add(d);
        await _ctx.SaveChangesAsync();
        return d;
    }
}
