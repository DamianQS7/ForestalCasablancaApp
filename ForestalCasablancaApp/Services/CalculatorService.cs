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
        /// <summary>
        /// Checks if the provided list of string values representing alturas (heights) are valid.
        /// </summary>
        /// <param name="alturas">A list of string values representing alturas to be validated.</param>
        /// <returns>
        /// Returns true if at least one valid altura (height) greater than 0 is found in the list; otherwise, returns false.
        /// </returns>
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

        /// <summary>
        /// Calculates the average altura (height) from a list of string values representing alturas.
        /// No input validation is made, because this method is called after the CheckIfAlturasAreValid method.
        /// </summary>
        /// <param name="alturas">A list of string values representing alturas for which the average is calculated.</param>
        /// <returns>
        /// Returns the calculated average altura if at least one valid altura greater than 0 is found in the list; otherwise, returns 0.
        /// </returns>
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

        /// <summary>
        /// Checks the validity of dimensions for a palomera based on provided width, height, and secondary height.
        /// </summary>
        /// <param name="ancho">The width of the palomera as a string.</param>
        /// <param name="alto">The primary height of the palomera as a string.</param>
        /// <param name="alto2">The secondary height of the palomera as a string.</param>
        /// <returns>
        /// Returns true if the provided dimensions are either all zero or empty, or satisfy the conditions for a valid palomera;
        /// otherwise, returns false.
        /// </returns>
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

        /// <summary>
        /// Calculates the total number of metres based on the provided parameters: bancos, largoCamion, alturaMedia, and medidaPalomera.
        /// No input validation is made, because this method is called after the CheckIfAlturasAreValid and CheckPalomera methods.
        /// </summary>
        /// <param name="bancosStr">The number of bancos (benches) as a string.</param>
        /// <param name="largoStr">The length of the camion (truck) as a string.</param>
        /// <param name="alturaMedia">The average height of alturas (heights) as a double.</param>
        /// <param name="medidaPalomera">The measurement of the palomera as a double.</param>
        /// <returns>
        /// Returns the calculated total number of metros based on the provided parameters.
        /// </returns>
        public double CalculateTotalMetros(string bancosStr, string largoStr, double alturaMedia, double medidaPalomera)
        {
            double.TryParse(bancosStr, CultureInfo.InvariantCulture, out double bancos);
            double.TryParse(largoStr, CultureInfo.InvariantCulture, out double largoCamion);

            return alturaMedia * bancos * largoCamion + medidaPalomera;
        }

        /// <summary>
        /// Calculates the average altura (height) for a palomera based on provided primary and secondary heights.
        /// </summary>
        /// <param name="alto">The primary height of the palomera as a string.</param>
        /// <param name="alto2">The secondary height of the palomera as a string.</param>
        /// <returns>
        /// Returns the calculated average altura for the palomera based on provided heights;
        /// otherwise, returns 0 if both heights are 0 or no valid heights are provided.
        /// </returns>
        public double CalculateAlturaMediaPalomera(string alto, string alto2)
        {
            double.TryParse(alto, CultureInfo.InvariantCulture, out double altoPalomera);
            double.TryParse(alto2, CultureInfo.InvariantCulture, out double altoPalomera2);
            int count = 0;

            if (altoPalomera > 0)
                count++;
            
            if(altoPalomera2 > 0)
                count++;

            return count > 0 ? (altoPalomera + altoPalomera2) / count : 0;
                        
        }

        /// <summary>
        /// Calculates the medida (measurement) of a palomera based on the average altura (height) and provided width.
        /// </summary>
        /// <param name="alturaMedia">The average height of alturas (heights) as a double.</param>
        /// <param name="anchoPalomera">The width of the palomera as a string.</param>
        /// <returns>
        /// Returns the calculated medida (measurement) of the palomera based on the average altura and provided width;
        /// otherwise, returns 0 if the width is not greater than 0.
        /// </returns>
        public double CalculateMedidaPalomera(double alturaMedia, string anchoPalomera)
        {
            double.TryParse(anchoPalomera, CultureInfo.InvariantCulture, out double ancho);

            if(ancho > 0)
                return alturaMedia * ancho;
            else
                return 0;
        }

        /// <summary>
        /// Calculates the volume of a trozo aserrable based on the provided dimensions: diametro, cantidad, and largo.
        /// It will apply a different formula depending on the length of the trozo.
        /// </summary>
        /// <param name="diametro">The diameter of the trozo as a nullable double.</param>
        /// <param name="cantidad">The quantity of the trozo as a nullable integer.</param>
        /// <param name="largoStr">The length of the trozo as a string.</param>
        /// <returns>
        /// Returns the calculated volume of the trozo aserrable based on the provided dimensions.
        /// </returns>
        public double CalculateTrozoAserrableVolume(string diametroStr, string largoStr)
        {
            double largo = double.Parse(largoStr, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture);
            double diametro = double.Parse(diametroStr, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture);

            if (largo <= 5.90)
            {
                return (diametro * diametro * largo) / 10000;
            }
            else
            {
                double result = Math.Pow(diametro + (largo - 4) / 2, 2) * ((largo + 0.10) / 10000);
                return result;
            }
        }

        /// <summary>
        /// Calculates the total sum of quantities from a collection of MedidaTrozoAserrable instances.
        /// </summary>
        /// <param name="lista">An ObservableCollection of MedidaTrozoAserrable instances.</param>
        /// <returns>
        /// Returns the total sum of quantities from the provided collection of MedidaTrozoAserrable instances.
        /// </returns>
        public int CalculateTotalSum(ObservableCollection<MedidaTrozoAserrable> lista)
        {
            int totalFinal = 0;

            foreach (int? total in lista.Select(x => x.Cantidad))
            {
                totalFinal += (int)total;
            }

            return totalFinal;
        }

        /// <summary>
        /// Calculates the final total sum of total values from a collection of MedidaTrozoAserrable instances.
        /// </summary>
        /// <param name="lista">An ObservableCollection of MedidaTrozoAserrable instances.</param>
        /// <returns>
        /// Returns the final total sum of total values from the provided collection of MedidaTrozoAserrable instances,
        /// rounded to two decimal places.
        /// </returns>
        public double CalculateFinalTotalSum(ObservableCollection<MedidaTrozoAserrable> lista)
        {
            double totalFinal = 0;

            foreach (double? total in lista.Select(x => x.Total))
            {
                totalFinal += (double)total;
            }

            return Math.Round(totalFinal, 2);
        }
    }
}
