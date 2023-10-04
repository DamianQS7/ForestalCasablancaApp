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

        public SettingsPageViewModel(IFolderPicker folderPicker)
        {
            Title = "Configuración";
            _folderPicker = folderPicker;
            CurrentWorkingDirectory = Preferences.Get("CurrentWorkingDirectory", FileSystem.Current.AppDataDirectory);
        }

        [RelayCommand]
        async Task PickFolder(CancellationToken cancellationToken)
        {
            var folderPickerResult = await _folderPicker.PickAsync(cancellationToken);
            if (folderPickerResult.IsSuccessful)
            {
                Preferences.Set("CurrentWorkingDirectory", folderPickerResult.Folder.Path);
                CurrentWorkingDirectory = folderPickerResult.Folder.Path;
                await Toast.Make($"Folder picked: Name - {folderPickerResult.Folder.Name}, Path - {folderPickerResult.Folder.Path}", ToastDuration.Long).Show(cancellationToken);
            }
            else
            {
                await Toast.Make($"Folder is not picked, {folderPickerResult.Exception.Message}").Show(cancellationToken);
            }
        }
    }
}
