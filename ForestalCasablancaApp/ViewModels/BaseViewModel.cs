using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ForestalCasablancaApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForestalCasablancaApp.ViewModels
{
    public abstract partial class BaseViewModel : ObservableObject
    {
        public BaseViewModel() { }

        [ObservableProperty]
        private string _title;

        [ObservableProperty]
        private bool _isValidInput;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsNotBusy))]
        private bool _isBusy;

        [ObservableProperty]
        private Cliente _cliente;

        [ObservableProperty]
        private DatosCamion _datosCamion;

        public bool IsNotBusy => !IsBusy;
        public DateTime CurrentDate => DateTime.Now;
        public static Page BasePage => Application.Current.MainPage;

        [RelayCommand]
        async Task GoBackAsync()
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}
