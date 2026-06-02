namespace AgroAlert.Domain.Entities;
public class Propriedade
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Localizacao { get; set; } = string.Empty;
    public double AreaHectares { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string TipoCultura { get; set; } = string.Empty;
    public DateTime DataCadastro { get; set; } = DateTime.UtcNow;
    public int AgricultorId { get; set; }
    public Agricultor? Agricultor { get; set; }
    public ICollection<Alerta> Alertas { get; set; } = new List<Alerta>();
    public ICollection<DadoClimatico> DadosClimaticos { get; set; } = new List<DadoClimatico>();
    public ICollection<RegraAlerta> RegrasAlerta { get; set; } = new List<RegraAlerta>();
}
