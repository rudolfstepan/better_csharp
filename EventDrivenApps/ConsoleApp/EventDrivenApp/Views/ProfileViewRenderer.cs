using System;
using System.Threading.Tasks;
using EventDrivenApp.Topics;

using EventHelper;

namespace EventDrivenApp.Views
{
    public class ProfileViewRenderer
    {
        public ProfileViewRenderer(LightweightEventBusAsync bus)
        {
            bus.Subscribe<ProfileViewed>(OnProfileViewed);
        }

        private async Task<EventAcknowledge> OnProfileViewed(EventEnvelope<ProfileViewed> envelope)
        {
            Console.WriteLine("Profile view opened.");
            return EventAcknowledge.Handled;
        }
    }
}