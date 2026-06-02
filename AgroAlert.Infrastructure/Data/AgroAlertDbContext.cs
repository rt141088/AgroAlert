using AgroAlert.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AgroAlert.Infrastructure.Data;

public class AgroAlertDbContext : DbContext
{
    public AgroAlertDbContext(DbContextOptions<AgroAlertDbContext> options) : base(options) { }

    public DbSet<Agricultor> Agricultores => Set<Agricultor>();
    public DbSet<Propriedade> Propriedades => Set<Propriedade>();
    public DbSet<DadoClimatico> DadosClimaticos => Set<DadoClimatico>();
    public DbSet<Alerta> Alertas => Set<Alerta>();
    public DbSet<RegraAlerta> RegrasAlerta => Set<RegraAlerta>();
    public DbSet<HistoricoAcesso> HistoricoAcessos => Set<HistoricoAcesso>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Agricultor
        modelBuilder.Entity<Agricultor>(e => {
            e.ToTable("AGRICULTORES");
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).HasColumnName("ID");
            e.Property(x => x.Nome).HasColumnName("NOME").HasMaxLength(200).IsRequired();
            e.Property(x => x.Email).HasColumnName("EMAIL").HasMaxLength(200).IsRequired();
            e.Property(x => x.SenhaHash).HasColumnName("SENHA_HASH").HasMaxLength(500);
            e.Property(x => x.Telefone).HasColumnName("TELEFONE").HasMaxLength(20);
            e.Property(x => x.CPF).HasColumnName("CPF").HasMaxLength(14);
            e.Property(x => x.DataCadastro).HasColumnName("DATA_CADASTRO");
            e.Property(x => x.Ativo).HasColumnName("ATIVO");
            e.HasIndex(x => x.Email).IsUnique();
        });

        // Propriedade
        modelBuilder.Entity<Propriedade>(e => {
            e.ToTable("PROPRIEDADES");
            e.HasKey(x => x.Id);
            e.Property(x => x.Nome).HasColumnName("NOME").HasMaxLength(200).IsRequired();
            e.Property(x => x.Localizacao).HasColumnName("LOCALIZACAO").HasMaxLength(500);
            e.Property(x => x.AreaHectares).HasColumnName("AREA_HECTARES");
            e.Property(x => x.Latitude).HasColumnName("LATITUDE");
            e.Property(x => x.Longitude).HasColumnName("LONGITUDE");
            e.Property(x => x.TipoCultura).HasColumnName("TIPO_CULTURA").HasMaxLength(100);
            e.HasOne(x => x.Agricultor).WithMany(a => a.Propriedades)
                .HasForeignKey(x => x.AgricultorId).OnDelete(DeleteBehavior.Cascade);
        });

        // DadoClimatico
        modelBuilder.Entity<DadoClimatico>(e => {
            e.ToTable("DADOS_CLIMATICOS");
            e.HasKey(x => x.Id);
            e.Property(x => x.Temperatura).HasColumnName("TEMPERATURA");
            e.Property(x => x.Umidade).HasColumnName("UMIDADE");
            e.Property(x => x.Precipitacao).HasColumnName("PRECIPITACAO");
            e.Property(x => x.VelocidadeVento).HasColumnName("VELOCIDADE_VENTO");
            e.Property(x => x.DataHora).HasColumnName("DATA_HORA");
            e.Property(x => x.FonteDados).HasColumnName("FONTE_DADOS").HasMaxLength(100);
            e.HasOne(x => x.Propriedade).WithMany(p => p.DadosClimaticos)
                .HasForeignKey(x => x.PropriedadeId).OnDelete(DeleteBehavior.Cascade);
        });

        // Alerta
        modelBuilder.Entity<Alerta>(e => {
            e.ToTable("ALERTAS");
            e.HasKey(x => x.Id);
            e.Property(x => x.Titulo).HasColumnName("TITULO").HasMaxLength(300).IsRequired();
            e.Property(x => x.Descricao).HasColumnName("DESCRICAO").HasMaxLength(1000);
            e.Property(x => x.NivelRisco).HasColumnName("NIVEL_RISCO");
            e.Property(x => x.TipoAlerta).HasColumnName("TIPO_ALERTA");
            e.Property(x => x.Lido).HasColumnName("LIDO");
            e.Property(x => x.DataCriacao).HasColumnName("DATA_CRIACAO");
            e.Property(x => x.DataLeitura).HasColumnName("DATA_LEITURA");
            e.HasOne(x => x.Propriedade).WithMany(p => p.Alertas)
                .HasForeignKey(x => x.PropriedadeId).OnDelete(DeleteBehavior.Cascade);
        });

        // RegraAlerta
        modelBuilder.Entity<RegraAlerta>(e => {
            e.ToTable("REGRAS_ALERTA");
            e.HasKey(x => x.Id);
            e.Property(x => x.Nome).HasColumnName("NOME").HasMaxLength(200).IsRequired();
            e.Property(x => x.Parametro).HasColumnName("PARAMETRO").HasMaxLength(100);
            e.Property(x => x.Operador).HasColumnName("OPERADOR").HasMaxLength(10);
            e.Property(x => x.ValorLimite).HasColumnName("VALOR_LIMITE");
            e.Property(x => x.Ativa).HasColumnName("ATIVA");
            e.HasOne(x => x.Propriedade).WithMany(p => p.RegrasAlerta)
                .HasForeignKey(x => x.PropriedadeId).OnDelete(DeleteBehavior.Cascade);
        });

        // HistoricoAcesso
        modelBuilder.Entity<HistoricoAcesso>(e => {
            e.ToTable("HISTORICO_ACESSO");
            e.HasKey(x => x.Id);
            e.Property(x => x.Acao).HasColumnName("ACAO").HasMaxLength(200);
            e.Property(x => x.EnderecoIP).HasColumnName("ENDERECO_IP").HasMaxLength(50);
            e.Property(x => x.DataHora).HasColumnName("DATA_HORA");
            e.Property(x => x.Sucesso).HasColumnName("SUCESSO");
            e.HasOne(x => x.Agricultor).WithMany(a => a.HistoricoAcessos)
                .HasForeignKey(x => x.AgricultorId).OnDelete(DeleteBehavior.Cascade);
        });

        base.OnModelCreating(modelBuilder);
    }
}
