namespace MonitoramentoAlagamentos.Models
{
    public enum NivelRisco
    {
        Baixo,
        Medio,
        Alto,
        Critico
    }

    public enum TipoEvento
    {
        Alerta,
        Atualizacao,
        Relatorio,
        Erro
    }

    public enum TipoUsuario
    {
        Operador,
        Supervisor,
        Administrador
    }
    
} 