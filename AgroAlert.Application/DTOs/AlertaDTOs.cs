using AgroAlert.Domain.Enums;
namespace AgroAlert.Application.DTOs;

public class AlertaDTO
{
    public int Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public string NivelRisco { get; set; } = string.Empty;
    public string TipoAlerta { get; set; } = string.Empty;
    public bool Lido { get; set; }
    public DateTime DataCriacao { get; set; }
    public int PropriedadeId { get; set; }
    public string NomePropriedade { get; set; } = string.Empty;
}
