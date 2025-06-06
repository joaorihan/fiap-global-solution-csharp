using System;

namespace MonitoramentoAlagamentos.Models
{
    /// <summary>
    /// Representa uma zona de risco de alagamento no sistema.
    /// Contém informações sobre localização, níveis de risco e monitoramento.
    /// </summary>
    public class ZonaRisco
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Bairro { get; set; }
        public string Regiao { get; set; }
        
        public NivelRisco NivelRisco { get; set; }
        public double NivelPluviometrico { get; set; }
        public DateTime UltimaAtualizacao { get; set; }

        /// <summary>
        /// Cria uma nova zona de risco com os dados básicos de localização.
        /// Inicializa com nível de risco baixo e sem registro de chuva.
        /// </summary>
        public ZonaRisco(int id, string nome, string bairro, string regiao)
        {
            Id = id;
            Nome = nome;
            Bairro = bairro;
            Regiao = regiao;
            NivelRisco = NivelRisco.Baixo;
            UltimaAtualizacao = DateTime.Now;
            NivelPluviometrico = 0;
        }

        /// <summary>
        /// Atualiza o nível pluviométrico da zona e recalcula o nível de risco.
        /// </summary>
        public void AtualizarNivelPluviometrico(double nivel)
        {
            NivelPluviometrico = nivel;
            UltimaAtualizacao = DateTime.Now;
            AtualizarNivelRisco();
        }
        

        /// <summary>
        /// Atualiza o nível de risco com base no nível pluviométrico atual.
        /// Os níveis são definidos conforme a quantidade de chuva em milímetros.
        /// </summary>
        private void AtualizarNivelRisco()
        {
            NivelRisco = NivelPluviometrico switch
            {
                < 20 => NivelRisco.Baixo,
                < 50 => NivelRisco.Medio,
                < 80 => NivelRisco.Alto,
                _ => NivelRisco.Critico
            };
            
        }
        
    }
    
} 