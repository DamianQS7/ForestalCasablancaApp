using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForestalCasablancaApp.ViewModels
{
    public partial class MainPageViewModel : BaseViewModel
    {
        public MainPageViewModel()
        {
            Title = string.Empty;
        }

        [RelayCommand]
        async Task GoToDespachoAsync(string name)
        {
            await Shell.Current.GoToAsync(name);
        }
    }
}
