using AgroAlert.Domain.Entities;
namespace AgroAlert.Domain.Interfaces;
public interface IDadoClimaticoRepository
{
    Task<IEnumerable<DadoClimatico>> GetByPropriedadeIdAsync(int propriedadeId);
    Task<DadoClimatico> AddAsync(DadoClimatico dado);
}
