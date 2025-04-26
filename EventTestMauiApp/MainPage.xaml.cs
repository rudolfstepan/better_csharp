using EventHelper;

namespace EventTestMauiApp
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage()
        {
            InitializeComponent();

            // although the event handler is registered multiple times,
            // it will only be called once when the event is raised!
            CounterBtn.OnClick += OnClick_Handler;
            CounterBtn.OnClick += OnClick_Handler;
            CounterBtn.OnClick += OnClick_Handler;
        }

        private void OnClick_Handler(string arg)
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
    public class CustomButton : Button
    {
        public WeakActionEvent<string> OnClick = new WeakActionEvent<string>();

        public CustomButton()
        {
            // Subscribe to the Clicked event of the Button
            // This is where we raise our custom event
            Clicked += (s, e) => OnClick?.Invoke("Button clicked");
        }
    }
}
