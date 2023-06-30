using ForestalCasablancaApp.ViewModels;

namespace ForestalCasablancaApp.Pages;

public partial class MetroRumaPage : ContentPage
{
	public MetroRumaPage(MetroRumaViewModel viewModel)
	{
		InitializeComponent();

		BindingContext = viewModel;
	}
}