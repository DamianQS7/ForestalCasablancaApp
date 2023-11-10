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

        //public bool CheckPalomera(double? ancho, double? alto)
        //{
        //    // Si ambos son mayores a 0 o ambos son 0 o ambos son null, es valido
        //    if((ancho > 0 && alto > 0) || (ancho == 0 && alto == 0) || (ancho is null && alto is null ))
        //        return true;
        //    else
        //        return false;
        //}

        public bool CheckPalomera(double? ancho, double? alto, double? alto2)
        {
            // Si ambos son mayores a 0 o ambos son 0 o ambos son null, es valido
            if((ancho > 0 && (alto > 0 || alto2 > 0)) || (ancho == 0 && alto == 0 && alto2 == 0) || (ancho is null && alto is null && alto2 is null ))
                return true;
            else
                return false;
        }

        public void CalculateTotalMetros(DespachoModel model)
        {
            model.AlturaMedia = CalculateAlturaMedia(model.Alturas);
            double medidaPalomera = CalculatePalomera(model.AnchoPalomera, model.AltoPalomera, model.AltoPalomera2);

            model.TotalMetros = model.AlturaMedia * model.Bancos * model.LargoCamion + medidaPalomera;
        }

        public double CalculatePalomera(double? ancho, double? alto, double? alto2)
        {
            double altura1;
            double altura2;
            double alturaMedia;
            int count = 0;

            // Checking for AnchoPalomera first
            if (ancho is null || ancho == 0)
                return 0;

            // Checking for AlturaPalomera
            if (alto is null && alto2 is null)
                return 0;

            // Checking for AlturaPalomera2
            if (alto == 0 && alto2 == 0)
                return 0;

            // Getting the value for AltoPalomera
            if (alto is null || alto == 0)
                altura1 = 0;
            else
            {
                altura1 = (double)alto;
                count++;
            }

            // Getting the value for AltoPalomera
            if (alto2 is null || alto2 == 0)
                altura2 = 0;
            else
            {
                altura2 = (double)alto2;
                count++;
            }

            // Getting the average of the heights
            alturaMedia = (altura1 + altura2) / count;

            return alturaMedia * (double)ancho;
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
