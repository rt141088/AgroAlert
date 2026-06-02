using AgroAlert.Domain.Enums;
namespace AgroAlert.Application.DTOs;

public class RegraAlertaDTO
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string TipoAlerta { get; set; } = string.Empty;
    public string Parametro { get; set; } = string.Empty;
    public string Operador { get; set; } = string.Empty;
    public double ValorLimite { get; set; }
    public string NivelRisco { get; set; } = string.Empty;
    public bool Ativa { get; set; }
    public int PropriedadeId { get; set; }
}

public class CreateRegraAlertaRequest
{
    public string Nome { get; set; } = string.Empty;
    public TipoAlerta TipoAlerta { get; set; }
    public string Parametro { get; set; } = string.Empty;
    public string Operador { get; set; } = string.Empty;
    public double ValorLimite { get; set; }
    public NivelRisco NivelRisco { get; set; }
    public int PropriedadeId { get; set; }
}
