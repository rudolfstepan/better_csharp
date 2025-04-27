
# Event Simulator Usage

The EventSimulator class allows you to simulate event flows for testing or demonstration purposes.

## Features

- **SimulateEventAsync**: Fire a single event.
- **SimulateChainAsync**: Fire a sequence of events with optional delays.
- **SimulateRandomEventsAsync**: Fire random events from a set, for stress-testing.

## Example Usage

```csharp
var simulator = new EventSimulator(bus);

// Simulate a basic login flow
await simulator.SimulateChainAsync(new object[]
{
    new ApplicationStart(),
    new LoginRequest(),
    new LoginSuccess("testuser"),
    new MainMenuReady()
}, delayBetweenEventsMs: 500);

// Simulate random topic storms
await simulator.SimulateRandomEventsAsync(new object[]
{
    new LoginRequest(),
    new LoginSuccess("admin"),
    new MainMenuReady(),
    new AdminMainMenuReady()
}, count: 50, maxDelayMs: 100);
```

Enjoy dynamic, automated testing of your Event-Driven App!
