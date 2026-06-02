namespace AgroAlert.Application.DTOs;

public class PropriedadeDTO
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Localizacao { get; set; } = string.Empty;
    public double AreaHectares { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string TipoCultura { get; set; } = string.Empty;
    public int AgricultorId { get; set; }
    public string NomeAgricultor { get; set; } = string.Empty;
}

public class CreatePropriedadeRequest
{
    public string Nome { get; set; } = string.Empty;
    public string Localizacao { get; set; } = string.Empty;
    public double AreaHectares { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string TipoCultura { get; set; } = string.Empty;
    public int AgricultorId { get; set; }
}
