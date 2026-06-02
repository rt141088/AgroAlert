namespace AgroAlert.Application.DTOs;

public class DadoClimaticoDTO
{
    public int Id { get; set; }
    public double Temperatura { get; set; }
    public double Umidade { get; set; }
    public double Precipitacao { get; set; }
    public double VelocidadeVento { get; set; }
    public DateTime DataHora { get; set; }
    public string FonteDados { get; set; } = string.Empty;
    public int PropriedadeId { get; set; }
}

public class CreateDadoClimaticoRequest
{
    public double Temperatura { get; set; }
    public double Umidade { get; set; }
    public double Precipitacao { get; set; }
    public double VelocidadeVento { get; set; }
    public int PropriedadeId { get; set; }
    public string FonteDados { get; set; } = "Sensor";
}
