using System;
using System.Collections.Generic;
using System.Linq;
using MonitoramentoAlagamentos.Models;
using MonitoramentoAlagamentos.Services;

namespace MonitoramentoAlagamentos.UI
{
    public class ConsoleUI
    {
        private readonly IZonaRiscoService _zonaRiscoService;
        private readonly IEventoService _eventoService;
        private readonly IUsuarioService _usuarioService;
        private Usuario? _usuarioAtual;

        public ConsoleUI(
            IZonaRiscoService zonaRiscoService,
            IEventoService eventoService,
            IUsuarioService usuarioService)
        {
            _zonaRiscoService = zonaRiscoService;
            _eventoService = eventoService;
            _usuarioService = usuarioService;
        }

        public void Iniciar()
        {
            Console.WriteLine("=== Sistema de Monitoramento de Alagamentos ===");
            
            while (true)
            {
                if (_usuarioAtual == null)
                {
                    if (!RealizarLogin())
                        continue;
                }

                ExibirMenuPrincipal();
                var opcao = Console.ReadLine();

                switch (opcao)
                {
                    case "1":
                        GerenciarZonas();
                        break;
                    case "2":
                        RegistrarNivelPluviometrico();
                        break;
                    case "3":
                        ConsultarEventos();
                        break;
                    case "4":
                        if (_usuarioAtual?.Tipo == TipoUsuario.Administrador)
                            GerenciarUsuarios();
                        else
                            Console.WriteLine("Acesso negado. Apenas administradores podem gerenciar usuários.");
                        break;
                    case "5":
                        if (_usuarioAtual?.Tipo == TipoUsuario.Administrador)
                            GerenciarConfiguracoes();
                        else
                            Console.WriteLine("Acesso negado. Apenas administradores podem gerenciar configurações.");
                        break;
                    case "6":
                        _usuarioAtual = null;
                        Console.WriteLine("Logout realizado com sucesso!");
                        break;
                    case "7":
                        RegistrarEvento();
                        break;
                    case "0":
                        Console.WriteLine("Encerrando o sistema...");
                        return;
                    default:
                        Console.WriteLine("Opção inválida!");
                        break;
                }

                Console.WriteLine("\nPressione qualquer tecla para continuar...");
                Console.ReadKey();
                Console.Clear();
            }
        }

        private bool RealizarLogin()
        {
            
            Console.WriteLine("\n=== Login ===");
            Console.Write("Login: ");
            var login = Console.ReadLine();
            
            Console.Write("Senha: ");
            var senha = Console.ReadLine();

            
            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(senha))
            {
                Console.WriteLine("Login e senha são obrigatórios!");
                return false;
            }
            

            if (_usuarioService.ValidarLogin(login, senha))
            {
                _usuarioAtual = _usuarioService.ObterUsuarioPorLogin(login);
                Console.WriteLine($"Bem-vindo, {_usuarioAtual.Nome}!");
                return true;
            }

            
            Console.WriteLine("Login ou senha inválidos!");
            return false;
        }

        private void ExibirMenuPrincipal()
        {
            Console.WriteLine("\n=== Menu Principal ===");
            Console.WriteLine("1. Gerenciar Zonas de Risco");
            Console.WriteLine("2. Registrar Nível Pluviométrico");
            Console.WriteLine("3. Consultar Eventos");
            Console.WriteLine("4. Gerenciar Usuários");
            Console.WriteLine("5. Gerenciar Configurações");
            Console.WriteLine("6. Registrar Evento");
            Console.WriteLine("0. Sair");
            
            Console.Write("Escolha uma opção: ");
        }

        private void GerenciarZonas()
        {
            while (true)
            {
                
                Console.WriteLine("\n=== Gerenciar Zonas de Risco ===");
                Console.WriteLine("1. Listar Zonas");
                Console.WriteLine("2. Cadastrar Nova Zona");
                Console.WriteLine("3. Atualizar Zona");
                Console.WriteLine("4. Remover Zona");
                Console.WriteLine("5. Filtrar por Nível de Risco");
                Console.WriteLine("0. Voltar");
                
                Console.Write("\nEscolha uma opção: ");

                var opcao = Console.ReadLine();
                
                
                switch (opcao)
                {
                    case "1":
                        ListarZonas();
                        break;
                    case "2":
                        CadastrarZona();
                        break;
                    case "3":
                        AtualizarZona();
                        break;
                    case "4":
                        RemoverZona();
                        break;
                    case "5":
                        FiltrarZonasPorNivelRisco();
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Opção inválida!");
                        break;
                }
            }
        }

        private void ListarZonas()
        {
            var zonas = _zonaRiscoService.ListarZonas();
            
            if (!zonas.Any())
            {
                Console.WriteLine("Nenhuma zona cadastrada.");
                return;
            }

            Console.WriteLine("\nZonas de Risco:");
            
            foreach (var zona in zonas)
            {
                Console.WriteLine($"ID: {zona.Id}");
                Console.WriteLine($"Nome: {zona.Nome}");
                Console.WriteLine($"Bairro: {zona.Bairro}");
                Console.WriteLine($"Região: {zona.Regiao}");
                Console.WriteLine($"Nível de Risco: {zona.NivelRisco}");
                Console.WriteLine($"Nível Pluviométrico: {zona.NivelPluviometrico:F2}mm");
                Console.WriteLine("------------------------");
            }
        }

        private void CadastrarZona()
        {
            Console.WriteLine("\n=== Cadastrar Nova Zona ===");
            
            
            Console.Write("Nome: ");
            var nome = Console.ReadLine();
            
            Console.Write("Bairro: ");
            var bairro = Console.ReadLine();
            
            Console.Write("Região: ");
            var regiao = Console.ReadLine();

            if (string.IsNullOrEmpty(nome) || string.IsNullOrEmpty(bairro) || string.IsNullOrEmpty(regiao))
            {
                Console.WriteLine("Todos os campos são obrigatórios!");
                return;
            }

            try
            {
                _zonaRiscoService.CadastrarZona(nome, bairro, regiao);
                Console.WriteLine("Zona cadastrada com sucesso!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao cadastrar zona: {ex.Message}");
            }
        }
        
        
        
        
        private void AtualizarZona()
        {
            
            Console.WriteLine("\n=== Atualizar Zona ===");
            
            Console.Write("ID da zona: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("ID inválido!");
                return;
            }

            Console.Write("Novo nome: ");
            var nome = Console.ReadLine();
            
            Console.Write("Novo bairro: ");
            var bairro = Console.ReadLine();
            
            Console.Write("Nova região: ");
            var regiao = Console.ReadLine();
            
            
            if (string.IsNullOrEmpty(nome) || string.IsNullOrEmpty(bairro) || string.IsNullOrEmpty(regiao))
            {
                Console.WriteLine("Todos os campos são obrigatórios!");
                return;
            }

            try
            {
                _zonaRiscoService.AtualizarZona(id, nome, bairro, regiao);
                Console.WriteLine("Zona atualizada com sucesso!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao atualizar zona: {ex.Message}");
            }
        }

        
        private void RemoverZona()
        {
            Console.WriteLine("\n=== Remover Zona ===");
            Console.Write("ID da zona: ");
            
            
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("ID inválido!");
                return;
            }

            try
            {
                _zonaRiscoService.RemoverZona(id);
                Console.WriteLine("Zona removida com sucesso!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao remover zona: {ex.Message}");
            }
        }

        private void FiltrarZonasPorNivelRisco()
        {
            Console.WriteLine("\n=== Filtrar por Nível de Risco ===");
            Console.WriteLine("1. Baixo");
            Console.WriteLine("2. Médio");
            Console.WriteLine("3. Alto");
            Console.WriteLine("4. Crítico");
            
            Console.Write("\nEscolha o nível: ");

            
            if (!int.TryParse(Console.ReadLine(), out int opcao) || opcao < 1 || opcao > 4)
            {
                Console.WriteLine("Opção inválida!");
                return;
            }

            var nivel = (NivelRisco)(opcao - 1);
            var zonas = _zonaRiscoService.ObterZonasPorNivelRisco(nivel);

            if (!zonas.Any())
            {
                Console.WriteLine($"Nenhuma zona com nível de risco {nivel}.");
                return;
            }

            Console.WriteLine($"\nZonas com nível de risco {nivel}:");
            foreach (var zona in zonas)
            {
                Console.WriteLine($"ID: {zona.Id}");
                Console.WriteLine($"Nome: {zona.Nome}");
                Console.WriteLine($"Bairro: {zona.Bairro}");
                Console.WriteLine($"Região: {zona.Regiao}");
                Console.WriteLine($"Nível Pluviométrico: {zona.NivelPluviometrico:F2}mm");
                Console.WriteLine("------------------------");
            }
        }

        private void RegistrarNivelPluviometrico()
        {
            Console.WriteLine("\n=== Registrar Nível Pluviométrico ===");
            Console.Write("ID da zona: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("ID inválido!");
                return;
            }

            Console.Write("Nível pluviométrico (mm): ");
            if (!double.TryParse(Console.ReadLine(), out double nivel))
            {
                Console.WriteLine("Nível inválido!");
                return;
            }

            try
            {
                _zonaRiscoService.RegistrarNivelPluviometrico(id, nivel);
                Console.WriteLine("Nível pluviométrico registrado com sucesso!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao registrar nível pluviométrico: {ex.Message}");
            }
        }

        private void ConsultarEventos()
        {
            while (true)
            {
                Console.WriteLine("\n=== Consultar Eventos ===");
                Console.WriteLine("1. Listar Todos os Eventos");
                Console.WriteLine("2. Filtrar por Tipo");
                Console.WriteLine("3. Filtrar por Zona");
                Console.WriteLine("0. Voltar");
                
                Console.Write("\nEscolha uma opção: ");

                
                var opcao = Console.ReadLine();
                switch (opcao)
                {
                    case "1":
                        ListarEventos();
                        break;
                    case "2":
                        FiltrarEventosPorTipo();
                        break;
                    case "3":
                        FiltrarEventosPorZona();
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Opção inválida!");
                        break;
                }
            }
        }

        private void ListarEventos()
        {
            var eventos = _eventoService.ListarEventos();
            if (!eventos.Any())
            {
                Console.WriteLine("Nenhum evento registrado.");
                return;
            }

            
            Console.WriteLine("\nEventos:");
            foreach (var evento in eventos)
            {
                Console.WriteLine($"Data/Hora: {evento.DataHora}");
                Console.WriteLine($"Tipo: {evento.Tipo}");
                Console.WriteLine($"Descrição: {evento.Descricao}");
                if (evento.ZonaId.HasValue)
                    Console.WriteLine($"Zona ID: {evento.ZonaId}");
                Console.WriteLine($"Responsável: {evento.Responsavel}");
                Console.WriteLine("------------------------");
            }
        }

        private void FiltrarEventosPorTipo()
        {
            Console.WriteLine("\n=== Filtrar por Tipo ===");
            Console.WriteLine("1. Alerta");
            Console.WriteLine("2. Atualização");
            Console.WriteLine("3. Relatório");
            Console.WriteLine("4. Erro");
            
            
            Console.Write("\nEscolha o tipo: ");

            if (!int.TryParse(Console.ReadLine(), out int opcao) || opcao < 1 || opcao > 4)
            {
                Console.WriteLine("Opção inválida!");
                return;
            }

            var tipo = (TipoEvento)(opcao - 1);
            var eventos = _eventoService.FiltrarEventosPorTipo(tipo);

            if (!eventos.Any())
            {
                Console.WriteLine($"Nenhum evento do tipo {tipo}.");
                return;
            }

            Console.WriteLine($"\nEventos do tipo {tipo}:");
            foreach (var evento in eventos)
            {
                Console.WriteLine($"Data/Hora: {evento.DataHora}");
                Console.WriteLine($"Descrição: {evento.Descricao}");
                if (evento.ZonaId.HasValue)
                    Console.WriteLine($"Zona ID: {evento.ZonaId}");
                Console.WriteLine($"Responsável: {evento.Responsavel}");
                
                Console.WriteLine("------------------------");
            }
        }

        private void FiltrarEventosPorZona()
        {
            Console.WriteLine("\n=== Filtrar por Zona ===");
            Console.Write("ID da zona: ");
            if (!int.TryParse(Console.ReadLine(), out int zonaId))
            {
                Console.WriteLine("ID inválido!");
                return;
            }

            var eventos = _eventoService.FiltrarEventosPorZona(zonaId);

            if (!eventos.Any())
            {
                Console.WriteLine($"Nenhum evento registrado para a zona {zonaId}.");
                return;
            }

            Console.WriteLine($"\nEventos da zona {zonaId}:");
            foreach (var evento in eventos)
            {
                Console.WriteLine($"Data/Hora: {evento.DataHora}");
                Console.WriteLine($"Tipo: {evento.Tipo}");
                Console.WriteLine($"Descrição: {evento.Descricao}");
                Console.WriteLine($"Responsável: {evento.Responsavel}");
                Console.WriteLine("------------------------");
                
            }
        }

        private void GerenciarUsuarios()
        {
            while (true)
            {
                Console.WriteLine("\n=== Gerenciar Usuários ===");
                Console.WriteLine("1. Listar Usuários");
                Console.WriteLine("2. Cadastrar Usuário");
                Console.WriteLine("3. Atualizar Usuário");
                Console.WriteLine("4. Remover Usuário");
                Console.WriteLine("0. Voltar");
                
                
                Console.Write("\nEscolha uma opção: ");

                var opcao = Console.ReadLine();
                switch (opcao)
                {
                    case "1":
                        ListarUsuarios();
                        break;
                    case "2":
                        CadastrarUsuario();
                        break;
                    case "3":
                        AtualizarUsuario();
                        break;
                    case "4":
                        RemoverUsuario();
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Opção inválida!");
                        break;
                }
                
            }
            
        }

        private void ListarUsuarios()
        {
            var usuarios = _usuarioService.ListarUsuarios();
            if (!usuarios.Any())
            {
                Console.WriteLine("Nenhum usuário cadastrado.");
                return;
            }

            Console.WriteLine("\nUsuários:");
            foreach (var usuario in usuarios)
            {
                Console.WriteLine($"Login: {usuario.Login}");
                Console.WriteLine($"Nome: {usuario.Nome}");
                Console.WriteLine($"Tipo: {usuario.Tipo}");
                Console.WriteLine("------------------------");
            }
        }

        private void CadastrarUsuario()
        {
            Console.WriteLine("\n=== Cadastrar Usuário ===");
            Console.Write("Login: ");
            var login = Console.ReadLine();
            Console.Write("Senha: ");
            var senha = Console.ReadLine();
            Console.Write("Nome: ");
            var nome = Console.ReadLine();

            Console.WriteLine("\nTipo de Usuário:");
            Console.WriteLine("1. Operador");
            Console.WriteLine("2. Supervisor");
            Console.WriteLine("3. Administrador");
            Console.Write("\nEscolha o tipo: ");

            if (!int.TryParse(Console.ReadLine(), out int opcao) || opcao < 1 || opcao > 3)
            {
                Console.WriteLine("Opção inválida!");
                return;
            }

            var tipo = (TipoUsuario)(opcao - 1);

            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(senha) || string.IsNullOrEmpty(nome))
            {
                Console.WriteLine("Todos os campos são obrigatórios!");
                return;
            }

            try
            {
                _usuarioService.CadastrarUsuario(login, senha, nome, tipo);
                Console.WriteLine("Usuário cadastrado com sucesso!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao cadastrar usuário: {ex.Message}");
            }
        }

        private void AtualizarUsuario()
        {
            Console.WriteLine("\n=== Atualizar Usuário ===");
            Console.Write("Login do usuário: ");
            var login = Console.ReadLine();

            if (string.IsNullOrEmpty(login))
            {
                Console.WriteLine("Login é obrigatório!");
                return;
            }

            Console.Write("Novo nome (deixe em branco para manter): ");
            var nome = Console.ReadLine();
            Console.Write("Nova senha (deixe em branco para manter): ");
            var senha = Console.ReadLine();

            Console.WriteLine("\nNovo tipo de usuário (deixe em branco para manter):");
            Console.WriteLine("1. Operador");
            Console.WriteLine("2. Supervisor");
            Console.WriteLine("3. Administrador");
            
            Console.Write("\nEscolha o tipo: ");

            TipoUsuario? tipo = null;
            var tipoStr = Console.ReadLine();
            if (!string.IsNullOrEmpty(tipoStr) && int.TryParse(tipoStr, out int opcao) && opcao >= 1 && opcao <= 3)
            {
                tipo = (TipoUsuario)(opcao - 1);
            }

            try
            {
                _usuarioService.AtualizarUsuario(login, nome, senha, tipo);
                Console.WriteLine("Usuário atualizado com sucesso!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao atualizar usuário: {ex.Message}");
            }
            
        }

        private void RemoverUsuario()
        {
            Console.WriteLine("\n=== Remover Usuário ===");
            
            Console.Write("Login do usuário: ");
            var login = Console.ReadLine();

            
            if (string.IsNullOrEmpty(login))
            {
                Console.WriteLine("Login é obrigatório!");
                return;
            }

            try
            {
                _usuarioService.RemoverUsuario(login);
                Console.WriteLine("Usuário removido com sucesso!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao remover usuário: {ex.Message}");
            }
        }

        private void GerenciarConfiguracoes()
        {
            while (true)
            {
                Console.WriteLine("\n=== Gerenciar Configurações ===");
                Console.WriteLine("1. Visualizar Configurações");
                Console.WriteLine("2. Atualizar Configurações");
                Console.WriteLine("0. Voltar");
                
                Console.Write("\nEscolha uma opção: ");
                

                var opcao = Console.ReadLine();
                
                switch (opcao)
                {
                    case "1":
                        VisualizarConfiguracoes();
                        break;
                    case "2":
                        AtualizarConfiguracoes();
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Opção inválida!");
                        break;
                }
            }
        }

        private void VisualizarConfiguracoes()
        {
            var config = _zonaRiscoService.ObterConfiguracao();
            
            Console.WriteLine("\nConfigurações Atuais:");
            Console.WriteLine($"Nível Crítico: {config.NivelCriticoPluviometrico:F2}mm");
            Console.WriteLine($"Nível Alto: {config.NivelAltoPluviometrico:F2}mm");
            Console.WriteLine($"Nível Médio: {config.NivelMedioPluviometrico:F2}mm");
            Console.WriteLine($"Retenção de Eventos: {config.RetencaoEventosDias} dias");
            
        }

        private void AtualizarConfiguracoes()
        {
            
            Console.WriteLine("\n=== Atualizar Configurações ===");
            Console.Write("Nível Crítico (mm): ");
            if (!double.TryParse(Console.ReadLine(), out double nivelCritico))
            {
                Console.WriteLine("Valor inválido!");
                return;
                
            }

            Console.Write("Nível Alto (mm): ");
            if (!double.TryParse(Console.ReadLine(), out double nivelAlto))
            {
                Console.WriteLine("Valor inválido!");
                return;
            }

            Console.Write("Nível Médio (mm): ");
            if (!double.TryParse(Console.ReadLine(), out double nivelMedio))
            {
                Console.WriteLine("Valor inválido!");
                return;
            }

            Console.Write("Retenção de Eventos (dias): ");
            if (!int.TryParse(Console.ReadLine(), out int retencao))
            {
                Console.WriteLine("Valor inválido!");
                return;
            }

            try
            {
                
                var config = new Configuracao
                {
                    NivelCriticoPluviometrico = nivelCritico,
                    NivelAltoPluviometrico = nivelAlto,
                    NivelMedioPluviometrico = nivelMedio,
                    RetencaoEventosDias = retencao
                    
                };

                _zonaRiscoService.AtualizarConfiguracao(config);
                Console.WriteLine("Configurações atualizadas com sucesso!");
                
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao atualizar configurações: {ex.Message}");
            }
        }

        public void RegistrarEvento()
        {
            Console.WriteLine("\n=== Registrar Evento ===");
            Console.Write("Descrição do evento: ");
            string descricao = Console.ReadLine();
            
            Console.Write("Tipo do evento (Alerta, Atualizacao, Relatorio, Erro): ");
            if (Enum.TryParse(Console.ReadLine(), out TipoEvento tipo))
            {
                Console.Write("ID da zona de risco (ou pressione Enter para nenhuma): ");
                if (int.TryParse(Console.ReadLine(), out int zonaId))
                    
                {
                    _eventoService.RegistrarEvento(descricao, tipo, zonaId);
                    Console.WriteLine("Evento registrado com sucesso!");
                }
                else
                {
                    _eventoService.RegistrarEvento(descricao, tipo, null);
                    Console.WriteLine("Evento registrado com sucesso!");
                }
                
            }
            else
            {
                Console.WriteLine("Tipo de evento inválido.");
            }
            
        }
        
        
        
    }
    
    
    
} 