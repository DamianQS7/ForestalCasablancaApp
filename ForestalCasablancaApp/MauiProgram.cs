using BosquesNalcahue.Services;
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
			.AddTransient<IPermissionsManager, PermissionsManager>()
			.AddSingleton<ICalculatorService, CalculatorService>()
			.AddSingleton<IPdfGeneratorService, PdfGeneratorService>()
			.AddSingleton<IInfoService, InfoService>()
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
			//.AddHttpClient<IRestService, RestService>(client =>
			//{
			//	client.Timeout = TimeSpan.FromSeconds(60);
			//	client.BaseAddress = new Uri(DeviceInfo.Platform == DevicePlatform.Android
			//												 ? "http://10.0.2.2:5207/api"
			//												 : "http://localhost:5207/api");
   //         });

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
