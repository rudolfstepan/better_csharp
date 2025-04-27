using EventHelper;
using EventHelper.Adapters;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;

using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace WinUITestApp
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        int cnt = 0;

        public MainWindow()
        {
            this.InitializeComponent();

            // register event handler
            myButton.Clicked += myButton_Click;
            myButton.Clicked += myButton_Click;
            myButton.Clicked += myButton_Click;

        }

        private void myButton_Click(object sender, EventArgs e)
        {
           myButton.Content = $"Clicked {++this.cnt}" ;
        }
    }

    // Custom button class that uses WeakActionEvent
    // to prevent memory leaks and multiple event handler calls.
    public class CustomButton : Button
    {
        public BulletproofWeakAction<Button, EventArgs> Clicked;

        public CustomButton()
        {
            Clicked = new BulletproofWeakAction<Button, EventArgs>();

            base.Click += (s, e) =>
            {
                // Call the registered event handlers
                Clicked.Invoke(this, EventArgs.Empty);
            };
        }

        // Override the Click event to use the WeakActionEvent
        public new event RoutedEventHandler Click
        {
            add
            {
                // Register the event handler using WeakActionEvent
               
            }
            remove
            {
                // Deregister the event handler using WeakActionEvent
                //Clicked -= value;
            }
        }
    }
}
