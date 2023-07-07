using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ForestalCasablancaApp.Controls;
using ForestalCasablancaApp.Models;
using ForestalCasablancaApp.Pages;
using ForestalCasablancaApp.Services;
using System.Threading.Tasks;

namespace ForestalCasablancaApp.ViewModels
{
    public partial class LeñaViewModel : BaseViewModel
    {
        private readonly ICalculatorService _calculatorService;

        [ObservableProperty]
        private DespachoLeñaModel _despacho;

        [ObservableProperty]
        private Cliente _cliente;

        [ObservableProperty]
        private double _totalDespacho;

        static Page Page => Application.Current.MainPage;

        public LeñaViewModel(ICalculatorService calculatorService)
        {
            Title = "Despacho Leña";
            _calculatorService = calculatorService;
            Despacho = new();
            Cliente = new();
        }

        [RelayCommand]
        private void DisplayTotalAsync()
        {
            TotalDespacho =  _calculatorService.CalculateTotalMetrosLeña(Despacho);

            var popup = new ConfirmationPopup();

            Page.ShowPopup(popup);
        }

    }
}
