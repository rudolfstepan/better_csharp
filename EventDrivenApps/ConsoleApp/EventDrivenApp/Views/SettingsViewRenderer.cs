using System;
using System.Threading.Tasks;
using EventDrivenApp.Topics;

using EventHelper;

namespace EventDrivenApp.Views
{
    public class SettingsViewRenderer
    {
        public SettingsViewRenderer(LightweightEventBusAsync bus)
        {
            bus.Subscribe<SettingsOpened>(OnSettingsOpened);
        }

        private async Task<EventAcknowledge> OnSettingsOpened(EventEnvelope<SettingsOpened> envelope)
        {
            Console.WriteLine("Settings view opened.");
            return EventAcknowledge.Handled;
        }
    }
}