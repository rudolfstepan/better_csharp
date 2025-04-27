
# From MVVM to EVC (Event-View-Component)

## Introduction

MVVM (Model-View-ViewModel) has served us well for many years.  
But as applications grow more modular, dynamic, and asynchronous, a more flexible architecture is needed.

We introduce **EVC**: **Event-View-Component**.

## Why EVC?

| MVVM | EVC |
|:--|:--|
| Views bind to ViewModels | Views subscribe to Events |
| ViewModels directly manage Views | Views react only to Event Topics |
| Tightly coupled logic | Loose, modular orchestration |
| Hard to scale in real-time apps | Scales naturally with asynchronous events |
| Property change tracking everywhere | Minimal state, event-driven reactions |
| Navigation controlled manually | Navigation flows through event-chains |

## Core Principles

- **Topics are the language** of your app.
- **Views are modular Renderers**, only responding to topics.
- **Business Logic triggers Events**, no direct View references.
- **Background Services publish events** seamlessly.

## Basic Structure

1. **Topics** represent events (e.g., `LoginRequest`, `MainMenuReady`).
2. **Views** are passive, self-activating components based on event subscriptions.
3. **EventBus** connects everything asynchronously and safely.

## Advantages

- **Maximum modularity**
- **Easy scalability**
- **Fully asynchronous**
- **Dynamic runtime behavior**
- **Superb testability** (every component can be tested with simple event simulation)

## Conclusion

EVC is the natural evolution for modern, scalable, maintainable applications.
It unleashes the true power of event-driven programming without the heavy burden of classic binding frameworks.

**Welcome to the next generation of application architecture: Event-View-Component!** 🚀
