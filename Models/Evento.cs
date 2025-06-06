using System;

namespace MonitoramentoAlagamentos.Models
{
    public class Evento
    {
        public int Id { get; set; }
        public DateTime DataHora { get; set; }
        
        public string Descricao { get; set; }
        public TipoEvento Tipo { get; set; }
        
        public int? ZonaId { get; set; }
        public string Responsavel { get; set; }

        public Evento(string descricao, TipoEvento tipo, int? zonaId, string responsavel)
        {
            DataHora = DateTime.Now;
            Descricao = descricao;
            Tipo = tipo;
            ZonaId = zonaId;
            Responsavel = responsavel;
        }
    }
} 
