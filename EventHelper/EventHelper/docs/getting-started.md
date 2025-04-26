# Getting Started

## Installing EventHelper

Add the EventHelper library to your project (future NuGet package planned).

For now, simply add the `EventHelper` folder or compile the solution from GitHub.

## Basic Usage

### 1. Create a Custom Control

```csharp
using EventHelper.Adapters;
using Microsoft.Maui.Controls;

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
```

### 2. Use the Custom Control

```csharp
<local:CustomButton Text="Click me" Clicked="OnButtonClicked" />
```

or in code-behind:

```csharp
myButton.Clicked += (s, e) => { Console.WriteLine("Clicked!"); };
```

No more memory leaks, no more bouncing, fully bulletproof.

## Supported Platforms

- .NET 6+
- .NET MAUI
- .NET Android / iOS via MAUI
- Classic WPF or WinForms (with minor adaptation)

## Requirements

- C# 10.0+
- Nullable enabled (`#nullable enable`)
