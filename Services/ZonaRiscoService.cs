using System;
using System.Collections.Generic;
using System.Linq;
using MonitoramentoAlagamentos.Models;

namespace MonitoramentoAlagamentos.Services
{
    /// <summary>
    /// Serviço responsável pelo gerenciamento das zonas de risco.
    /// Implementa a lógica de negócio para cadastro, atualização e monitoramento das zonas.
    /// </summary>
    public class ZonaRiscoService : IZonaRiscoService
    {
        private readonly IConfiguracaoService _configuracaoService;
        private readonly IEventoService _eventoService;
        private readonly List<ZonaRisco> _zonas;

        
        public ZonaRiscoService(IConfiguracaoService configuracaoService)
        {
            _configuracaoService = configuracaoService;
            _eventoService = new EventoService();
            _zonas = new List<ZonaRisco>();
            
        }

        /// <summary>
        /// Retorna a lista completa de zonas de risco cadastradas.
        /// </summary>
        public List<ZonaRisco> ListarZonas()
        {
            return _zonas;
        }

        
        
        /// <summary>
        /// Busca uma zona específica pelo seu ID.
        /// Lança exceção se a zona não for encontrada.
        /// </summary>
        public ZonaRisco ObterZonaPorId(int id)
        {
            var zona = _zonas.FirstOrDefault(z => z.Id == id);
            
            if (zona == null)
                throw new Exception("Zona de risco não encontrada!");

            return zona;
        }

        /// <summary>
        /// Cadastra uma nova zona de risco no sistema.
        /// Registra um evento de atualização após o cadastro.
        /// </summary>
        public void CadastrarZona(string nome, string bairro, string regiao)
        {
            var zona = new ZonaRisco(_zonas.Count + 1, nome, bairro, regiao)
            
            {
                NivelRisco = NivelRisco.Baixo,
                UltimaAtualizacao = DateTime.Now,
                NivelPluviometrico = 0
            };

            _zonas.Add(zona);

            _eventoService.RegistrarEvento(
                $"Nova zona de risco cadastrada: {nome}",
                TipoEvento.Atualizacao,
                zona.Id
            );
            
        }

        /// <summary>
        /// Atualiza os dados de uma zona de risco existente.
        /// Registra um evento de atualização após a modificação.
        /// </summary>
        
        public void AtualizarZona(int id, string nome, string bairro, string regiao)
        {
            var zona = ObterZonaPorId(id);

            zona.Nome = nome;
            zona.Bairro = bairro;
            zona.Regiao = regiao;
            zona.UltimaAtualizacao = DateTime.Now;

            _eventoService.RegistrarEvento(
                $"Zona de risco atualizada: {nome}",
                TipoEvento.Atualizacao,
                zona.Id
            );
        }

        /// <summary>
        /// Remove uma zona de risco do sistema.
        /// Registra um evento de atualização após a remoção.
        /// </summary>
        public void RemoverZona(int id)
        {
            var zona = ObterZonaPorId(id);
            
            _zonas.Remove(zona);

            _eventoService.RegistrarEvento(
                $"Zona de risco removida: {zona.Nome}",
                TipoEvento.Atualizacao,
                null
            );
        }

        /// <summary>
        /// Registra um novo nível pluviométrico para uma zona.
        /// Atualiza o nível de risco e gera alertas se necessário.
        /// </summary>
        public void RegistrarNivelPluviometrico(int id, double nivel)
        {
            if (nivel < 0)
                throw new Exception("Nível pluviométrico não pode ser negativo!");

            var zona = ObterZonaPorId(id);
            zona.NivelPluviometrico = nivel;
            zona.UltimaAtualizacao = DateTime.Now;

            var configuracao = _configuracaoService.ObterConfiguracao();
            zona.NivelRisco = nivel switch

            {
                var n when n >= configuracao.NivelCriticoPluviometrico => NivelRisco.Critico,
                var n when n >= configuracao.NivelAltoPluviometrico => NivelRisco.Alto,
                var n when n >= configuracao.NivelMedioPluviometrico => NivelRisco.Medio,
            
                _ => NivelRisco.Baixo
            };

            var tipoEvento = nivel switch
            {
                
                var n when n >= configuracao.NivelCriticoPluviometrico => TipoEvento.Alerta,
                var n when n >= configuracao.NivelAltoPluviometrico => TipoEvento.Alerta,
                
                _ => TipoEvento.Atualizacao
                
            };

            var descricao = nivel switch
            {
                
                var n when n >= configuracao.NivelCriticoPluviometrico => $"Nível pluviométrico crítico registrado: {nivel}mm",
                var n when n >= configuracao.NivelAltoPluviometrico => $"Nível pluviométrico alto registrado: {nivel}mm",
                _ => $"Nível pluviométrico registrado: {nivel}mm"
            };

            _eventoService.RegistrarEvento(descricao, tipoEvento, zona.Id);
        }

        /// <summary>
        /// Retorna todas as zonas com um determinado nível de risco.
        /// </summary>
        public List<ZonaRisco> ObterZonasPorNivelRisco(NivelRisco nivel)
        {
            return _zonas.Where(z => z.NivelRisco == nivel).ToList();
        }

        /// <summary>
        /// Retorna as configurações atuais do sistema.
        /// </summary>
        public Configuracao ObterConfiguracao()
        {
            return _configuracaoService.ObterConfiguracao();
        }

        /// <summary>
        /// Atualiza as configurações do sistema e recalcula os níveis de risco
        /// de todas as zonas com base nos novos parâmetros.
        /// </summary>
        public void AtualizarConfiguracao(Configuracao configuracao)
        {
            _configuracaoService.AtualizarConfiguracao(configuracao);

            foreach (var zona in _zonas)
            {
                var nivel = zona.NivelPluviometrico;
                zona.NivelRisco = nivel switch
                {
                    var n when n >= configuracao.NivelCriticoPluviometrico => NivelRisco.Critico,
                    var n when n >= configuracao.NivelAltoPluviometrico => NivelRisco.Alto,
                    var n when n >= configuracao.NivelMedioPluviometrico => NivelRisco.Medio,
                
                    
                    _ => NivelRisco.Baixo
                };
            }

            _eventoService.RegistrarEvento(
                "Configurações do sistema atualizadas",
                TipoEvento.Atualizacao,
                null
            );
        }
    }
} 