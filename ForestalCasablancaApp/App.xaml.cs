using ForestalCasablancaApp.Helpers;

namespace ForestalCasablancaApp;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();

		ThemePicker.SetTheme();

		MainPage = new AppShell();
	}

	protected override void OnStart()
	{
		OnResume();
    }

	protected override void OnSleep()
	{
		ThemePicker.SetTheme();
		RequestedThemeChanged -= AppRequestedThemeChanged;
    }

	protected override void OnResume()
	{
		ThemePicker.SetTheme();
		RequestedThemeChanged += AppRequestedThemeChanged;
	}

	private void AppRequestedThemeChanged(object sender, AppThemeChangedEventArgs e)
	{
		MainThread.BeginInvokeOnMainThread(() =>
		{
			ThemePicker.SetTheme();
		});
	}
}
