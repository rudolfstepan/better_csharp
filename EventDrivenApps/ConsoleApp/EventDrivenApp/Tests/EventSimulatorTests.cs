using System.Threading.Tasks;
using EventDrivenApp.EventCore;
using EventDrivenApp.EventCore.Topics;
using EventDrivenApp.Simulation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EventDrivenApp.Tests
{
    [TestClass]
    public class EventSimulatorTests
    {
        private LightweightEventBusAsync _bus;
        private EventSimulator _simulator;
        private bool _mainMenuTriggered;
        private bool _adminMenuTriggered;

        [TestInitialize]
        public void Setup()
        {
            _bus = new LightweightEventBusAsync();
            _simulator = new EventSimulator(_bus);

            _bus.Subscribe<MainMenuReady>(async envelope =>
            {
                _mainMenuTriggered = true;
                return EventAcknowledge.Handled;
            });

            _bus.Subscribe<AdminMainMenuReady>(async envelope =>
            {
                _adminMenuTriggered = true;
                return EventAcknowledge.Handled;
            });
        }

        [TestMethod]
        public async Task SimulateUserLoginFlow_ShouldTriggerMainMenu()
        {
            await _simulator.SimulateChainAsync(new object[]
            {
                new ApplicationStart(),
                new LoginRequest(),
                new LoginSuccess("user"),
                new MainMenuReady()
            });

            Assert.IsTrue(_mainMenuTriggered, "MainMenuReady was not triggered.");
            Assert.IsFalse(_adminMenuTriggered, "AdminMainMenuReady should not be triggered.");
        }

        [TestMethod]
        public async Task SimulateAdminLoginFlow_ShouldTriggerAdminMainMenu()
        {
            await _simulator.SimulateChainAsync(new object[]
            {
                new ApplicationStart(),
                new LoginRequest(),
                new LoginSuccess("admin"),
                new AdminMainMenuReady()
            });

            Assert.IsTrue(_adminMenuTriggered, "AdminMainMenuReady was not triggered.");
            Assert.IsFalse(_mainMenuTriggered, "MainMenuReady should not be triggered.");
        }
    }
}