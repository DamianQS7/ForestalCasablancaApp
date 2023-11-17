﻿using ForestalCasablancaApp.Models;
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
        double CalculateTotalMetros(DespachoModel model);
        double CalculateTrozoAserrableVolume(double? diametro, int? cantidad, double? largo);
        int CalculateTotalSum(ObservableCollection<MedidaTrozoAserrable> lista);
        public double CalculateFinalTotalSum(ObservableCollection<MedidaTrozoAserrable> lista);
    }
}
