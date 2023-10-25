using ForestalCasablancaApp.Models;
using ForestalCasablancaApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        public double CalculateTrozoAserrableVolume(double? diametro, int? cantidad, double? largo)
        {
            if(largo <= 5.90)
            {
                return ((double)diametro * (double)diametro * (double)largo) / 10000;
            }
            else
            {
                double result = Math.Pow((double)diametro + ((double)largo - 4) / 2, 2) * (((double)largo + 0.10) / 10000);
                return result;
            }
        }

        public int CalculateTotalSum(ObservableCollection<MedidaTrozoAserrable> lista)
        {
            int totalFinal = 0;

            foreach (int total in lista.Select(x => x.Cantidad))
            {
                totalFinal += total;
            }

            return totalFinal;
        }

        public double CalculateFinalTotalSum(ObservableCollection<MedidaTrozoAserrable> lista)
        {
            double totalFinal = 0;

            foreach (double total in lista.Select(x => x.Total))
            {
                totalFinal += total;
            }

            return Math.Round(totalFinal, 2);
        }
    }
}
