using AgroAlert.Application.DTOs;
using AgroAlert.Application.Interfaces;
using AgroAlert.Domain.Entities;
using AgroAlert.Domain.Enums;
using AgroAlert.Domain.Interfaces;

namespace AgroAlert.Application.Services;

public class AlertaService : IAlertaService
{
    private readonly IAlertaRepository _alertaRepo;
    private readonly IRegraAlertaRepository _regraRepo;

    public AlertaService(IAlertaRepository alertaRepo, IRegraAlertaRepository regraRepo)
    {
        _alertaRepo = alertaRepo;
        _regraRepo = regraRepo;
    }

    public async Task<IEnumerable<AlertaDTO>> GetAllAsync(int? propriedadeId, NivelRisco? nivelRisco)
    {
        IEnumerable<Alerta> alertas;
        if (propriedadeId.HasValue)
            alertas = await _alertaRepo.GetByPropriedadeIdAsync(propriedadeId.Value);
        else if (nivelRisco.HasValue)
            alertas = await _alertaRepo.GetByNivelRiscoAsync(nivelRisco.Value);
        else
            alertas = await _alertaRepo.GetAllAsync();
        return alertas.Select(ToDTO);
    }

    public async Task<AlertaDTO?> GetByIdAsync(int id)
    {
        var a = await _alertaRepo.GetByIdAsync(id);
        return a == null ? null : ToDTO(a);
    }

    public async Task ProcessarDadosClimaticosAsync(DadoClimaticoDTO dado)
    {
        var regras = await _regraRepo.GetByPropriedadeIdAsync(dado.PropriedadeId);
        foreach (var regra in regras.Where(r => r.Ativa))
        {
            double valorAtual = regra.Parametro.ToLower() switch
            {
                "temperatura" => dado.Temperatura,
                "umidade" => dado.Umidade,
                "precipitacao" => dado.Precipitacao,
                "vento" => dado.VelocidadeVento,
                _ => 0
            };

            bool disparar = regra.Operador switch
            {
                ">" => valorAtual > regra.ValorLimite,
                "<" => valorAtual < regra.ValorLimite,
                ">=" => valorAtual >= regra.ValorLimite,
                "<=" => valorAtual <= regra.ValorLimite,
                "==" => Math.Abs(valorAtual - regra.ValorLimite) < 0.001,
                _ => false
            };

            if (disparar)
            {
                var alerta = new Alerta
                {
                    Titulo = $"Alerta: {regra.Nome}",
                    Descricao = $"{regra.Parametro} {regra.Operador} {regra.ValorLimite}. Valor atual: {valorAtual}",
                    NivelRisco = regra.NivelRisco,
                    TipoAlerta = regra.TipoAlerta,
                    PropriedadeId = dado.PropriedadeId
                };
                await _alertaRepo.AddAsync(alerta);
            }
        }
    }

    private static AlertaDTO ToDTO(Alerta a) => new()
    {
        Id = a.Id, Titulo = a.Titulo, Descricao = a.Descricao,
        NivelRisco = a.NivelRisco.ToString(), TipoAlerta = a.TipoAlerta.ToString(),
        Lido = a.Lido, DataCriacao = a.DataCriacao,
        PropriedadeId = a.PropriedadeId,
        NomePropriedade = a.Propriedade?.Nome ?? string.Empty
    };
}
