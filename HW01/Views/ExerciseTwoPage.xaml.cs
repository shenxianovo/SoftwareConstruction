using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace HW01.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ExerciseTwoPage : Page
    {
        public ExerciseTwoPage()
        {
            this.InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int a = Int32.Parse(boxA.Text);
                int b = Int32.Parse(boxB.Text);
                int c = a;

                if (a > b)
                {
                    text.Text = PrimeNumber.CalculateBetween(b, a);
                }
                else
                {
                    text.Text = PrimeNumber.CalculateBetween(a, b);
                }
            }
            catch (FormatException)
            {
                MessageDialog dialog = new MessageDialog("Invalid Parameter!");
                await dialog.ShowAsync();
            }
        }
    }

    public class PrimeNumber
    {
        private static bool IsPrime(int n)
        {
            if (n <= 1)
            {
                throw new FormatException();
            }

            // Some optimization
            if (n == 2 || n == 3 || n == 5 || n == 7 || n == 11 || n == 13)
            {
                return true;
            }

            if (n % 2 == 0)
            {
                return false;
            }

            for (int i = 3; i < n; i++)
            {
                if (n % i == 0)
                {
                    return false;
                }
            }

            return true;
        }

        // [a, b)
        public static string CalculateBetween(int a, int b)
        {
            StringBuilder retVal = new StringBuilder();

            for (int i = a; i < b; i++)
            {
                if (IsPrime(i))
                {
                    retVal.AppendLine(i.ToString());
                }
            }

            return retVal.ToString();
        }
    }
}
