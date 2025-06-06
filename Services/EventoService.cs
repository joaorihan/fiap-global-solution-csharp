using System;
using System.Collections.Generic;
using System.Linq;
using MonitoramentoAlagamentos.Models;

namespace MonitoramentoAlagamentos.Services
{
    public class EventoService : IEventoService
    {
        private readonly List<Evento> _eventos;

        public EventoService()
        {
            _eventos = new List<Evento>();
            
        }

        public List<Evento> ListarEventos()
        {
            return _eventos.OrderByDescending(e => e.DataHora).ToList();
        }

        public List<Evento> FiltrarEventosPorTipo(TipoEvento tipo)
        {
            return _eventos
                .Where(e => e.Tipo == tipo)
                .OrderByDescending(e => e.DataHora)
                .ToList();
            
        }

        public List<Evento> FiltrarEventosPorZona(int zonaId)
        {
            return _eventos
                .Where(e => e.ZonaId == zonaId)
                .OrderByDescending(e => e.DataHora)
                .ToList();
        }

        public void RegistrarEvento(string descricao, TipoEvento tipo, int? zonaId)
        {
            var evento = new Evento(descricao, tipo, zonaId, "Sistema")
            {
                Id = _eventos.Count + 1
                
            };
            
            
            _eventos.Add(evento);
        }
    }
} 