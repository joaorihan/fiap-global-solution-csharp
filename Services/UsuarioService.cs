using System;
using System.Collections.Generic;
using System.Linq;
using MonitoramentoAlagamentos.Models;

namespace MonitoramentoAlagamentos.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly List<Usuario> _usuarios;
        
        
        public UsuarioService()
        {
            _usuarios = new List<Usuario>
            {
                new Usuario("admin", "admin123", "Administrador", TipoUsuario.Administrador)
            };
        }

        public List<Usuario> ListarUsuarios()
        {
            return _usuarios;
        }

        public Usuario? ObterUsuarioPorLogin(string login)
        {
            return _usuarios.FirstOrDefault(u => u.Login == login);
        }

        public void CadastrarUsuario(string login, string senha, string nome, TipoUsuario tipo)
        {
            
            if (_usuarios.Any(u => u.Login == login))
                throw new Exception("Login já existe!");

            
            var usuario = new Usuario(login, senha, nome, tipo);
            
            _usuarios.Add(usuario);
        }

        public void AtualizarUsuario(string login, string? nome, string? senha, TipoUsuario? tipo)
        {
            var usuario = ObterUsuarioPorLogin(login);
            if (usuario == null)
                throw new Exception("Usuário não encontrado!");

            if (nome != null)
                usuario.Nome = nome;

            if (senha != null)
                usuario.Senha = senha;

            if (tipo.HasValue)
                usuario.Tipo = tipo.Value;
        }

        public void RemoverUsuario(string login)
        {
            if (login == "admin")
                throw new Exception("Não é possível remover o usuário administrador padrão!");

            var usuario = ObterUsuarioPorLogin(login);
            if (usuario == null)
                throw new Exception("Usuário não encontrado!");

            _usuarios.Remove(usuario);
        }

        public bool ValidarLogin(string login, string senha)
        {
            var usuario = ObterUsuarioPorLogin(login);
            return usuario != null && usuario.Senha == senha;
        }
    }
} 