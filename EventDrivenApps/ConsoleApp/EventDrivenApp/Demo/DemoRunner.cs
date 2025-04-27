using System;
using System.Threading.Tasks;
using EventDrivenApp.ConsoleTools;
using EventDrivenApp.Simulation;
using EventDrivenApp.Topics;
using EventHelper;

namespace EventDrivenApp.Demo
{
    public class DemoRunner
    {
        private readonly LightweightEventBusAsync _bus;
        private readonly EventSimulator _simulator;
        private readonly EventConsole _console;

        public DemoRunner()
        {
            _bus = new LightweightEventBusAsync();
            _simulator = new EventSimulator(_bus);
            _console = new EventConsole(_bus);

            _bus.Subscribe<ApplicationStart>(e => Task.FromResult(EventAcknowledge.Handled));
            _bus.Subscribe<LoginRequest>(e => Task.FromResult(EventAcknowledge.Handled));
            _bus.Subscribe<LoginSuccess>(e => Task.FromResult(EventAcknowledge.Handled));
            _bus.Subscribe<MainMenuReady>(e => Task.FromResult(EventAcknowledge.Handled));
            _bus.Subscribe<AdminMainMenuReady>(e => Task.FromResult(EventAcknowledge.Handled));
            _bus.Subscribe<SettingsOpened>(e => Task.FromResult(EventAcknowledge.Handled));
            _bus.Subscribe<ProfileViewed>(e => Task.FromResult(EventAcknowledge.Handled));
            _bus.Subscribe<HelpRequested>(e => Task.FromResult(EventAcknowledge.Handled));
            _bus.Subscribe<LogoutRequest>(e => Task.FromResult(EventAcknowledge.Handled));
            _bus.Subscribe<ApplicationExit>(e => Task.FromResult(EventAcknowledge.Handled));
        }

        public async Task RunAsync()
        {
            Console.WriteLine("Starting Demo Runner...");

            var chainTask = _simulator.SimulateChainAsync(new object[]
            {
                new ApplicationStart(),
                new LoginRequest(),
                new LoginSuccess("user"),
                new MainMenuReady(),
                new SettingsOpened(),
                new ProfileViewed(),
                new HelpRequested(),
                new LogoutRequest(),
                new ApplicationExit()
            }, delayBetweenEventsMs: 500);

            var consoleTask = _console.RunAsync();

            await Task.WhenAny(chainTask, consoleTask);
        }
    }
}