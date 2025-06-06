using System.Text.Json;
using MonitoramentoAlagamentos.Models;

namespace MonitoramentoAlagamentos.Services
{
    /// <summary>
    /// Serviço responsável pelo gerenciamento das configurações do sistema.
    /// Implementa a persistência das configurações em arquivo JSON.
    /// </summary>
    public class ConfiguracaoService : IConfiguracaoService
    {
        
        private const string ARQUIVO_CONFIGURACAO = "appsettings.json";
        private ConfiguracaoSistema _configuracaoSistema;

        /// <summary>
        /// Inicializa o serviço carregando as configurações do arquivo.
        /// Se o arquivo não existir, cria uma nova configuração padrão.
        /// </summary>
        /// 
        public ConfiguracaoService()
        {
            CarregarConfiguracao();
        }

        /// <summary>
        /// Retorna as configurações atuais do sistema.
        /// </summary>
        public Configuracao ObterConfiguracao()
        {
            return _configuracaoSistema.Configuracoes;
        }

        /// <summary>
        /// Atualiza as configurações do sistema e salva no arquivo.
        /// </summary>
        public void AtualizarConfiguracao(Configuracao configuracao)
        {
            
            _configuracaoSistema.Configuracoes = configuracao;
            SalvarConfiguracao();
            
        }

        /// <summary>
        /// Carrega as configurações do arquivo JSON.
        /// Se houver erro na leitura, cria uma nova configuração padrão.
        /// </summary>
        private void CarregarConfiguracao()
        {
            try
            {
                var json = File.ReadAllText(ARQUIVO_CONFIGURACAO);
                
                _configuracaoSistema = JsonSerializer.Deserialize<ConfiguracaoSistema>(json) ?? new ConfiguracaoSistema();
            }
            catch
            {
                _configuracaoSistema = new ConfiguracaoSistema();
                SalvarConfiguracao();
            }
        }

        /// <summary>
        /// Salva as configurações atuais no arquivo JSON.
        /// </summary>
        private void SalvarConfiguracao()
        {
            var json = JsonSerializer.Serialize(_configuracaoSistema, new JsonSerializerOptions { WriteIndented = true });
            
            
            File.WriteAllText(ARQUIVO_CONFIGURACAO, json);
        }
    }
} 