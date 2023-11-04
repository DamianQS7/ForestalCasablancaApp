using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ForestalCasablancaApp.Helpers;
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
        #region Properties
        [ObservableProperty] private string _title;
        [ObservableProperty] private bool _isValidInput;
        [ObservableProperty] private string _folio;
        [ObservableProperty] private Cliente _cliente;
        [ObservableProperty] private DatosCamion _datosCamion;
        
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsNotBusy))]
        private bool _isBusy;
        public bool IsNotBusy => !IsBusy;
        public static Page BasePage => Application.Current.MainPage;

        #endregion

        #region Methods
        public static string GenerateFolio() => Preferences.Get("CurrentUserInitials", "NN") + DateTime.Now.ToString("dd'/'MM'/'yy'-'HH:mm");

        public async Task DisplayInputError(InfoMessage infoMessage)
        {
            if (infoMessage == InfoMessage.InvalidDiameter)
            {
                await Shell.Current.DisplayAlert("Error", "El diámetro debe ser un número par", "OK");
            }
            else if (infoMessage == InfoMessage.MissingTrozoData)
            {
                await Shell.Current.DisplayAlert("Error", "Se requiere Largo, Diámetro, y Cantidad para agregar una medida", "OK");
            }
        }

        #endregion

        #region Commands
        [RelayCommand]
        async static Task GoBackAsync()
        {
            await Shell.Current.GoToAsync("..");
        }

        #endregion
    }
}
