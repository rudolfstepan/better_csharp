using EventDrivenMauiTestApp.Views;

namespace EventDrivenMauiTestApp;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        // Dynamisches Registrieren der restlichen Seiten
        Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
        Routing.RegisterRoute(nameof(MainMenuPage), typeof(MainMenuPage));
        Routing.RegisterRoute(nameof(SettingsPage), typeof(SettingsPage));
        Routing.RegisterRoute(nameof(ProfilePage), typeof(ProfilePage));
    }
}