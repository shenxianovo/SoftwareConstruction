using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Windows.System;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace HW05
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();
            ViewModel = App.GetService<MainViewModel>();

            foreach (var element in ButtonGrid.Children)
            {
                if (element is Button button)
                {
                    button.Click += Button_Click;
                }
            }
        }

        public MainViewModel ViewModel { get; }

        private void KeyboradListener(object sender, KeyRoutedEventArgs e)
        {
            switch (e.Key)
            {
                case VirtualKey.Enter:
                    ViewModel.Text = "Enter key pressed";
                    break;
                case VirtualKey.Space:
                    ViewModel.Text = "Space key pressed";
                    break;
                // Add more cases as needed
                default:
                    ViewModel.Text = $"Key {e.Key} pressed";
                        break;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not Button button)
                return;

            switch (button.Content.ToString())
            {
                case "AC":
                    ViewModel.Text = "0";
                    break;
                default:
                    break;
            }

            button.Focus(FocusState.Keyboard);
        }
    }
}
