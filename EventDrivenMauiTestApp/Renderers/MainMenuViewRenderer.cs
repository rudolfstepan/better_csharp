
using EventDrivenApp.Topics;

using EventHelper;

namespace EventDrivenMauiTestApp.Renderers;

public class MainMenuViewRenderer
{
    public MainMenuViewRenderer()
    {
        App.EventBus.Subscribe<MainMenuReady>(OnMainMenuReady);
    }

    private async Task<EventAcknowledge> OnMainMenuReady(EventEnvelope<MainMenuReady> envelope)
    {
        Console.WriteLine("[MainMenu] Showing Main Menu Page...");
        await NavigationHelper.SafeNavigateToAsync(nameof(Views.MainMenuPage));
        return EventAcknowledge.Handled;
    }
}