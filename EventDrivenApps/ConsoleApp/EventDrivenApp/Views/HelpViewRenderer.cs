using System;
using System.Threading.Tasks;
using EventDrivenApp.Topics;

using EventHelper;

namespace EventDrivenApp.Views
{
    public class HelpViewRenderer
    {
        public HelpViewRenderer(LightweightEventBusAsync bus)
        {
            bus.Subscribe<HelpRequested>(OnHelpRequested);
        }

        private async Task<EventAcknowledge> OnHelpRequested(EventEnvelope<HelpRequested> envelope)
        {
            Console.WriteLine("Help view opened.");
            return EventAcknowledge.Handled;
        }
    }
}