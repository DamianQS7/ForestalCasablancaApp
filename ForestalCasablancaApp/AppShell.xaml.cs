using ForestalCasablancaApp.Pages;

namespace ForestalCasablancaApp;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();

		Routing.RegisterRoute(nameof(LeñaPage), typeof(LeñaPage));
        Routing.RegisterRoute(nameof(MetroRumaPage), typeof(MetroRumaPage));
        Routing.RegisterRoute(nameof(TrozoAserrablePage), typeof(TrozoAserrablePage));
    }
}
