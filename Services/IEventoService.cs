using System.Collections.Generic;
using MonitoramentoAlagamentos.Models;

namespace MonitoramentoAlagamentos.Services
{
    public interface IEventoService
    {
        List<Evento> ListarEventos();
        List<Evento> FiltrarEventosPorTipo(TipoEvento tipo);
        List<Evento> FiltrarEventosPorZona(int zonaId);
        
        
        void RegistrarEvento(string descricao, TipoEvento tipo, int? zonaId);
    }
} 