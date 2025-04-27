using EventDrivenApp.Topics;

namespace EventDrivenMauiTestApp.Views;

public partial class LoginPage : ContentPage
{
    public LoginPage()
    {
        InitializeComponent();
    }

    private async void OnLoginClicked(object sender, EventArgs e)
    {
        await App.EventBus.PublishAsync(new LoginSuccess("user"));
    }

    protected override bool OnBackButtonPressed()
    {
        _ = App.EventBus.PublishAsync(new NavigateBackRequested());
        return true; // Wir haben das Back-Handling selbst übernommen
    }

}