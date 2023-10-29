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
        public double CalculateAlturaMedia(List<double?> alturas)
        {
            int count = 0;
            double? sum = 0;
            double alturaMedia = 0;

            foreach (double? altura in alturas)
            {
                if (altura > 0)
                {
                    count++;
                    sum += altura;
                }
            }

            alturaMedia = (double)sum / count;
            
            return alturaMedia > 0 ? alturaMedia : 0;
        }

        public bool CheckPalomera(double? ancho, double? alto)
        {
            // Si ambos son mayores a 0 o ambos son 0 o ambos son null, es valido
            if((ancho > 0 && alto > 0) || (ancho == 0 && alto == 0) || (ancho is null && alto is null ))
                return true;
            else
                return false;
        }

        public void CalculateTotalMetrosLeña(DespachoLeñaModel model)
        {
            model.AlturaMedia = CalculateAlturaMedia(model.Alturas);
            double medidaPalomera = CalculatePalomera(model.AnchoPalomera, model.AltoPalomera);

            model.TotalMetrosLeña = model.AlturaMedia * model.Bancos * model.LargoCamion + medidaPalomera;
        }

        public double CalculatePalomera(double? largo, double? ancho)
        {
            if(largo is null || ancho is null)
                return 0;
            
            if(largo == 0 || ancho == 0)
                return 0;

            return (double)largo * (double)ancho;
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
