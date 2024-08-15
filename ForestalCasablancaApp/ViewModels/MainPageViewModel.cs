using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForestalCasablancaApp.ViewModels
{
    public partial class MainPageViewModel
    {
        public bool IsBusy { get; set; } = false;

        public MainPageViewModel()
        {
            QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;
            CopyFileToAppDataDirectory("pdf_image.png");
        }

        [RelayCommand]
        async Task GoToDespachoAsync(string name)
        {
            if (IsBusy)
                return;

            try
            {
                IsBusy = true;
                await Shell.Current.GoToAsync(name);
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "Ok");
            }
            finally
            {
                IsBusy = false;
            }
            
        }

        public async Task CopyFileToAppDataDirectory(string filename)
        {
            // Open the source file
            using Stream inputStream = await FileSystem.Current.OpenAppPackageFileAsync(filename);

            // Create an output filename
            string targetFile = Path.Combine(FileSystem.Current.AppDataDirectory, filename);

            // Copy the file to the AppDataDirectory
            using FileStream outputStream = File.Create(targetFile);
            await inputStream.CopyToAsync(outputStream);
        }
    }
}
