using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ForestalCasablancaApp.Models;
using ForestalCasablancaApp.Services;
using ForestalCasablancaApp.Popups;
using System.Collections.ObjectModel;
using CommunityToolkit.Maui.Views;
using ForestalCasablancaApp.Controls;
using ForestalCasablancaApp.Helpers;
using System.Globalization;

namespace ForestalCasablancaApp.ViewModels
{
    public partial class TrozoAserrableViewModel : BaseViewModel
    {
        private readonly ICalculatorService _calculatorService;
        private readonly IPdfGeneratorService _pdfGeneratorService;

        #region Properties

        [ObservableProperty] private string _especieUno;
        [ObservableProperty] private string _especieDos;
        [ObservableProperty] private string _especieTres;
        [ObservableProperty] private string _largoEspecieUno;
        [ObservableProperty] private string _largoEspecieDos;
        [ObservableProperty] private string _largoEspecieTres;
        [ObservableProperty] private double? _diametroIngresado;
        [ObservableProperty] private int? _cantidadIngresada;
        [ObservableProperty] private double? _diametroIngresado2;
        [ObservableProperty] private int? _cantidadIngresada2;
        [ObservableProperty] private double? _diametroIngresado3;
        [ObservableProperty] private int? _cantidadIngresada3;
        private TrozoAserrableSummaryPopup _popup;

        public int TotalSumLista1 { get; set; }
        public double FinalTotalSumLista1 { get; set; }
        public int TotalSumLista2 { get; set; }
        public double FinalTotalSumLista2 { get; set; }
        public int TotalSumLista3 { get; set; }
        public double FinalTotalSumLista3 { get; set; }
        public ObservableCollection<MedidaTrozoAserrable> MedidasEspecieUno { get; set; } = new();
        public ObservableCollection<MedidaTrozoAserrable> MedidasEspecieDos { get; set; } = new();
        public ObservableCollection<MedidaTrozoAserrable> MedidasEspecieTres { get; set; } = new();
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

        public TrozoAserrableViewModel(ICalculatorService calculatorService, IPdfGeneratorService pdfGeneratorService)
        {
            Title = "Despacho Trozo Aserrable";
            _calculatorService = calculatorService;
            Cliente = new();
            DatosCamion = new();
            _pdfGeneratorService = pdfGeneratorService;
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
                if(!string.IsNullOrEmpty(LargoEspecieUno) && DiametroIngresado > 0 && CantidadIngresada > 0)
                {
                    if(DiametroIngresado % 2 == 0)
                    {
                           return true;
                    }
                    else
                    {
                        DisplayInputError(InfoMessage.InvalidDiameter);
                        return false;
                    }
                }
                else
                {
                    DisplayInputError(InfoMessage.MissingTrozoData);
                    return false;
                }
                    
            }
            else if (numeroLista == 2)
            {
                if (!string.IsNullOrEmpty(LargoEspecieDos) && DiametroIngresado2 > 0 && CantidadIngresada2 > 0)
                {
                    if (DiametroIngresado2 % 2 == 0)
                    {
                        return true;
                    }
                    else
                    {
                        DisplayInputError(InfoMessage.InvalidDiameter);
                        return false;
                    }
                }
                else
                {
                    DisplayInputError(InfoMessage.MissingTrozoData);
                    return false;
                }

            }
            else if (numeroLista == 3)
            {
                if (!string.IsNullOrEmpty(LargoEspecieTres) && DiametroIngresado3 > 0 && CantidadIngresada3 > 0)
                {
                    if (DiametroIngresado3 % 2 == 0)
                    {
                        return true;
                    }
                    else
                    {
                        DisplayInputError(InfoMessage.InvalidDiameter);
                        return false;
                    }
                }
                else
                {
                    DisplayInputError(InfoMessage.MissingTrozoData);
                    return false;
                }

            }

            return false;
            
        }

        /// <summary>
        /// Updates the total sum and final total sum for a specific list based on the content of each MedidasEspecie list.
        /// </summary>
        public void UpdateViewModelTotals()
        {
            if (MedidasEspecieUno.Count > 0)
            {
                TotalSumLista1 = _calculatorService.CalculateTotalSum(MedidasEspecieUno);
                FinalTotalSumLista1 = _calculatorService.CalculateFinalTotalSum(MedidasEspecieUno);
            }
            
            if (MedidasEspecieDos.Count > 0)
            {
                TotalSumLista2 = _calculatorService.CalculateTotalSum(MedidasEspecieDos);
                FinalTotalSumLista2 = _calculatorService.CalculateFinalTotalSum(MedidasEspecieDos);
            }
            
            if (MedidasEspecieTres.Count > 0)
            {
                TotalSumLista3 = _calculatorService.CalculateTotalSum(MedidasEspecieTres);
                FinalTotalSumLista3 = _calculatorService.CalculateFinalTotalSum(MedidasEspecieTres);
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
            double? diametro, int? cantidad, string largo)
        {
            double volume = _calculatorService.CalculateTrozoAserrableVolume(diametro, cantidad, largo);

            list.Add(new MedidaTrozoAserrable()
            {
                NumeroLista = listNumber,
                Diametro = diametro,
                Cantidad = cantidad,
                Volumen = volume,
                Total = Math.Round((double)(volume * cantidad), 2)
            });
        }

        #endregion

        #region Commands

        /// <summary>
        /// Adds a new item to the respective list based on the assigned property "Identifier" of the NumericEntryCell.
        /// </summary>
        /// <param name="cell"></param>
        [RelayCommand]
        public void AddItemToList(NumericEntryCell cell)
        {
            if(cell.Identifier == "1")
            {
                if (ValidateInput(1))
                {
                    // Add the new item to the list number 1
                    AddMedidaTrozoAserrableToList(1, MedidasEspecieUno, DiametroIngresado, CantidadIngresada, LargoEspecieUno);
                    
                    // Clear the input fields
                    DiametroIngresado = null;
                    CantidadIngresada = null;

                    // Set the focus on Diametro Entry.
                    cell.Focus();
                }
            } 
            else if(cell.Identifier == "2")
            {
                if (ValidateInput(2))
                {
                    AddMedidaTrozoAserrableToList(2, MedidasEspecieDos, DiametroIngresado2, CantidadIngresada2, LargoEspecieDos);

                    // Clear the input fields
                    DiametroIngresado2 = null;
                    CantidadIngresada2 = null;

                    // Set the focus on Diametro Entry.
                    cell.Focus();
                }
            }    
            else if(cell.Identifier == "3")
            {
                if(ValidateInput(3))
                {
                    AddMedidaTrozoAserrableToList(3, MedidasEspecieTres, DiametroIngresado3, CantidadIngresada3, LargoEspecieTres);

                    // Clear the input fields
                    DiametroIngresado3 = null;
                    CantidadIngresada3 = null;

                    // Set the focus on Diametro Entry.
                    cell.Focus();
                }
            }
        }

        /// <summary>
        /// Removes a specified item from the respective list based on its associated NumeroLista.
        /// </summary>
        /// <param name="item">The MedidaTrozoAserrable item to be removed.</param>
        [RelayCommand]
        public void RemoveItemFromList(MedidaTrozoAserrable item)
        {
            // Check the associated NumeroLista and remove the item from the respective list.
            if (item.NumeroLista == 1)
                MedidasEspecieUno.Remove(item);
            else if (item.NumeroLista == 2)
                MedidasEspecieDos.Remove(item);
            else if (item.NumeroLista == 3)
                MedidasEspecieTres.Remove(item);
        }

        /// <summary>
        /// Clears all data on the current page, including measurements and client information.
        /// </summary>
        [RelayCommand]
        public void ClearPage()
        {
            // Clear the lists of measurements for different species.
            MedidasEspecieUno.Clear();
            MedidasEspecieDos.Clear();
            MedidasEspecieTres.Clear();

            // Reset client and truck information.
            Cliente = new();
            DatosCamion = new();

            // Reset species names and lengths.
            EspecieUno = "";
            EspecieDos = "";
            EspecieTres = "";
            LargoEspecieUno = "";
            LargoEspecieDos = "";
            LargoEspecieTres = "";

            // Reset other values.
            DiametroIngresado = null;
            DiametroIngresado2 = null;
            DiametroIngresado3 = null;
            CantidadIngresada = null;
            CantidadIngresada2 = null;
            CantidadIngresada3 = null;
            TotalSumLista1 = 0;
            FinalTotalSumLista1 = 0;
            TotalSumLista2 = 0;
            FinalTotalSumLista2 = 0;
            TotalSumLista3 = 0;
            FinalTotalSumLista3 = 0;
        }

        [RelayCommand]
        public async Task DisplaySummaryAsync()
        {
            if (MedidasEspecieUno.Count > 0 || MedidasEspecieDos.Count > 0 || MedidasEspecieTres.Count > 0)
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
                await Shell.Current.DisplayAlert("Error", "Debe ingresar al menos una medida", "OK");
            }
        }

        [RelayCommand]
        public async Task ClosePopup()
        {
             await _popup.CloseAsync();
        }

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

        #endregion
    }
}
