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

                List<int> primes;

                if (a > b)
                {
                    primes = PrimeNumber.CalculateBetween(b, a);
                }
                else
                {
                    primes = PrimeNumber.CalculateBetween(a, b);
                }

                StringBuilder text = new StringBuilder();
                for (int i = 0; i < primes.Count; i++)
                {
                    if (i > 0 && i % 10 == 0)
                    {
                        text.AppendLine();
                    }
                    text.Append(primes[i] + " ");
                }

                textBlock.Text = text.ToString();
            }
            catch (FormatException)
            {
                MessageDialog dialog = new MessageDialog("Invalid Parameter!\nPleace enter a non-negative integer");
                await dialog.ShowAsync();
            }
        }
    }

    public class PrimeNumber
    {
        private static bool IsPrime(int n)
        {
            if (n < 0)
            {
                throw new FormatException();
            }
            if (n == 2)
                return true;

            // Some optimization
            if (n % 2 == 0 || n == 1)
            {
                return false;
            }

            for (int i = 3; i < n; i+=2)
            {
                if (n % i == 0)
                {
                    return false;
                }
            }

            return true;
        }

        // [a, b)
        public static List<int> CalculateBetween(int a, int b)
        {
            List<int> retVal = new List<int>();

            for (int i = a; i < b; i++)
            {
                if (IsPrime(i))
                {
                    retVal.Add(i);
                }
            }

            return retVal;
        }
    }
}
