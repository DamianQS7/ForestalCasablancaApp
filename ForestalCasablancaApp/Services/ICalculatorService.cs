using ForestalCasablancaApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForestalCasablancaApp.Services
{
    public interface ICalculatorService
    {
        bool CheckIfAlturasAreValid(List<string> alturas);
        bool CheckPalomera(string ancho, string alto, string alto2);
        double CalculateAlturaMedia(List<string> alturas);
        double CalculateAlturaMediaPalomera(string alto, string alto2);
        double CalculateMedidaPalomera(double alturaMedia, string anchoPalomera);
        double CalculateTotalMetros(string bancosStr, string largoStr, double alturaMedia, double medidaPalomera);
        double CalculateTrozoAserrableVolume(string diametroStr, string largoStr);
        int CalculateTotalSum(ObservableCollection<MedidaTrozoAserrable> lista);
        double CalculateFinalTotalSum(ObservableCollection<MedidaTrozoAserrable> lista);
        int GetCantidadFinalDespachoTrozos(int cantidad1, int cantidad2, int cantidad3, int cantidad4, int cantidad5, 
            int cantidad6);
        double GetVolumenFinalDespachoTrozos(double cantidad1, double cantidad2, double cantidad3, double cantidad4,
            double cantidad5, double cantidad6);
    }
}
