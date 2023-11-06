using ForestalCasablancaApp.ViewModels;

namespace ForestalCasablancaApp.Pages;

public partial class TrozoAserrablePage : ContentPage
{
	public TrozoAserrablePage(TrozoAserrableViewModel viewModel)
	{
		InitializeComponent();
        
		BindingContext = viewModel;
	}

    //void OnPickerSelectedIndexChanged(object sender, EventArgs e)
    //{
    //    var picker = (Picker)sender;
    //    int selectedIndex = picker.SelectedIndex;

    //    if (selectedIndex == 0 || selectedIndex == 1)
    //    {
    //        picker.ItemsSource = new List<string>() { "3,2", "4", "5", "6", "7" };
    //    }
    //    else if (selectedIndex == 2 || selectedIndex == 3)
    //    {

    //    }
    //}
}