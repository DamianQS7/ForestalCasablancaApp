using BosquesNalcahue.Mapping;
using BosquesNalcahue.Services;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.Input;
using ForestalCasablancaApp.Popups;
using ForestalCasablancaApp.Services;

namespace ForestalCasablancaApp.ViewModels
{
    public partial class LeñaViewModel : BaseViewModel
    {
        public LeñaViewModel(ICalculatorService calculatorService, IPdfGeneratorService pdfGeneratorService,
            IInfoService infoService) : base(calculatorService, pdfGeneratorService, infoService)
        {
            Title = "Despacho Leña";
            ReportType = "Leña";
            ListaEspecies = new List<string>
            {
                "Nativo",
                "Oregón",
                "Encino",
                "Mezcla de Oregón-Nativo"
            };
        }

        #region Commands


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
        /// Displays a Popup with a summary based on the Despacho model data.
        /// </summary>
        [RelayCommand]
        public virtual void DisplaySummaryAsync()
        {
            IsValidInput = ValidateInput();

            if (IsValidInput)
            {
                Despacho.TotalMetros = _calculatorService.CalculateTotalMetros(Despacho.Bancos,
                                        Despacho.LargoCamion, Despacho.AlturaMedia, Despacho.MedidaPalomera);

                Popup = new ConfirmationPopup();
                BasePage.ShowPopup(Popup);
            }
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
                var response = await _restService.PostAsync(ModelToDtoMapper.MapToSingleProductReport(this), "single-product-report");

                if(response.IsSuccessStatusCode)
                {
                    var (fileName, filePath) = await PdfGeneratorService.GeneratePdfFile(response);
                    await _infoService.ShowToast("El archivo PDF se ha generado con éxito");
                    await Launcher.Default.OpenAsync(new OpenFileRequest(fileName, new ReadOnlyFile(filePath)));
                }  
                else
                    await _infoService.ShowAlert("Error al enviar el reporte al servidor");
                
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

        #endregion

    }
}
