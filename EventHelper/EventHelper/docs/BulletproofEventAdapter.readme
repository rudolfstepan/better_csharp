				 # BulletproofEventAdapter - Modern Event Handling for MAUI Controls

## Introduction

This project introduces the **BulletproofEventAdapter<TControl>**, a simple but powerful way to upgrade the event handling of any MAUI control (or .NET control) to a memory-safe, duplicate-safe, and bulletproof event system.

Traditional C# events have well-known weaknesses:
- **Memory leaks** due to strong references between publishers and subscribers
- **Multiple registrations** leading to duplicated event firing ("bouncing" problems)
- **Difficulty cleaning up** during complex view lifecycles (e.g., Xamarin/MAUI Pages)

The **BulletproofEventAdapter** solves all of this elegantly, with minimal code changes.

---

## Key Features

- ✨ Memory-safe event handling using **weak references**
- ✨ Prevents **multiple registrations** of the same event handler
- ✨ **Automatically unsubscribes** from native events when no more handlers are attached
- ✨ Works with **any control** that exposes an event like `Clicked`, `Tapped`, `Completed`, etc.
- ✨ Designed for **maximum compatibility** with existing MAUI controls
- ✨ Ultra-simple integration into existing or custom controls

---

## Core Concept

The **BulletproofEventAdapter** wraps any control and its original event, like `Clicked`:

- It only hooks the native event **once**, when the first subscriber attaches.
- It automatically unhooks the native event **when all subscribers are removed**.
- Internally, it uses a **BulletproofWeakAction** to store event handlers.
- Duplicate handlers are automatically ignored.
- No manual management of event subscriptions needed.

---

## How to Adapt a Control (Example: Button)

### 1. Create a Custom Control

```csharp
using EventHelper.Adapters;
using Microsoft.Maui.Controls;

namespace YourApp.Controls
{
    public class CustomButton : Button
    {
        private readonly BulletproofEventAdapter<Button> _adapter;

        public CustomButton()
        {
            _adapter = new BulletproofEventAdapter<Button>(
                this,
                (ctrl, handler) => ctrl.Clicked += handler,
                (ctrl, handler) => ctrl.Clicked -= handler
            );
        }

        public new event EventHandler? Clicked
        {
            add => _adapter.Clicked += value;
            remove => _adapter.Clicked -= value;
        }
    }
}
```

**That's it!**

The `CustomButton` now behaves exactly like a normal `Button`, but with:
- Memory leak protection
- Duplicate registration protection
- Auto-deregistration

### 2. Use the Control Normally

```csharp
<local:CustomButton Text="Click me" Clicked="OnButtonClicked" />
```

Or in code-behind:

```csharp
myCustomButton.Clicked += MyClickHandler;
myCustomButton.Clicked += MyClickHandler; // safe, no duplicates
```

Only **one** invocation will occur, even if `+=` is mistakenly called multiple times.

---

## Extending to Other Controls

You can adapt **any** event-based control easily:

| Control            | Event              |
|--------------------|--------------------|
| `Entry`             | `Completed`         |
| `ImageButton`       | `Clicked`           |
| `CheckBox`          | `CheckedChanged`    |
| `SwipeView`         | `SwipeEnded`        |
| etc.                | any `EventHandler`  |

**Example: CustomEntry**

```csharp
_adapter = new BulletproofEventAdapter<Entry>(
    this,
    (ctrl, handler) => ctrl.Completed += handler,
    (ctrl, handler) => ctrl.Completed -= handler
);
```

Adaptation becomes consistent, fast, and safe.

---

## Why Is This Important?

In modern MAUI/Xamarin applications, dynamic view lifecycles cause frequent creation and disposal of UI elements.

Without bulletproof event handling:
- You risk memory leaks.
- You risk "bouncing" problems (multiple handler invocations).
- You make debugging complex and frustrating.

The **BulletproofEventAdapter** brings control and safety back to your UI architecture.

---

## Final Notes

- Works out-of-the-box with .NET 6/7/8 and MAUI
- No runtime overhead: uses simple `lock` and weak reference patterns
- Easy to debug and maintain
- Completely compatible with MAUI/NET idioms

---

## Credits

Inspired by discussions from Nick Chapsas and practical real-world needs for rock-solid event management.

**Let's make event handling bulletproof in every project!** 🌟

---

## Future Enhancements
- Support for events with custom `EventArgs` (generic overloads)
- Fluent API helpers (e.g., `control.BindSafeClick(handler)`)

