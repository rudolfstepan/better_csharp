using System;
using System.Threading.Tasks;
using EventDrivenApp.Topics;

using EventHelper;

namespace EventDrivenApp.Views
{
    public class AdminMainMenuViewRenderer
    {
        private readonly LightweightEventBusAsync _bus;

        public AdminMainMenuViewRenderer(LightweightEventBusAsync bus)
        {
            _bus = bus;
            _bus.Subscribe<AdminMainMenuReady>(OnAdminMainMenuReady);
        }

        private async Task<EventAcknowledge> OnAdminMainMenuReady(EventEnvelope<AdminMainMenuReady> envelope)
        {
            Console.WriteLine("AdminMainMenu: Welcome, Admin User!");
            return EventAcknowledge.Handled;
        }
    }
}