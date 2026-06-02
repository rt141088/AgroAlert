using AgroAlert.Application.DTOs;
using AgroAlert.Domain.Enums;
namespace AgroAlert.Application.Interfaces;

public interface IAlertaService
{
    Task<IEnumerable<AlertaDTO>> GetAllAsync(int? propriedadeId, NivelRisco? nivelRisco);
    Task<AlertaDTO?> GetByIdAsync(int id);
    Task ProcessarDadosClimaticosAsync(DadoClimaticoDTO dado);
}
