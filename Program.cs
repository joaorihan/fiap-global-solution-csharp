using MonitoramentoAlagamentos.Services;
using MonitoramentoAlagamentos.UI;

namespace MonitoramentoAlagamentos
{
    /// <summary>
    /// Classe principal do sistema de monitoramento de alagamentos.
    /// Responsável por inicializar os serviços e a interface do usuário.
    /// </summary>
    public class Program
    {
        public static void Main(string[] args)
        {
            // Inicializa os serviços necessários para o funcionamento do sistema
            var configuracaoService = new ConfiguracaoService();
            var zonaRiscoService = new ZonaRiscoService(configuracaoService);
            var eventoService = new EventoService();
            var usuarioService = new UsuarioService();

            // Cria e inicia a interface do usuário
            var consoleUI = new ConsoleUI(zonaRiscoService, eventoService, usuarioService);
            consoleUI.Iniciar();
        }
    }
} 