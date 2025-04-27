using EventHelper;
using EventHelper.Adapters;

namespace EventTestMauiApp
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage()
        {
            InitializeComponent();

            //BulletproofWeakAction.DebugLogging = true;

            // although the event handler is registered multiple times,
            // it will only be called once when the event is raised!
            CounterBtn.Clicked += OnClick_Handler;
            CounterBtn.Clicked += OnClick_Handler;
            CounterBtn.Clicked += OnClick_Handler;
        }

        private void OnClick_Handler(object sender, EventArgs arg)
        {
            count++;
            if (count == 1)
                CounterBtn.Text = $"Clicked {count} time";
            else
                CounterBtn.Text = $"Clicked {count} times";
            SemanticScreenReader.Announce(CounterBtn.Text);
        }
    }

    // Custom button class that uses WeakActionEvent
    // to prevent memory leaks and multiple event handler calls.
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
