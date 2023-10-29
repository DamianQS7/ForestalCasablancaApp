using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace ForestalCasablancaApp.ViewModels
{
    public partial class SettingsPageViewModel : BaseViewModel
    {
        private readonly IFolderPicker _folderPicker;

        [ObservableProperty] private string _currentWorkingDirectory;
        [ObservableProperty] private string _currentUser;

        public SettingsPageViewModel(IFolderPicker folderPicker)
        {
            Title = "Configuración";
            _folderPicker = folderPicker;
            CurrentWorkingDirectory = Preferences.Get("CurrentWorkingDirectory", FileSystem.Current.AppDataDirectory);
            CurrentUser = Preferences.Get("CurrentUser", "Usuario 01");
        }

        public void GetUserInitials()
        {
            CurrentUser = CurrentUser.Trim();
            string[] names = CurrentUser.Split(' ');

            string initials = "";

            foreach (string name in names)
            {
                initials += name[0];
            }

            Preferences.Set("CurrentUserInitials", initials.ToUpper());
        }

        [RelayCommand]
        async Task PickFolder(CancellationToken cancellationToken)
        {
            var folderPickerResult = await _folderPicker.PickAsync(cancellationToken);
            if (folderPickerResult.IsSuccessful)
            {
                Preferences.Set("CurrentWorkingDirectory", folderPickerResult.Folder.Path);
                CurrentWorkingDirectory = folderPickerResult.Folder.Path;
                await Toast.Make($"Directorio Seleccionado: {folderPickerResult.Folder.Path}", ToastDuration.Long).Show(cancellationToken);
            }
            else
            {
                await Toast.Make($"Tarea Cancelada: {folderPickerResult.Exception.Message}").Show(cancellationToken);
            }
        }

        [RelayCommand]
        async Task SetCurrentUser()
        {
            if(string.IsNullOrEmpty(CurrentUser))
            {
                await Toast.Make($"Debe ingresar un nombre de usuario.").Show();
                return;
            }

            Preferences.Set("CurrentUser", CurrentUser.Trim());
            GetUserInitials();
            await Toast.Make($"Usuario actualizado con éxito.").Show();
        }
    }
}
