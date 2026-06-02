namespace AgroAlert.Domain.Entities;
public class DadoClimatico
{
    public int Id { get; set; }
    public double Temperatura { get; set; }
    public double Umidade { get; set; }
    public double Precipitacao { get; set; }
    public double VelocidadeVento { get; set; }
    public DateTime DataHora { get; set; } = DateTime.UtcNow;
    public string FonteDados { get; set; } = "Sensor";
    public int PropriedadeId { get; set; }
    public Propriedade? Propriedade { get; set; }
}
