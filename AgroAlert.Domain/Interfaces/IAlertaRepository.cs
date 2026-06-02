using AgroAlert.Domain.Entities;
using AgroAlert.Domain.Enums;
namespace AgroAlert.Domain.Interfaces;
public interface IAlertaRepository
{
    Task<IEnumerable<Alerta>> GetAllAsync();
    Task<IEnumerable<Alerta>> GetByPropriedadeIdAsync(int propriedadeId);
    Task<IEnumerable<Alerta>> GetByNivelRiscoAsync(NivelRisco nivel);
    Task<Alerta?> GetByIdAsync(int id);
    Task<Alerta> AddAsync(Alerta alerta);
    Task<Alerta> UpdateAsync(Alerta alerta);
}
