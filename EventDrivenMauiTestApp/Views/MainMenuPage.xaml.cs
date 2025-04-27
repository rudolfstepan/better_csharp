using EventDrivenApp.Topics;

namespace EventDrivenMauiTestApp.Views;

public partial class MainMenuPage : ContentPage
{
    public MainMenuPage()
    {
        InitializeComponent();
    }

    private async void OnSettingsClicked(object sender, EventArgs e)
    {
        await App.EventBus.PublishAsync(new SettingsOpened());
    }

    private async void OnProfileClicked(object sender, EventArgs e)
    {
        await App.EventBus.PublishAsync(new ProfileViewed());
    }

    private async void OnLogoutClicked(object sender, EventArgs e)
    {
        await App.EventBus.PublishAsync(new LogoutRequest());
    }

    protected override bool OnBackButtonPressed()
    {
        _ = App.EventBus.PublishAsync(new NavigateBackRequested());
        return true; // Wir haben das Back-Handling selbst übernommen
    }

}