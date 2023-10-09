using ForestalCasablancaApp.Helpers;
using ForestalCasablancaApp.ViewModels;

namespace ForestalCasablancaApp.Pages;

public partial class SettingsPage : ContentPage
{
	public SettingsPage(SettingsPageViewModel viewModel)
	{
		InitializeComponent();

        BindingContext = viewModel;

        // Set the correct RadioButton when the app is launched.
		switch (Settings.Theme)
		{
            case 0:
                RadBtnSystem.IsChecked = true;
                break;
            case 1:
                RadBtnLight.IsChecked = true;
                break;
            case 2:
                RadBtnDark.IsChecked = true;
                break;
        }
	}

    bool isLoaded;

    protected override void OnAppearing()
    {
        base.OnAppearing();
        isLoaded = true;
    }

    void RadioButton_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        if (!isLoaded)
            return;

        if (!e.Value)
            return;

        var val = (sender as RadioButton)?.Value as string;

        if(string.IsNullOrEmpty(val))
            return;

        switch(val)
        {
            case "System":
                Settings.Theme = 0;
                break;
            case "Light":
                Settings.Theme = 1;
                break;
            case "Dark":
                Settings.Theme = 2;
                break;
        }

        ThemePicker.SetTheme();
    }
}