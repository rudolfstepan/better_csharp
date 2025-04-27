using System.Threading.Tasks;
using EventDrivenApp.Topics;

using EventHelper;

namespace EventDrivenApp.Services
{
    public class AdminLoginHandler
    {
        private readonly LightweightEventBusAsync _bus;

        public AdminLoginHandler(LightweightEventBusAsync bus)
        {
            _bus = bus;
            _bus.Subscribe<LoginSuccess>(OnLoginSuccess);
        }

        private async Task<EventAcknowledge> OnLoginSuccess(EventEnvelope<LoginSuccess> envelope)
        {
            if (envelope.Payload.Username == "admin")
            {
                await _bus.PublishAsync(new AdminMainMenuReady());
            }
            else
            {
                await _bus.PublishAsync(new MainMenuReady());
            }
            return EventAcknowledge.Handled;
        }
    }
}