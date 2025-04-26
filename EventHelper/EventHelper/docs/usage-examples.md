# Usage Examples

## Example 1: Safe Custom Button

```csharp
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

## Example 2: Adapting Entry.Completed

```csharp
public class CustomEntry : Entry
{
    private readonly BulletproofEventAdapter<Entry> _adapter;

    public CustomEntry()
    {
        _adapter = new BulletproofEventAdapter<Entry>(
            this,
            (ctrl, handler) => ctrl.Completed += handler,
            (ctrl, handler) => ctrl.Completed -= handler
        );
    }

    public event EventHandler? Completed
    {
        add => _adapter.Clicked += value;
        remove => _adapter.Clicked -= value;
    }
}
```

## Example 3: Adapting CheckBox CheckedChanged

```csharp
public class CustomCheckBox : CheckBox
{
    private readonly BulletproofEventAdapter<CheckBox> _adapter;

    public CustomCheckBox()
    {
        _adapter = new BulletproofEventAdapter<CheckBox>(
            this,
            (ctrl, handler) => ctrl.CheckedChanged += handler,
            (ctrl, handler) => ctrl.CheckedChanged -= handler
        );
    }

    public event EventHandler? CheckedChanged
    {
        add => _adapter.Clicked += value;
        remove => _adapter.Clicked -= value;
    }
}
``` 
