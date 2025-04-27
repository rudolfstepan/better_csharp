using EventDrivenApp.Topics;

namespace EventDrivenMauiTestApp.Views;

public partial class ProfilePage : ContentPage
{
    public ProfilePage()
    {
        InitializeComponent();
    }

    protected override bool OnBackButtonPressed()
    {
        _ = App.EventBus.PublishAsync(new NavigateBackRequested());
        return true; // Wir haben das Back-Handling selbst übernommen
    }

}