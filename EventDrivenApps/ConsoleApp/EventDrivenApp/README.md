# Event-Driven Application Core (EVC Framework)

> Welcome to the next generation of scalable, modular, dynamic applications.

---

## What is EVC?

**EVC** stands for **Event-View-Component**:
- Events are the **language** of the application.
- Views are **modular renderers** responding to events.
- Components are **listeners** that create dynamic workflows.

## Key Features

- Asynchronous, event-driven architecture
- Fully modular and scalable
- Natural support for dynamic workflows
- Full simulation, testing, and live event injection support
- Roadmap ready for Open Source and NuGet packaging

## Project Structure

| Folder | Purpose |
|:--|:--|
| **EventCore** | Core event system: Bus, Envelope, Topics |
| **Views** | ViewRenderers for Splash, Login, Main Menu, Admin Menu |
| **Services** | Business logic triggered by events |
| **Simulation** | EventSimulator for load/stress testing |
| **ConsoleTools** | Live Event Console for manual event firing |
| **Demo** | DemoRunner for combined random event storm & console |
| **Tests** | Unit and integration tests |
| **Docs** | Full documentation, diagrams, philosophy, and roadmap |

## Quick Start

1. Add your `LightweightEventBusAsync` to `EventCore`.
2. Create an `ApplicationStartup` and initialize all ViewRenderers and Services.
3. (Optional) Use `DemoRunner` or `EventConsole` for live testing.
4. Enjoy your living, flowing, dynamic application.

```csharp
var startup = new ApplicationStartup();
await startup.RunAsync();

// Optionally, for live interaction:
var demo = new DemoRunner();
await demo.RunAsync();
```

## Documentation

- Philosophy: Why EVC?
- Roadmap: How to Open Source and Grow
- Diagrams: Full system architecture
- Usage Guides: EventSimulator, EventConsole, DemoRunner

---

# Welcome to the future of software architecture.

> Build dynamic applications that flow like life itself.

🚀