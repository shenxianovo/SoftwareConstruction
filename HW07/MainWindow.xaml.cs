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

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace HW07
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();
        }

        private async void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            ISearchEngine searchEngine = new SearchEngine();

            string keywords = KeyWordTextBox.Text;

            var bingTask = searchEngine.SearchAsync("Bing", keywords);
            var duckDuckGoTask = searchEngine.SearchAsync("DuckDuckGo", keywords);

            var bingResult = await bingTask;
            var duckDuckGoResult = await duckDuckGoTask;

            BingResultTextBox.Text = bingResult;
            DuckDuckGoResultTextBox.Text = duckDuckGoResult;
        }
    }
}
