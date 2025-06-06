namespace MonitoramentoAlagamentos.Models
{
    public class Configuracao
    {
        public double NivelCriticoPluviometrico { get; set; }
        public double NivelAltoPluviometrico { get; set; }
        public double NivelMedioPluviometrico { get; set; }
        
        public int RetencaoEventosDias { get; set; }
    }
} 