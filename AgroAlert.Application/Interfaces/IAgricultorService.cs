using AgroAlert.Application.DTOs;
namespace AgroAlert.Application.Interfaces;

public interface IAgricultorService
{
    Task<IEnumerable<AgricultorDTO>> GetAllAsync();
    Task<AgricultorDTO?> GetByIdAsync(int id);
    Task<AgricultorDTO> CreateAsync(CreateAgricultorRequest request);
    Task<AgricultorDTO?> UpdateAsync(int id, UpdateAgricultorRequest request);
    Task<bool> DeleteAsync(int id);
}
