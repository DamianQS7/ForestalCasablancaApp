using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ForestalCasablancaApp.Helpers;
using ForestalCasablancaApp.Models;
using ForestalCasablancaApp.Popups;
using ForestalCasablancaApp.Services;
using System.Globalization;

namespace ForestalCasablancaApp.ViewModels
{
    public partial class MetroRumaViewModel : BaseViewModel
    {
        private readonly ICalculatorService _calculatorService;
        private readonly IPdfGeneratorService _pdfGeneratorService;
        private MetroRumaPopup _popup;

        [ObservableProperty] private Cliente _cliente;
        [ObservableProperty] private DatosCamion _datosCamion;
        [ObservableProperty] private DespachoModel _despacho;

        public List<string> ListaEspecies { get; set; } = new()
        {
            "Nativo",
            "Oregón",
            "Otro"
        };

        public MetroRumaViewModel(ICalculatorService calculatorService, IPdfGeneratorService pdfGeneratorService)
        {
            Title = "Despacho Metro Ruma";
            _calculatorService = calculatorService;
            Despacho = new DespachoModel();
            Cliente = new();
            DatosCamion = new();
            _pdfGeneratorService = pdfGeneratorService;
        }

        private bool ValidateInput()
        {
            // Get the average height of the wood
            if (_calculatorService.CheckIfAlturasAreValid(Despacho.Alturas))
                Despacho.AlturaMedia = _calculatorService.CalculateAlturaMedia(Despacho.Alturas);
            else
                Despacho.AlturaMedia = 0;

            // Check if palomera is valid
            if (_calculatorService.CheckPalomera(Despacho.AnchoPalomera, Despacho.AltoPalomera, Despacho.AltoPalomera2))
            {
                Despacho.AlturaMediaPalomera = _calculatorService.CalculateAlturaMediaPalomera(Despacho.AltoPalomera, Despacho.AltoPalomera2);
                Despacho.MedidaPalomera = _calculatorService.CalculateMedidaPalomera(Despacho.AlturaMediaPalomera, Despacho.AnchoPalomera);
                Despacho.IsPalomeraValid = true;
            }
            else
            {
                Despacho.MedidaPalomera = 0;
                Despacho.IsPalomeraValid = false;
            }

            if (Despacho.AlturaMedia <= 0 || Despacho.Bancos is null || Despacho.LargoCamion is null)
            {
                DisplayInputError(InfoMessage.MissingMetroRumaData);
                return false;
            }

            if (!Despacho.IsPalomeraValid)
            {
                DisplayInputError(InfoMessage.InvalidPalomera);
                return false;
            }

            return true;
        }

        #region Commands

        [RelayCommand]
        private void DisplaySummaryAsync()
        {
            if (ValidateInput())
            {
                Despacho.TotalMetros = _calculatorService.CalculateTotalMetros(Despacho);

                _popup = new MetroRumaPopup();

                BasePage.ShowPopup(_popup);
            }
        }

        [RelayCommand]
        void ClearPage()
        {
            Cliente = new();
            DatosCamion = new();
            Despacho = new();
        }

        [RelayCommand]
        private async Task GeneratePDF()
        {
            if (IsBusy)
                return;

            try
            {
                IsBusy = true;

                Folio = GenerateFolio();

                _pdfGeneratorService.GenerateMetroRumaPDF(this);

                await Toast.Make("El archivo PDF se ha generado con éxito").Show();
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "Ok");
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        private async Task ClosePopup()
        {
            await _popup.CloseAsync();
        }

        #endregion
    }
}
