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

        public LeñaViewModel(ICalculatorService calculatorService)
        {
            Title = "Despacho Leña";
            _calculatorService = calculatorService;
            Despacho = new DespachoLeñaModel();
            Cliente = new();
        }

        [RelayCommand]
        private void DisplaySummaryAsync()
        {
            _calculatorService.CalculateTotalMetrosLeña(Despacho);

            var popup = new ConfirmationPopup();

            BasePage.ShowPopup(popup);
        }

    }
}
