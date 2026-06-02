using AgroAlert.Domain.Entities;
namespace AgroAlert.Domain.Interfaces;
public interface IRegraAlertaRepository
{
    Task<IEnumerable<RegraAlerta>> GetAllAsync();
    Task<IEnumerable<RegraAlerta>> GetByPropriedadeIdAsync(int propriedadeId);
    Task<RegraAlerta?> GetByIdAsync(int id);
    Task<RegraAlerta> AddAsync(RegraAlerta regra);
    Task DeleteAsync(int id);
}
