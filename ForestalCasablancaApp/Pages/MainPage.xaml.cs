using CommunityToolkit.Mvvm.Input;
using ForestalCasablancaApp.Pages;
using ForestalCasablancaApp.ViewModels;

namespace ForestalCasablancaApp;

public partial class MainPage : ContentPage
{
	public MainPage(MainPageViewModel viewModel)
	{
		InitializeComponent();

        BindingContext = viewModel;
	}
}

