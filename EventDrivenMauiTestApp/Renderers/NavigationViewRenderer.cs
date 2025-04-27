using EventDrivenApp.Topics;
using EventHelper;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventDrivenMauiTestApp.Renderers
{
    public class NavigationViewRenderer
    {
        public NavigationViewRenderer()
        {
            App.EventBus.Subscribe<NavigateBackRequested>(OnNavigateBack);
        }

        private async Task<EventAcknowledge> OnNavigateBack(EventEnvelope<NavigateBackRequested> envelope)
        {
            Console.WriteLine("[Navigation] Handling Back Navigation...");
            if (Shell.Current.Navigation.NavigationStack.Count > 1)
            {
                await Shell.Current.Navigation.PopAsync(); // <- Pop statt GoTo
            }
            return EventAcknowledge.Handled;
        }

    }

    public static class NavigationHelper
    {
        public static async Task SafeNavigateToAsync(string route)
        {
            try
            {
                if (Shell.Current?.CurrentPage == null || Shell.Current.Navigation == null)
                {
                    Console.WriteLine("[NavigationHelper] Shell not ready, delaying...");
                    await Task.Delay(100); // Retry kurz warten
                }

                var current = Shell.Current?.CurrentPage?.GetType().Name;
                if (!string.Equals(current, route, StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine($"[NavigationHelper] Navigating to {route}...");
                    MainThread.BeginInvokeOnMainThread(async () =>
                    {
                        await Shell.Current.GoToAsync($"///{route}");
                    });

                }
                else
                {
                    Console.WriteLine($"[NavigationHelper] Already at {route}, skipping navigation.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[NavigationHelper] Navigation to {route} failed: {ex.Message}");
            }
        }
    }
}
