using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Storage;
using ForestalCasablancaApp.Pages;
using ForestalCasablancaApp.Services;
using ForestalCasablancaApp.ViewModels;
using Microsoft.Extensions.Logging;

namespace ForestalCasablancaApp;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.UseMauiCommunityToolkit()
			.UseMauiCommunityToolkitCore()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
				fonts.AddFont("fontello.ttf", "Icons");
                fonts.AddFont("fa-brands.otf", "FAB");
                fonts.AddFont("fa-regular.otf", "FAR");
                fonts.AddFont("fa-solid.otf", "FAS");
            });

		builder.Services
			.AddSingleton<ICalculatorService, CalculatorService>()
			.AddSingleton<IFolderPicker>(FolderPicker.Default)
			.AddSingleton<MainPage>()
			.AddSingleton<MainPageViewModel>()
			.AddSingleton<SettingsPage>()
			.AddSingleton<SettingsPageViewModel>()
			.AddTransient<LeñaPage>()
			.AddTransient<LeñaViewModel>()
			.AddTransient<MetroRumaPage>()
			.AddTransient<MetroRumaViewModel>()
			.AddTransient<TrozoAserrablePage>()
			.AddTransient<TrozoAserrableViewModel>();

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
