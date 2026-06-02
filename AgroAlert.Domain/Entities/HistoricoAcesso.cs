namespace AgroAlert.Domain.Entities;
public class HistoricoAcesso
{
    public int Id { get; set; }
    public string Acao { get; set; } = string.Empty;
    public string EnderecoIP { get; set; } = string.Empty;
    public DateTime DataHora { get; set; } = DateTime.UtcNow;
    public bool Sucesso { get; set; }
    public int AgricultorId { get; set; }
    public Agricultor? Agricultor { get; set; }
}
