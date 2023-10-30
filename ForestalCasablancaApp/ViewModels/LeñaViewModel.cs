using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ForestalCasablancaApp.Models;
using ForestalCasablancaApp.Popups;
using ForestalCasablancaApp.Services;

namespace ForestalCasablancaApp.ViewModels
{
    public partial class LeñaViewModel : BaseViewModel
    {
        private readonly ICalculatorService _calculatorService;

        [ObservableProperty] private DespachoLeñaModel _despacho;
        [ObservableProperty] private Cliente _cliente;
        [ObservableProperty] private DatosCamion _datosCamion;

        #region Methods
        public LeñaViewModel(ICalculatorService calculatorService)
        {
            Title = "Despacho Leña";
            _calculatorService = calculatorService;
            Despacho = new DespachoLeñaModel();
            Cliente = new();
            DatosCamion = new();
            IsValidInput = false;
        }

        private void ValidateInput()
        {
            _calculatorService.CalculateTotalMetrosLeña(Despacho);
            bool validPalomera = _calculatorService.CheckPalomera(Despacho.AnchoPalomera, Despacho.AltoPalomera);
            
            if(Despacho.AlturaMedia <= 0  || validPalomera == false)
                IsValidInput = false;
            else
                IsValidInput = true;
        }

        #endregion

        #region Commands

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
                await Shell.Current.DisplayAlert("Error", "Debe incluir 'Largo Camión', 'N° de Bancos' y por lo menos una altura", "OK");
            }
        }

        [RelayCommand]
        void ClearPage()
        {
            Cliente = new();
            DatosCamion = new();
            Despacho = new();
            IsValidInput = false;
        }

        #endregion

    }
}
