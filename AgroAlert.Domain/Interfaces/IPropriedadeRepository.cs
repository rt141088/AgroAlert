using AgroAlert.Domain.Entities;
namespace AgroAlert.Domain.Interfaces;
public interface IPropriedadeRepository
{
    Task<IEnumerable<Propriedade>> GetAllAsync();
    Task<IEnumerable<Propriedade>> GetByAgricultorIdAsync(int agricultorId);
    Task<Propriedade?> GetByIdAsync(int id);
    Task<Propriedade> AddAsync(Propriedade propriedade);
    Task<Propriedade> UpdateAsync(Propriedade propriedade);
    Task DeleteAsync(int id);
}
