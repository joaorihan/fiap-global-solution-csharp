using System.Collections.Generic;
using MonitoramentoAlagamentos.Models;

namespace MonitoramentoAlagamentos.Services
{
    public interface IZonaRiscoService
    {
        List<ZonaRisco> ListarZonas();
        ZonaRisco ObterZonaPorId(int id);
        
        
        void CadastrarZona(string nome, string bairro, string regiao);
        void AtualizarZona(int id, string nome, string bairro, string regiao);
        void RemoverZona(int id);
        
        void RegistrarNivelPluviometrico(int id, double nivel);
        
        List<ZonaRisco> ObterZonasPorNivelRisco(NivelRisco nivel);
        
        
        Configuracao ObterConfiguracao();

        void AtualizarConfiguracao(Configuracao configuracao);
    }
} 