				 # LightweightEventBus

**LightweightEventBus** is an ultra-fast, memory-friendly, and thread-safe event bus system for .NET applications.

It uses:
- **Weak references** for subscribers (no manual unsubscribe needed if the subscriber is garbage collected)
- **Minimal locking** with Copy-on-Write design
- **Direct delegate invocation** (no reflection slowdown)
- **Central management** of events by event type

This makes it ideal for scenarios where you have many dynamic event subscriptions and high-frequency event firing.

---

## Features

- **WeakReference-based**: Subscribers do not prevent garbage collection.
- **Thread-safe**: All operations (subscribe, unsubscribe, publish) are safe across threads.
- **Fast publish**: Minimal overhead on publishing events.
- **Error tolerance**: Exceptions in subscribers do not crash the publisher.
- **Minimal API**: Clean and simple to use.

---

## Getting Started

### 1. Create an instance
```csharp
var eventBus = new LightweightEventBus();
```

### 2. Subscribe to an event
```csharp
eventBus.Subscribe<string>(message =>
{
    Console.WriteLine($"Received: {message}");
});
```

### 3. Publish an event
```csharp
eventBus.Publish("Hello from LightweightEventBus!");
```

### 4. Unsubscribe from an event
```csharp
Action<string> handler = message => Console.WriteLine($"Handler removed: {message}");

// Subscribe
eventBus.Subscribe(handler);

// Later, unsubscribe
eventBus.Unsubscribe(handler);
```

---

## Advanced Example

Handling multiple types of events:

```csharp
public record UserLoggedIn(string Username);
public record OrderPlaced(int OrderId);

// Subscribe to multiple event types
eventBus.Subscribe<UserLoggedIn>(e => Console.WriteLine($"User logged in: {e.Username}"));
eventBus.Subscribe<OrderPlaced>(e => Console.WriteLine($"Order placed: {e.OrderId}"));

// Publish events
eventBus.Publish(new UserLoggedIn("john.doe"));
eventBus.Publish(new OrderPlaced(12345));
```

---

## Internals

- Internally uses `ConcurrentDictionary<Type, IEventGroup>` to manage event groups.
- Each event type has its own handler list with weak references.
- Publishing an event only invokes alive subscribers.
- Dead handlers are cleaned up automatically.

---

## License

This component is free to use in commercial and non-commercial projects.

---

## Author

Designed and optimized for high-performance .NET systems 🚀
