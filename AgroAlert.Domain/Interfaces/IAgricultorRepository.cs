using AgroAlert.Domain.Entities;
namespace AgroAlert.Domain.Interfaces;
public interface IAgricultorRepository
{
    Task<IEnumerable<Agricultor>> GetAllAsync();
    Task<Agricultor?> GetByIdAsync(int id);
    Task<Agricultor?> GetByEmailAsync(string email);
    Task<Agricultor> AddAsync(Agricultor agricultor);
    Task<Agricultor> UpdateAsync(Agricultor agricultor);
    Task DeleteAsync(int id);
}
