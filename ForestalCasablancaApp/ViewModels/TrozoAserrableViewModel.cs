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

        [ObservableProperty]
        private Cliente _cliente;

        public ObservableCollection<MedidaTrozoAserrable> MedidasEspecieUno { get; set; } = new() { new MedidaTrozoAserrable() { Diametro = 10}, new MedidaTrozoAserrable() { Diametro = 20} };
        public ObservableCollection<MedidaTrozoAserrable> MedidasEspecieDos { get; set; }
        public ObservableCollection<MedidaTrozoAserrable> MedidasEspecieTres { get; set; }

        public TrozoAserrableViewModel(ICalculatorService calculatorService)
        {
            Title = "Despacho Trozo Aserrable";
            _calculatorService = calculatorService;
            Cliente = new();
        }

        /// <summary>
        /// I had to do it this way because the ObservableCollection was not allowed in the RelayCommands
        /// </summary>
        /// <param name="item"></param>
        [RelayCommand]
        void RemoveItemFromList1(MedidaTrozoAserrable item)
        {
            MedidasEspecieUno.Remove(item);

            Toast.Make("Medida Eliminada", ToastDuration.Short);
        }
    }
}
