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
            Console.WriteLine("Commands:");
            Console.WriteLine(" start, login_user, login_admin, settings, profile, help, logout, exit_app, exit_console");

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
                    case "settings":
                        await _bus.PublishAsync(new SettingsOpened());
                        break;
                    case "profile":
                        await _bus.PublishAsync(new ProfileViewed());
                        break;
                    case "help":
                        await _bus.PublishAsync(new HelpRequested());
                        break;
                    case "logout":
                        await _bus.PublishAsync(new LogoutRequest());
                        break;
                    case "exit_app":
                        await _bus.PublishAsync(new ApplicationExit());
                        break;
                    case "exit_console":
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