using ForestalCasablancaApp.Models;
using ForestalCasablancaApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForestalCasablancaApp.Services
{
    public class CalculatorService : ICalculatorService
    {
        public bool CheckIfAlturasAreValid(List<string> alturas)
        {
            foreach (string altura in alturas)
            {
                if (double.TryParse(altura, CultureInfo.InvariantCulture, out double valueHolder))
                {
                    if (valueHolder > 0)
                        return true;
                }
            }

            return false;
        }
        public double CalculateAlturaMedia(List<string> alturas)
        {
            int count = 0;
            double sum = 0;
            double alturaMedia;

            foreach (string altura in alturas)
            {
                if (double.TryParse(altura, CultureInfo.InvariantCulture, out double valueHolder))
                {
                    if(valueHolder > 0)
                    {
                        count++;
                        sum += valueHolder;
                    }
                }
            }

            alturaMedia = sum / count;
            
            return alturaMedia > 0 ? alturaMedia : 0;
        }

        public bool CheckPalomera(string ancho, string alto, string alto2)
        {
            // Check if the user has entered the values required to perform the calculation
            if (string.IsNullOrEmpty(ancho) && string.IsNullOrEmpty(alto) && string.IsNullOrEmpty(alto2))
                return true;

            double.TryParse(ancho, CultureInfo.InvariantCulture, out double anchoPalomera);
            double.TryParse(alto, CultureInfo.InvariantCulture, out double altoPalomera);
            double.TryParse(alto2, CultureInfo.InvariantCulture, out double altoPalomera2);

            if (anchoPalomera == 0 && altoPalomera == 0 && altoPalomera2 == 0)
                return true;

            // Check if the values entered are valid
            if (anchoPalomera > 0 && (altoPalomera > 0 || altoPalomera2 > 0))
                return true;
            
            return false;
        }

        public double CalculateTotalMetros(DespachoModel model)
        {
            double.TryParse(model.Bancos, CultureInfo.InvariantCulture, out double bancos);
            double.TryParse(model.LargoCamion, CultureInfo.InvariantCulture, out double largoCamion);

            return model.AlturaMedia * bancos * largoCamion + model.MedidaPalomera;
        }

        /// <summary>
        /// This method calculates the palomera's volume. It assumes that the user input is valid, because it will be called after the CheckPalomera method.
        /// </summary>
        /// <param name="ancho"> String representing the value for AnchoPalomera input by the user. </param>
        /// <param name="alto"> String representing the value for AltoPalomera input by the user. </param>
        /// <param name="alto2"> String representing the value for AltoPalomera2 input by the user. </param>
        /// <returns> A double value representing the palomera's volume. </returns>
        public double CalculatePalomera(string ancho, string alto, string alto2)
        {

            double.TryParse(ancho, CultureInfo.InvariantCulture, out double anchoPalomera);
            double.TryParse(alto, CultureInfo.InvariantCulture, out double altoPalomera);
            double.TryParse(alto2, CultureInfo.InvariantCulture, out double altoPalomera2);
            int count = 0;

            if (altoPalomera > 0)
                count++;
            
            if(altoPalomera2 > 0)
                count++;


            return count > 0 ? (altoPalomera + altoPalomera2) / count * anchoPalomera : 0;
                        
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
