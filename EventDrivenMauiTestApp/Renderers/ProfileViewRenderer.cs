
using EventDrivenApp.Topics;

using EventHelper;

namespace EventDrivenMauiTestApp.Renderers;

public class ProfileViewRenderer
{
    public ProfileViewRenderer()
    {
        App.EventBus.Subscribe<ProfileViewed>(OnProfileViewed);
    }

    private async Task<EventAcknowledge> OnProfileViewed(EventEnvelope<ProfileViewed> envelope)
    {
        Console.WriteLine("[Profile] Showing Profile Page...");
        await NavigationHelper.SafeNavigateToAsync(nameof(Views.ProfilePage));
        return EventAcknowledge.Handled;
    }
}