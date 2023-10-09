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

            
            return sum / count > 0 ? sum / count : 0;
        }

        public bool CheckPalomera(double ancho, double alto)
        {
            if((ancho > 0 && alto > 0) || (ancho == 0 && alto == 0))
                return true;
            else
                return false;
        }

        public void CalculateTotalMetrosLeña(DespachoLeñaModel model)
        {
            model.AlturaMedia = CalculateAlturaMedia(model.Alturas);
            double medidaPalomera = model.AnchoPalomera * model.AltoPalomera;

            model.TotalMetrosLeña = model.AlturaMedia * model.Bancos * model.LargoCamion + medidaPalomera;
        }

        public double CalculateTrozoAserrableVolume(double diametro, int cantidad, double largo)
        {
            if(largo <= 5.90)
            {
                return Math.Round((diametro * diametro * largo) / 10000, 2);
            }
            else
            {
                return Math.Round(Math.Pow(diametro + (largo - 4) / 2, 2) * ((largo + 0.10)/ 10000), 2);
            }
        }
    }
}
