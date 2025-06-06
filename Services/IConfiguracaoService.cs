using MonitoramentoAlagamentos.Models;

namespace MonitoramentoAlagamentos.Services
{
    public interface IConfiguracaoService
    {
        Configuracao ObterConfiguracao();
        void AtualizarConfiguracao(Configuracao configuracao);
    }
} 