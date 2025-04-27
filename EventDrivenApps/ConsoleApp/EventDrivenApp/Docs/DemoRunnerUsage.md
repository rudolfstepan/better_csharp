
# Demo Runner Usage

The DemoRunner class does two things in parallel:

- Fires 100 random events from different topics
- Opens the EventConsole for manual event injection

## How to Use

```csharp
var demo = new DemoRunner();
await demo.RunAsync();
```

Perfect for demonstrating the flexibility, stability, and modularity of your Event-Driven Architecture!
