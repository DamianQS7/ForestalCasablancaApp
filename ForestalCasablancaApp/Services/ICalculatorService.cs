using ForestalCasablancaApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForestalCasablancaApp.Services
{
    public interface ICalculatorService
    {
        bool CheckPalomera(double ancho, double alto);
        double CalculateAlturaMedia(List<double> alturas);
        void CalculateTotalMetrosLeña(DespachoLeñaModel model);
        double CalculateTrozoAserrableVolume(double diametro, int cantidad, double largo);
    }
}
