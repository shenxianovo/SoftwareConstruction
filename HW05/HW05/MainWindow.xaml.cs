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
        public MainViewModel ViewModel { get; }

        public MainWindow()
        {
            this.InitializeComponent();
            ViewModel = App.GetService<MainViewModel>();

            ButtonSetup();
        }

        private void ButtonSetup()
        {
            foreach (var element in ButtonGrid.Children)
            {
                if (element is Button button)
                {
                    button.Click += Button_Click;
                    switch (button.Name)
                    {
                        case "Button_Divide":
                            button.KeyboardAccelerators.Add(new KeyboardAccelerator
                            {
                                Key = (VirtualKey)191
                            });
                            break;
                        case "Button_Enter":
                            button.KeyboardAccelerators.Add(new KeyboardAccelerator
                            {
                                Key = (VirtualKey)187
                            });
                            break;
                        case "Button_Add":
                            button.KeyboardAccelerators.Add(new KeyboardAccelerator
                            {
                                Modifiers = VirtualKeyModifiers.Shift,
                                Key = (VirtualKey)187
                            });
                            break;
                        case "Button_Subtract":
                            button.KeyboardAccelerators.Add(new KeyboardAccelerator
                            {
                                Key = (VirtualKey)189
                            });
                            break;
                        case "Button_Decimal":
                            button.KeyboardAccelerators.Add(new KeyboardAccelerator
                            {
                                Key = (VirtualKey)190
                            });
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not Button button)
                return;

            button.Focus(FocusState.Keyboard);
            var content = button.Content.ToString();

            if (button.Name == "Button_Back")
            {
                ViewModel.RemoveDigit();
                return;
            }

            if (button.Name == "Button_Info")
            {
                ShowInfo();
            }

            switch (content)
            {
                case "AC":
                    ViewModel.Clear();
                    break;
                default:
                    ViewModel.AppendDigit(content); // possible content: % Operator:(/ x - +) digit, and .
                    break;
            }
        }

        private async void ShowInfo()
        {
            ContentDialog dialog = new ContentDialog
            {
                Title = "About",
                Content = "CalQulator Version 0.1\nAuthor: -QuQ-",
                CloseButtonText = "OK"
            };

            // XamlRoot must be set in the case of a ContentDialog running in a Desktop app
            dialog.XamlRoot = App.MainWindow.Content.XamlRoot;

            var result = await dialog.ShowAsync();
        }
    }
}
