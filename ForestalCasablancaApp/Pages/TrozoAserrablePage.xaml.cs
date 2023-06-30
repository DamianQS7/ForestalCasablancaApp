using ForestalCasablancaApp.ViewModels;

namespace ForestalCasablancaApp.Pages;

public partial class TrozoAserrablePage : ContentPage
{
	public TrozoAserrablePage(TrozoAserrableViewModel viewModel)
	{
		InitializeComponent();

		BindingContext = viewModel;
	}
}