using EventDrivenApp.Topics;

using EventHelper;

namespace EventDrivenMauiTestApp.Renderers;

public class LoginViewRenderer
{
    public LoginViewRenderer()
    {
        App.EventBus.Subscribe<SplashCompleted>(OnSplashCompleted);
        App.EventBus.Subscribe<LoginRequest>(OnLoginRequested);
        App.EventBus.Subscribe<LoginSuccess>(OnLoginSuccess);
    }

    private async Task<EventAcknowledge> OnLoginRequested(EventEnvelope<LoginRequest> envelope)
    {
        Console.WriteLine("[Login] Showing Login Page...");
        await NavigationHelper.SafeNavigateToAsync(nameof(Views.LoginPage));
        return EventAcknowledge.Handled;
    }

    private async Task<EventAcknowledge> OnLoginSuccess(EventEnvelope<LoginSuccess> envelope)
    {
        Console.WriteLine($"[Login] Login successful for {envelope.Payload.Username}");
        await App.EventBus.PublishAsync(new MainMenuReady());
        return EventAcknowledge.Handled;
    }

    private async Task<EventAcknowledge> OnSplashCompleted(EventEnvelope<SplashCompleted> envelope)
    {
        Console.WriteLine("[Splash] Completed, requesting Login...");
        await App.EventBus.PublishAsync(new LoginRequest());
        return EventAcknowledge.Handled;
    }
}