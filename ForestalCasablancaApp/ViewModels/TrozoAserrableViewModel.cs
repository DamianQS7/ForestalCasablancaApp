using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Maui.Core;
using ForestalCasablancaApp.Models;
using ForestalCasablancaApp.Services;
using ForestalCasablancaApp.Controls;
using System.Collections.ObjectModel;
using CommunityToolkit.Maui.Views;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

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
        [ObservableProperty] private double _diametroIngresado;
        [ObservableProperty] private int _cantidadIngresada;
        [ObservableProperty] private double _diametroIngresado2;
        [ObservableProperty] private int _cantidadIngresada2;
        [ObservableProperty] private double _diametroIngresado3;
        [ObservableProperty] private int _cantidadIngresada3;
        private TrozoAserrableSummaryPopup _popup;

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
            "2,44",
            "2,74",
            "3.20",
            "3.66",
            "4.00",
            "5.00",
            "6.00",
            "7.00"
        };

        #endregion

        #region Methods

        public TrozoAserrableViewModel(ICalculatorService calculatorService, IPdfGeneratorService pdfGeneratorService)
        {
            Title = "Despacho Trozo Aserrable";
            _calculatorService = calculatorService;
            Cliente = new();
            DatosCamion = new();
            _pdfGeneratorService = pdfGeneratorService;
        }

        private bool ValidateInput(int numeroLista)
        {
            if(numeroLista == 1)
            {
                if(!string.IsNullOrEmpty(LargoEspecieUno) && DiametroIngresado > 0 && CantidadIngresada > 0)
                    return true;
                else
                    return false;
            }
            else if(numeroLista == 2)
            {
                if (!string.IsNullOrEmpty(LargoEspecieDos) && DiametroIngresado2 > 0 && CantidadIngresada2 > 0)
                    return true;
                else
                    return false;
            }
            else if(numeroLista == 3)
            {
                if (!string.IsNullOrEmpty(LargoEspecieTres) && DiametroIngresado3 > 0 && CantidadIngresada3 > 0)
                    return true;
                else
                    return false;
            }
            else
                return false;
        }

        #endregion

        #region Commands

        /// <summary>
        /// Adds a new item to a specific list based on the provided "numeroLista" value.
        /// </summary>
        /// <param name="numeroLista">The identifier for the target list (1, 2, or 3).</param>
        [RelayCommand]
        void AddItemToList(string numeroLista)
        {
            if(numeroLista == "1")
            {
                if (ValidateInput(1))
                {
                    // Add the new item to the list number 1
                    MedidasEspecieUno.Add(new MedidaTrozoAserrable()
                    {
                        NumeroLista = 1,
                        Diametro = DiametroIngresado,
                        Cantidad = CantidadIngresada,
                        Volumen = _calculatorService.CalculateTrozoAserrableVolume(DiametroIngresado, CantidadIngresada, double.Parse(LargoEspecieUno))
                    });

                    // Clear the input fields
                    DiametroIngresado = 0;
                    CantidadIngresada = 0;
                }
            } 
            else if(numeroLista == "2")
            {
                if (ValidateInput(2))
                {
                    // Add the new item to the list number 2
                    MedidasEspecieDos.Add(new MedidaTrozoAserrable()
                    {
                        NumeroLista = 2,
                        Diametro = DiametroIngresado2,
                        Cantidad = CantidadIngresada2,
                        Volumen = _calculatorService.CalculateTrozoAserrableVolume(DiametroIngresado2, CantidadIngresada2, double.Parse(LargoEspecieDos))
                    });

                    // Clear the input fields
                    DiametroIngresado2 = 0;
                    CantidadIngresada2 = 0;
                }
            }    
            else if(numeroLista == "3")
            {
                if(ValidateInput(3))
                {
                    // Add the new item to the list number 3
                    MedidasEspecieTres.Add(new MedidaTrozoAserrable()
                    {
                        NumeroLista = 3,
                        Diametro = DiametroIngresado3,
                        Cantidad = CantidadIngresada3,
                        Volumen = _calculatorService.CalculateTrozoAserrableVolume(DiametroIngresado3, CantidadIngresada3, double.Parse(LargoEspecieTres))
                    });

                    // Clear the input fields
                    DiametroIngresado3 = 0;
                    CantidadIngresada3 = 0;
                }
            }
        }


        /// <summary>
        /// Removes a specified item from the respective list based on its associated NumeroLista.
        /// </summary>
        /// <param name="item">The MedidaTrozoAserrable item to be removed.</param>
        [RelayCommand]
        void RemoveItemFromList(MedidaTrozoAserrable item)
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
        void ClearPage()
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
        }

        [RelayCommand]
        private async void DisplaySummaryAsync()
        {
            if(MedidasEspecieUno.Count > 0 || MedidasEspecieDos.Count > 0 || MedidasEspecieTres.Count > 0)
            {
                _popup = new TrozoAserrableSummaryPopup();

                BasePage.ShowPopup(_popup);
            }
            else
            {
                await Shell.Current.DisplayAlert("Error", "Debe ingresar al menos una medida", "OK");
            }
    
        }

        [RelayCommand]
        private async Task ClosePopup()
        {
             await _popup.CloseAsync();
        }

        [RelayCommand]
        private void GeneratePDF()
        {
            _pdfGeneratorService.GenerateTrozoAserrablePDF(this);
        }
        #endregion
    }
}
