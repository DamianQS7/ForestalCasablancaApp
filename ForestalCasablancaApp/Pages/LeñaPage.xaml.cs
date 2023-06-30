using ForestalCasablancaApp.ViewModels;

namespace ForestalCasablancaApp.Pages;

[QueryProperty(nameof(LeñaViewModel), nameof(LeñaViewModel))]
public partial class LeñaPage : ContentPage
{
	public LeñaPage(LeñaViewModel viewModel)
	{
		InitializeComponent();

		BindingContext = viewModel;
	}
}