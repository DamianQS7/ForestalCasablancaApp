using BosquesNalcahue.Services;
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
        private readonly IInfoService _infoService;
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

        public MetroRumaViewModel(ICalculatorService calculatorService, IPdfGeneratorService pdfGeneratorService,
            IInfoService infoService)
        {
            Title = "Despacho Metro Ruma";
            _calculatorService = calculatorService;
            Despacho = new DespachoModel();
            Cliente = new();
            DatosCamion = new();
            _pdfGeneratorService = pdfGeneratorService;
            _infoService = infoService;
        }

        #region Methods

        /// <summary>
        /// Validates the input data for a Despacho (dispatch) and updates the model accordingly.
        /// </summary>
        /// <returns>
        /// Returns true if the input data is valid; otherwise, shows an alert and returns false.
        /// </returns>
        public bool ValidateInput()
        {
            ValidateAlturasAndUpdateModelAccordingly();
            ValidatePalomeraAndUpdateModelAccordingly();

            if (Despacho.AlturaMedia <= 0 || Despacho.Bancos is null || Despacho.LargoCamion is null)
            {
                _infoService.ShowAlert(InfoMessage.MissingMetroRumaData);
                return false;
            }

            if (!Despacho.IsPalomeraValid)
            {
                _infoService.ShowAlert(InfoMessage.InvalidPalomera);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Validates the alturas (heights) in the Despacho model and updates the model's AlturaMedia accordingly.
        /// </summary>
        public void ValidateAlturasAndUpdateModelAccordingly()
        {
            if (_calculatorService.CheckIfAlturasAreValid(Despacho.Alturas))
                Despacho.AlturaMedia = _calculatorService.CalculateAlturaMedia(Despacho.Alturas);
            else
                Despacho.AlturaMedia = 0;
        }

        /// <summary>
        /// Validates the palomera in the Despacho model and updates the model's properties accordingly.
        /// </summary>
        public void ValidatePalomeraAndUpdateModelAccordingly()
        {
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
        }

        #endregion

        #region Commands

        /// <summary>
        /// Displays a Popup with a summary based on the Despacho model data.
        /// </summary>
        [RelayCommand]
        public void DisplaySummaryAsync()
        {
            IsValidInput = ValidateInput();

            if (IsValidInput)
            {
                Despacho.TotalMetros = _calculatorService.CalculateTotalMetros(Despacho.Bancos,
                                        Despacho.LargoCamion, Despacho.AlturaMedia, Despacho.MedidaPalomera);

                _popup = new MetroRumaPopup();

                BasePage.ShowPopup(_popup);
            }
        }

        /// <summary>
        /// Clears the current page by resetting the Cliente, DatosCamion, and Despacho properties to new instances,
        /// and sets IsValidInput to false.
        /// </summary>
        [RelayCommand]
        public void ClearPage()
        {
            Cliente = new();
            DatosCamion = new();
            Despacho = new();
            IsValidInput = false;

            _infoService.ShowToast("Módulo reiniciado con éxito");
        }

        /// <summary>
        /// Generates a PDF document based on the current data and displays a toast notification upon success, or an alert
        /// upon failure.
        /// </summary>
        [RelayCommand]
        public async Task GeneratePDF()
        {
            if (IsBusy)
                return;

            try
            {
                IsBusy = true;

                Folio = GenerateFolio();

                _pdfGeneratorService.GenerateMetroRumaPDF(this);

                await _infoService.ShowToast("El archivo PDF se ha generado con éxito");
            }
            catch (Exception ex)
            {
                await _infoService.ShowAlert(ex.Message);
            }
            finally
            {
                IsBusy = false;
            }
        }

        /// <summary>
        /// Asynchronously closes the currently displayed popup.
        /// </summary>
        [RelayCommand]
        public async Task ClosePopup()
        {
            await _popup.CloseAsync();
        }

        #endregion
    }
}
