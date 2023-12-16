using CommunityToolkit.Maui.Alerts;
using ForestalCasablancaApp.Helpers;

namespace BosquesNalcahue.Services
{
    public class InfoService : IInfoService
    {
        public async Task ShowAlert(InfoMessage infoMessage) 
        {
            if (infoMessage is InfoMessage.InvalidDiameter)
            {
                await Shell.Current.DisplayAlert("Error",
                    "El diámetro debe ser un número par", "OK");
            }
            else if (infoMessage is InfoMessage.MissingTrozoData)
            {
                await Shell.Current.DisplayAlert("Error",
                    "Se requiere Largo, Diámetro, y Cantidad para agregar una medida", "OK");
            }
            else if (infoMessage is InfoMessage.MissingMedidaTrozoAserrable)
            {
                await Shell.Current.DisplayAlert("Error",
                    "Debe ingresar al menos una medida", "OK");
            }
            else if (infoMessage is InfoMessage.MissingLeñaData)
            {
                await Shell.Current.DisplayAlert("Error",
                    "Debe incluir 'Largo Camión', 'N° de Bancos' y por lo menos una altura", "OK");
            }
            else if (infoMessage is InfoMessage.MissingMetroRumaData)
            {
                await Shell.Current.DisplayAlert("Error",
                    "Debe incluir 'Ancho Camión', 'N° de Bancos' y por lo menos una altura", "OK");
            }
            else if (infoMessage is InfoMessage.InvalidPalomera)
            {
                await Shell.Current.DisplayAlert("Error",
                    "Los datos introducidos para el cálculo de la palomera son incorrectos", "OK");
            }
        }

        public async Task ShowAlert(string message)
        {
            await Shell.Current.DisplayAlert("Error", message, "OK");
        }

        public async Task ShowToast(string message)
        {
            await Toast.Make(message).Show();
        }
    }
}
