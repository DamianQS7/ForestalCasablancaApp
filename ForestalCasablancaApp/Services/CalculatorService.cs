using ForestalCasablancaApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForestalCasablancaApp.Services
{
    public class CalculatorService : ICalculatorService
    {
        public double CalculateAlturaMedia(List<double> alturas)
        {
            int count = 0;
            double sum = 0;

            foreach (double altura in alturas)
            {
                if (altura > 0)
                {
                    count++;
                    sum += altura;
                }
            }

            return sum / count;
        }

        public bool CheckPalomera(double ancho, double alto, out double medidaPalomera)
        {
            if(ancho > 0 && alto > 0)
            {
                medidaPalomera = ancho * alto;
                return true;
            }
            else
            {
                medidaPalomera = 0;
                return false;
            }
        }

        public double CalculateTotalMetrosLeña(DespachoLeñaModel model)
        {
            double alturaMedia = CalculateAlturaMedia(model.Alturas);
            double medidaPalomera = model.AnchoPalomera * model.AltoPalomera;

            return alturaMedia * model.Bancos * model.LargoCamion + medidaPalomera;
        }
    }
}
