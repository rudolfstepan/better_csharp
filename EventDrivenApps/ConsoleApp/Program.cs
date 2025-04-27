using EventDrivenApp.ConsoleTools;
using EventDrivenApp.Demo;
using EventDrivenApp.Simulation;
using EventDrivenApp.Topics;

using EventHelper;

using System;

namespace ConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {

            var _bus = new LightweightEventBusAsync();
            var _simulator = new EventSimulator(_bus);
            var _console = new EventConsole(_bus);

            _bus.SubscribeWithLogging<ApplicationStart>();
            _bus.SubscribeWithLogging<LoginRequest>();
            _bus.SubscribeWithLogging<LoginSuccess>(e => $"[Event Received] LoginSuccess for user: {e.Payload.Username}");
            _bus.SubscribeWithLogging<MainMenuReady>();
            _bus.SubscribeWithLogging<AdminMainMenuReady>();             
            _bus.SubscribeWithLogging<SettingsOpened>();
            _bus.SubscribeWithLogging<ProfileViewed>();
            _bus.SubscribeWithLogging<HelpRequested>();
            _bus.SubscribeWithLogging<LogoutRequest>();
            _bus.SubscribeWithLogging<ApplicationExit>();

            _console.RunAsync().GetAwaiter().GetResult();

            DemoRunner demoRunner = new DemoRunner();
            demoRunner.RunAsync().GetAwaiter().GetResult();

        }
    }
}
