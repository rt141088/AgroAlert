namespace AgroAlert.Domain.Entities;
public class Agricultor
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string SenhaHash { get; set; } = string.Empty;
    public string Telefone { get; set; } = string.Empty;
    public string CPF { get; set; } = string.Empty;
    public DateTime DataCadastro { get; set; } = DateTime.UtcNow;
    public bool Ativo { get; set; } = true;
    public ICollection<Propriedade> Propriedades { get; set; } = new List<Propriedade>();
    public ICollection<HistoricoAcesso> HistoricoAcessos { get; set; } = new List<HistoricoAcesso>();
}
