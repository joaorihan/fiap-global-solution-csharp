using System.Collections.Generic;
using MonitoramentoAlagamentos.Models;

namespace MonitoramentoAlagamentos.Services
{
    public interface IUsuarioService
    {
        List<Usuario> ListarUsuarios();
        Usuario? ObterUsuarioPorLogin(string login);
        
        void CadastrarUsuario(string login, string senha, string nome, TipoUsuario tipo);
        void AtualizarUsuario(string login, string? nome, string? senha, TipoUsuario? tipo);
        void RemoverUsuario(string login);
        bool ValidarLogin(string login, string senha);
    }
} 