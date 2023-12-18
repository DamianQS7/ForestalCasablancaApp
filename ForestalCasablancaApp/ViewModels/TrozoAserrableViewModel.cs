using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ForestalCasablancaApp.Models;
using ForestalCasablancaApp.Services;
using ForestalCasablancaApp.Popups;
using System.Collections.ObjectModel;
using CommunityToolkit.Maui.Views;
using ForestalCasablancaApp.Controls;
using ForestalCasablancaApp.Helpers;
using BosquesNalcahue.Services;
using BosquesNalcahue.Models;

namespace ForestalCasablancaApp.ViewModels
{
    public partial class TrozoAserrableViewModel : BaseViewModel
    {
        private readonly ICalculatorService _calculatorService;
        private readonly IPdfGeneratorService _pdfGeneratorService;
        private readonly IInfoService _infoService;
        private TrozoAserrableSummaryPopup _popup;

        #region Properties

        [ObservableProperty] private int _cantidadFinalDespacho;
        [ObservableProperty] private double _volumenFinalDespacho;
        [ObservableProperty] private MedidasEspecie _especie1;
        [ObservableProperty] private MedidasEspecie _especie2;
        [ObservableProperty] private MedidasEspecie _especie3;
        [ObservableProperty] private MedidasEspecie _especie4;
        [ObservableProperty] private MedidasEspecie _especie5;
        [ObservableProperty] private MedidasEspecie _especie6;
        public List<string> ListaEspecies { get; set; } = new() 
        { 
            "Oregón",
            "Eucalipto",
            "Roble Estaca",
            "Roble Desecho",
            "Hualle", 
            "Raulí", 
            "Coihue", 
            "Pellín",
            "Laurel",
            "Olivillo",
            "Tepa", 
            "Ulmo",
            "Lingue", 
            "Avellano",
            "Aromo",
            "Encino" 
        };
        public List<string> ListaLargos { get; set; } = new()
        {
            "1.20",
            "1.60",
            "2.10",
            "2.44",
            "2.74",
            "3.20",
            "3.66",
            "4.00",
            "5.00",
            "6.00",
            "7.00"
        };

        #endregion

        public TrozoAserrableViewModel(ICalculatorService calculatorService, IPdfGeneratorService pdfGeneratorService,
            IInfoService infoService)
        {
            Title = "Despacho Trozo Aserrable";
            _calculatorService = calculatorService;
            _pdfGeneratorService = pdfGeneratorService;
            _infoService = infoService;

            Cliente = new();
            DatosCamion = new();
            Especie1 = new();
            Especie2 = new();
            Especie3 = new();
            Especie4 = new();
            Especie5 = new();
            Especie6 = new();
            
        }

        #region Methods

        /// <summary>
        /// Validates the input fields for a specific list based on the provided "numeroLista" value.
        /// It is meant to be used within the AddItemToList command.
        /// </summary>
        /// <param name="numeroLista">Used as an identifier for the lists</param>
        /// <returns></returns>
        public bool ValidateInput(int numeroLista)
        {
            if(numeroLista == 1)
            {
                return CheckIfMedidaTrozoAserrableIsValid(Especie1.LargoEspecie,
                    Especie1.DiametroIngresado, Especie1.CantidadIngresada);                    
            }
            else if (numeroLista == 2)
            {
                return CheckIfMedidaTrozoAserrableIsValid(Especie2.LargoEspecie,
                    Especie2.DiametroIngresado, Especie2.CantidadIngresada);
            }
            else if (numeroLista == 3)
            {
                return CheckIfMedidaTrozoAserrableIsValid(Especie3.LargoEspecie,
                    Especie3.DiametroIngresado, Especie3.CantidadIngresada);
            }
            else if (numeroLista == 4)
            {
                return CheckIfMedidaTrozoAserrableIsValid(Especie4.LargoEspecie,
                    Especie4.DiametroIngresado, Especie4.CantidadIngresada);
            }
            else if (numeroLista == 5)
            {
                return CheckIfMedidaTrozoAserrableIsValid(Especie5.LargoEspecie,
                    Especie5.DiametroIngresado, Especie5.CantidadIngresada);
            }
            else if (numeroLista == 6)
            {
                return CheckIfMedidaTrozoAserrableIsValid(Especie6.LargoEspecie,
                    Especie6.DiametroIngresado, Especie6.CantidadIngresada);
            }

            return false;
            
        }

        public bool CheckIfMedidaTrozoAserrableIsValid(string largoEspecie, string diametroStr, string cantidadStr)
        {
            bool validDiametro = double.TryParse(diametroStr, out double diametro);
            bool validCantidad = int.TryParse(cantidadStr, out int cantidad);

            if(!validCantidad || !validDiametro)
            {
                _infoService.ShowAlert(InfoMessage.MissingTrozoData);
                return false;
            }

            if (!string.IsNullOrEmpty(largoEspecie) && diametro > 0 && cantidad > 0)
            {
                if (diametro % 2 == 0)
                {
                    return true;
                }
                else
                {
                    _infoService.ShowAlert(InfoMessage.InvalidDiameter);
                    return false;
                }
            }
            else
            {
                _infoService.ShowAlert(InfoMessage.MissingTrozoData);
                return false;
            }
        }

        /// <summary>
        /// Updates the total sum and final total sum for a specific list based on the content of each MedidasEspecie list.
        /// </summary>
        public void UpdateViewModelTotals()
        {
            GetListaMedidasTotals(Especie1);
            GetListaMedidasTotals(Especie2);
            GetListaMedidasTotals(Especie3);
            GetListaMedidasTotals(Especie4);
            GetListaMedidasTotals(Especie5);
            GetListaMedidasTotals(Especie6);

            CantidadFinalDespacho = _calculatorService.GetCantidadFinalDespachoTrozos(Especie1.CantidadTotalSum,
                Especie2.CantidadTotalSum, Especie3.CantidadTotalSum, Especie4.CantidadTotalSum, Especie5.CantidadTotalSum, 
                Especie6.CantidadTotalSum);

            VolumenFinalDespacho = _calculatorService.GetVolumenFinalDespachoTrozos(Especie1.TotalSumFinal, Especie2.TotalSumFinal,
                Especie3.TotalSumFinal, Especie4.TotalSumFinal, Especie5.TotalSumFinal, Especie6.TotalSumFinal);
        }

        /// <summary>
        /// Calculates and updates the total and final total sums for a given MedidasEspecie instance.
        /// </summary>
        /// <param name="especie">The MedidasEspecie instance for which totals are calculated and updated.</param>
        public void GetListaMedidasTotals(MedidasEspecie especie)
        {
            if (especie.ListaMedidas.Count > 0)
            {
                especie.CantidadTotalSum = _calculatorService.CalculateTotalSum(especie.ListaMedidas);
                especie.TotalSumFinal = _calculatorService.CalculateFinalTotalSum(especie.ListaMedidas);
            }
        }

        /// <summary>
        /// Adds a new MedidaTrozoAserrable instance to the specified ObservableCollection based on provided dimensions.
        /// </summary>
        /// <param name="listNumber">The number representing the list to which the MedidaTrozoAserrable instance is added.</param>
        /// <param name="list">The ObservableCollection to which the MedidaTrozoAserrable instance is added.</param>
        /// <param name="diametro">The diameter of the trozo as a nullable double.</param>
        /// <param name="cantidad">The quantity of the trozo as a nullable integer.</param>
        /// <param name="largo">The length of the trozo as a string.</param>
        public void AddMedidaTrozoAserrableToList(int listNumber, ObservableCollection<MedidaTrozoAserrable> list, 
            string diametro, string cantidad, string largo)
        {
            double volume = _calculatorService.CalculateTrozoAserrableVolume(diametro, largo);

            list.Add(new MedidaTrozoAserrable()
            {
                NumeroLista = listNumber,
                Diametro = double.Parse(diametro),
                Cantidad = int.Parse(cantidad),
                Volumen = volume,
                Total = Math.Round((double)(volume * int.Parse(cantidad)), 2)
            });
        }

        #endregion

        #region Commands

        /// <summary>
        /// Adds a new MedidaTrozoAserrable item to the appropriate list based on the specified NumericEntryCell identifier.
        /// </summary>
        /// <param name="cell">The NumericEntryCell containing information about the target list.</param>
        [RelayCommand]
        public void AddItemToList(NumericEntryCell cell)
        {
            if(cell.Identifier == "1")
            {
                if (ValidateInput(1))
                {
                    // Add the new item to the list number 1
                    AddMedidaTrozoAserrableToList(1, Especie1.ListaMedidas,
                        Especie1.DiametroIngresado, Especie1.CantidadIngresada, Especie1.LargoEspecie);

                    // Clear the input fields
                    cell.ClearValue(NumericEntryCell.UserInputProperty);
                    Especie1.CantidadIngresada = null;

                    // Set the focus on Diametro Entry.
                    cell.Focus(); 
                }
            } 
            else if(cell.Identifier == "2")
            {
                if (ValidateInput(2))
                {
                    AddMedidaTrozoAserrableToList(2, Especie2.ListaMedidas,
                        Especie2.DiametroIngresado, Especie2.CantidadIngresada, Especie2.LargoEspecie);

                    // Clear the input fields
                    Especie2.DiametroIngresado = null;
                    Especie2.CantidadIngresada = null;

                    // Set the focus on Diametro Entry.
                    cell.Focus();
                }
            }    
            else if(cell.Identifier == "3")
            {
                if(ValidateInput(3))
                {
                    AddMedidaTrozoAserrableToList(3, Especie3.ListaMedidas,
                        Especie3.DiametroIngresado, Especie3.CantidadIngresada, Especie3.LargoEspecie);

                    // Clear the input fields
                    Especie3.DiametroIngresado = null;
                    Especie3.CantidadIngresada = null;

                    // Set the focus on Diametro Entry.
                    cell.Focus();
                }
            }
            else if (cell.Identifier == "4")
            {
                if (ValidateInput(4))
                {
                    AddMedidaTrozoAserrableToList(4, Especie4.ListaMedidas,
                        Especie4.DiametroIngresado, Especie4.CantidadIngresada, Especie4.LargoEspecie);

                    // Clear the input fields
                    Especie4.DiametroIngresado = null;
                    Especie4.CantidadIngresada = null;

                    // Set the focus on Diametro Entry.
                    cell.Focus();
                }
            }
            else if (cell.Identifier == "5")
            {
                if (ValidateInput(5))
                {
                    AddMedidaTrozoAserrableToList(5, Especie5.ListaMedidas,
                        Especie5.DiametroIngresado, Especie5.CantidadIngresada, Especie5.LargoEspecie);

                    // Clear the input fields
                    Especie5.DiametroIngresado = null;
                    Especie5.CantidadIngresada = null;

                    // Set the focus on Diametro Entry.
                    cell.Focus();
                }
            }
            else if (cell.Identifier == "6")
            {
                if (ValidateInput(6))
                {
                    AddMedidaTrozoAserrableToList(6, Especie6.ListaMedidas,
                        Especie6.DiametroIngresado, Especie6.CantidadIngresada, Especie6.LargoEspecie);

                    // Clear the input fields
                    Especie6.DiametroIngresado = null;
                    Especie6.CantidadIngresada = null;

                    // Set the focus on Diametro Entry.
                    cell.Focus();
                }
            }
        }

        /// <summary>
        /// Removes a MedidaTrozoAserrable item from the appropriate list based on its NumeroLista.
        /// </summary>
        /// <param name="item">The MedidaTrozoAserrable item to be removed from the lists.</param>
        [RelayCommand]
        public void RemoveItemFromList(MedidaTrozoAserrable item)
        {
            // Check the associated NumeroLista and remove the item from the respective list.
            if (item.NumeroLista == 1)
                Especie1.ListaMedidas.Remove(item);
            else if (item.NumeroLista == 2)
                Especie2.ListaMedidas.Remove(item);
            else if (item.NumeroLista == 3)
                Especie3.ListaMedidas.Remove(item);
            else if (item.NumeroLista == 4)
                Especie4.ListaMedidas.Remove(item);
            else if (item.NumeroLista == 5)
                Especie5.ListaMedidas.Remove(item);
            else if (item.NumeroLista == 6)
                Especie6.ListaMedidas.Remove(item);
        }

        /// <summary>
        /// Clears the current page by instantiating new objects.
        /// </summary>
        [RelayCommand]
        public void ClearPage()
        {
            // Reset client and truck information.
            Cliente = new();
            DatosCamion = new();
            Especie1 = new();
            Especie2 = new();
            Especie3 = new();
            Especie4 = new();
            Especie5 = new();
            Especie6 = new();

            _infoService.ShowToast("Módulo reiniciado con éxito");
        }

        /// <summary>
        /// Displays a Popup with a summary based on the data from multiple MedidasEspecie lists.
        /// </summary>
        [RelayCommand]
        public async Task DisplaySummaryAsync()
        {
            if (Especie1.ListaMedidas.Count > 0 || Especie2.ListaMedidas.Count > 0 || Especie3.ListaMedidas.Count > 0
                || Especie4.ListaMedidas.Count > 0 || Especie5.ListaMedidas.Count > 0|| Especie6.ListaMedidas.Count > 0)
            {
                // We get the totals for each list, before calling the popup.
                UpdateViewModelTotals();

                // We get a new instance of the popup each time we call it.
                _popup = new TrozoAserrableSummaryPopup();

                // Call the popup and display it.
                BasePage.ShowPopup(_popup);
            }
            else
            {
                await _infoService.ShowAlert(InfoMessage.MissingMedidaTrozoAserrable);
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

        /// <summary>
        /// Generates a PDF document based on the current data and displays a toast notification upon success, or an alert
        /// upon failure.
        /// </summary>
        [RelayCommand]
        public async Task GeneratePDF()
        {
            if(IsBusy)
                return;

            try
            {
                IsBusy = true;

                Folio = GenerateFolio();

                _pdfGeneratorService.GenerateTrozoAserrablePDF(this);

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

        #endregion
    }
}
