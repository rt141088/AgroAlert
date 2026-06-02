using AgroAlert.Domain.Enums;
namespace AgroAlert.Domain.Entities;
public class RegraAlerta
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public TipoAlerta TipoAlerta { get; set; }
    public string Parametro { get; set; } = string.Empty;
    public string Operador { get; set; } = string.Empty;
    public double ValorLimite { get; set; }
    public NivelRisco NivelRisco { get; set; }
    public bool Ativa { get; set; } = true;
    public DateTime DataCriacao { get; set; } = DateTime.UtcNow;
    public int PropriedadeId { get; set; }
    public Propriedade? Propriedade { get; set; }
}
