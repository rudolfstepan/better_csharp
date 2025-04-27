
# Event Console Usage

The EventConsole allows you to manually trigger events during runtime.

## Commands

- `start` - Fires ApplicationStart event.
- `login_user` - Simulates a normal user login.
- `login_admin` - Simulates an admin user login.
- `exit` - Exits the EventConsole.

## How to Use

1. Instantiate EventConsole with your LightweightEventBusAsync instance.
2. Call `RunAsync()` during your Program startup or testing phase.
3. Type commands and watch your event-driven app react in real-time!

Example:

```csharp
var console = new EventConsole(bus);
await console.RunAsync();
```

Enjoy live interaction with your Event-Driven Architecture!
