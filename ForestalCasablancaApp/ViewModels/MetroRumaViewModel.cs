using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ForestalCasablancaApp.Models;
using ForestalCasablancaApp.Popups;
using ForestalCasablancaApp.Services;

namespace ForestalCasablancaApp.ViewModels
{
    public partial class MetroRumaViewModel : BaseViewModel
    {
        private readonly ICalculatorService _calculatorService;
        private readonly IPdfGeneratorService _pdfGeneratorService;
        private MetroRumaPopup _popup;

        [ObservableProperty] private Cliente _cliente;
        [ObservableProperty] private DatosCamion _datosCamion;
        [ObservableProperty] private DespachoLeñaModel _despacho;

        public MetroRumaViewModel(ICalculatorService calculatorService, IPdfGeneratorService pdfGeneratorService)
        {
            Title = "Despacho Metro Ruma";
            _calculatorService = calculatorService;
            Despacho = new DespachoLeñaModel();
            Cliente = new();
            DatosCamion = new();
            _pdfGeneratorService = pdfGeneratorService;
        }

        private bool ValidateInput()
        {
            return false;
        }

        #region Commands

        [RelayCommand]
        private async void DisplaySummaryAsync()
        {
            if (ValidateInput())
            {
                _popup = new MetroRumaPopup();

                BasePage.ShowPopup(_popup);
            }
            else
            {
                await Shell.Current.DisplayAlert("Error", "...", "OK");
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
            //await _popup.CloseAsync();
        }
        #endregion
    }
}
