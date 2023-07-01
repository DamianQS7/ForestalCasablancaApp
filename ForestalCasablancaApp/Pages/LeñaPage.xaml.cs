using ForestalCasablancaApp.ViewModels;

namespace ForestalCasablancaApp.Pages;

public partial class LeñaPage : ContentPage
{
	public LeñaPage(LeñaViewModel viewModel)
	{
		InitializeComponent();

		BindingContext = viewModel;
	}
}