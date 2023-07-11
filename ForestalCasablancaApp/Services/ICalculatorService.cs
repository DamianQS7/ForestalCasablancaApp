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
        bool CheckPalomera(double ancho, double alto, out double medidaPalomera);
        double CalculateAlturaMedia(List<double> alturas);
        void CalculateTotalMetrosLeña(DespachoLeñaModel model);
    }
}
