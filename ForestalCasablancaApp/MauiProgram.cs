using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Core;
using ForestalCasablancaApp.Pages;
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
			});

		builder.Services
			.AddSingleton<MainPage>()
			.AddSingleton<MainPageViewModel>()
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
