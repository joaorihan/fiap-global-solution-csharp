using System;

namespace MonitoramentoAlagamentos.Models
{
    public class Usuario
    {
        public string Login { get; set; }
        public string Senha { get; set; }
        public string Nome { get; set; }
        public TipoUsuario Tipo { get; set; }

        public Usuario(string login, string senha, string nome, TipoUsuario tipo)
        {
            Login = login;
            Senha = senha;
            Nome = nome;
            Tipo = tipo;
        }

        public bool ValidarSenha(string senha)
        {
            return Senha == senha;
        }
    }
    
} 