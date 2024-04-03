using BosquesNalcahue.Mapping;
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
    public partial class LeñaViewModel : BaseViewModel
    {
        private readonly ICalculatorService _calculatorService;
        private readonly IPdfGeneratorService _pdfGeneratorService;
        private readonly IInfoService _infoService;
        private readonly IRestService _restService;
        private ConfirmationPopup _popup;

        [ObservableProperty] private DespachoModel _despacho;
        [ObservableProperty] private Cliente _cliente;
        [ObservableProperty] private DatosCamion _datosCamion;

        public List<string> ListaEspecies { get; set; } = new()
        {
            "Nativo",
            "Oregón",
            "Encino",
            "Mezcla de Oregón-Nativo"
        };


        public LeñaViewModel(ICalculatorService calculatorService, IPdfGeneratorService pdfGeneratorService,
            IInfoService infoService, IRestService restService)
        {
            Title = "Despacho Leña";
            _calculatorService = calculatorService;
            Despacho = new();
            Cliente = new();
            DatosCamion = new();
            IsValidInput = false;
            _pdfGeneratorService = pdfGeneratorService;
            _infoService = infoService;
            _restService = restService;
        }

        #region Methods

        /// <summary>
        /// Validates the input data for a Despacho and updates the model accordingly.
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
                _infoService.ShowAlert(InfoMessage.MissingLeñaData);
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

                _popup = new ConfirmationPopup();
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

                // Generate a unique folio for the report using the current date and time.
                Folio = GenerateFolio();

                // Set the current date and time as the report date.
                ReportDate = DateTime.Now;

                // Post the report to the server after mapping the ViewModel to a DTO.
                await _restService.PostAsync(ModelToDtoMapper.MapToSingleProductReport(this));

                // Generate the PDF file only if the report was successfully posted.
                _pdfGeneratorService.GenerateLeñaPDF(this);

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
