using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Maui.Core;
using ForestalCasablancaApp.Models;
using ForestalCasablancaApp.Services;
using System.Collections.ObjectModel;

namespace ForestalCasablancaApp.ViewModels
{
    public partial class TrozoAserrableViewModel : BaseViewModel
    {
        private readonly ICalculatorService _calculatorService;

        [ObservableProperty] private Cliente _cliente;
        [ObservableProperty] private string _especieUno;
        [ObservableProperty] private string _especieDos;
        [ObservableProperty] private string _especieTres;
        [ObservableProperty] private double _largoEspecieUno;
        [ObservableProperty] private double _largoEspecieDos;
        [ObservableProperty] private double _largoEspecieTres;

        public ObservableCollection<MedidaTrozoAserrable> MedidasEspecieUno { get; set; } = new();
        public ObservableCollection<MedidaTrozoAserrable> MedidasEspecieDos { get; set; } = new();
        public ObservableCollection<MedidaTrozoAserrable> MedidasEspecieTres { get; set; } = new();

        public TrozoAserrableViewModel(ICalculatorService calculatorService)
        {
            Title = "Despacho Trozo Aserrable";
            _calculatorService = calculatorService;
            Cliente = new();
        }

        #region Commands

        [RelayCommand]
        void AddItemToList(string numeroLista)
        {
            if(numeroLista == "1")
                MedidasEspecieUno.Add(new MedidaTrozoAserrable() { NumeroLista = 1});
            else if(numeroLista == "2")
                MedidasEspecieDos.Add(new MedidaTrozoAserrable() { NumeroLista = 2});
            else if(numeroLista == "3")
                MedidasEspecieTres.Add(new MedidaTrozoAserrable() { NumeroLista = 3});
        }

        [RelayCommand]
        void RemoveItemFromList(MedidaTrozoAserrable item)
        {
            if(item.NumeroLista == 1)
                MedidasEspecieUno.Remove(item);
            else if(item.NumeroLista == 2)
                MedidasEspecieDos.Remove(item);
            else if(item.NumeroLista == 3)
                MedidasEspecieTres.Remove(item);
        }

        [RelayCommand]
        void ClearPage()
        {
            MedidasEspecieUno.Clear();
            MedidasEspecieDos.Clear();
            MedidasEspecieTres.Clear();
            Cliente = new();
            EspecieUno = "";
            EspecieDos = "";
            EspecieTres = "";
            LargoEspecieUno = 0;
            LargoEspecieDos = 0;
            LargoEspecieTres = 0;
        }
        #endregion
    }
}
