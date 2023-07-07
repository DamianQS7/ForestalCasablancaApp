using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.Input;
using ForestalCasablancaApp.Controls;
using ForestalCasablancaApp.ViewModels;

namespace ForestalCasablancaApp.Pages;

public partial class LeñaPage : ContentPage
{
	public LeñaPage(LeñaViewModel viewModel)
	{
		InitializeComponent();

		BindingContext = viewModel;
	}

	
	void ShowConfirmationPopup(object sender, EventArgs args)
	{
        var Popup = new ConfirmationPopup();

		this.ShowPopup(Popup);
    }
}