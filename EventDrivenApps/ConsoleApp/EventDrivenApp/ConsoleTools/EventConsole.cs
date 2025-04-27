using System;
using System.Threading.Tasks;
using EventDrivenApp.Topics;

using EventHelper;

namespace EventDrivenApp.ConsoleTools
{
    public class EventConsole
    {
        private readonly LightweightEventBusAsync _bus;

        public EventConsole(LightweightEventBusAsync bus)
        {
            _bus = bus;
        }

        public async Task RunAsync()
        {
            Console.WriteLine("Event Console started. Type a command:");
            Console.WriteLine("Commands: start, login_user, login_admin, exit");

            while (true)
            {
                Console.Write("> ");
                var input = Console.ReadLine()?.Trim().ToLower();

                switch (input)
                {
                    case "start":
                        await _bus.PublishAsync(new ApplicationStart());
                        break;
                    case "login_user":
                        await _bus.PublishAsync(new LoginRequest());
                        await _bus.PublishAsync(new LoginSuccess("user"));
                        break;
                    case "login_admin":
                        await _bus.PublishAsync(new LoginRequest());
                        await _bus.PublishAsync(new LoginSuccess("admin"));
                        break;
                    case "exit":
                        Console.WriteLine("Exiting Event Console.");
                        return;
                    default:
                        Console.WriteLine("Unknown command.");
                        break;
                }
            }
        }
    }
}