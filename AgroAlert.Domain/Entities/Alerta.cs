using AgroAlert.Domain.Enums;
namespace AgroAlert.Domain.Entities;
public class Alerta
{
    public int Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public NivelRisco NivelRisco { get; set; }
    public TipoAlerta TipoAlerta { get; set; }
    public bool Lido { get; set; } = false;
    public DateTime DataCriacao { get; set; } = DateTime.UtcNow;
    public DateTime? DataLeitura { get; set; }
    public int PropriedadeId { get; set; }
    public Propriedade? Propriedade { get; set; }
}
