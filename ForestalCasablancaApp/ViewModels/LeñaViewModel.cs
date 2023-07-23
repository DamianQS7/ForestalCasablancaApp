﻿using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ForestalCasablancaApp.Controls;
using ForestalCasablancaApp.Models;
using ForestalCasablancaApp.Pages;
using ForestalCasablancaApp.Services;
using System.Threading.Tasks;

namespace ForestalCasablancaApp.ViewModels
{
    public partial class LeñaViewModel : BaseViewModel
    {
        private readonly ICalculatorService _calculatorService;

        [ObservableProperty]
        private DespachoLeñaModel _despacho;

        [ObservableProperty]
        private Cliente _cliente;

        public LeñaViewModel(ICalculatorService calculatorService)
        {
            Title = "Despacho Leña";
            _calculatorService = calculatorService;
            Despacho = new DespachoLeñaModel();
            Cliente = new();
            IsValidInput = false;
        }

        private void ValidateInput()
        {
            _calculatorService.CalculateTotalMetrosLeña(Despacho);
            bool validPalomera = _calculatorService.CheckPalomera(Despacho.AnchoPalomera, Despacho.AltoPalomera);
            
            if(Cliente.Nombre is null || Cliente.RUT is null || Cliente.Patente is null
                || Despacho.AlturaMedia <= 0  || validPalomera == false)
                IsValidInput = false;
            else
                IsValidInput = true;
        }

        [RelayCommand]
        private async void DisplaySummaryAsync()
        {
            ValidateInput();

            if (IsValidInput)
            {
                var popup = new ConfirmationPopup();

                BasePage.ShowPopup(popup);
            }
            else
            {
                await Shell.Current.DisplayAlert("Error", "Debe completar todos los campos", "OK");
            }
        }

    }
}
