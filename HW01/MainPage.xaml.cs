using HW01.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace HW01
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        // 控件添加到页面的控件树时触发
        private void NavView_Loaded(object sender, RoutedEventArgs e)
        {
            foreach (NavigationViewItemBase item in NavView.MenuItems)
            {
                if (item is NavigationViewItem && item.Tag.ToString() == "ExerciseOne_Page")
                {
                    NavView.SelectedItem = item;
                    break;
                }
            }
            ContentFrame.Navigate(typeof(ExerciseOnePage));
        }

        // 选择某个菜单项时触发
        private void NavView_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            if (args.IsSettingsInvoked)
            {
                ContentFrame.Navigate(typeof(SettingsPage));
            }

            // 奇怪，用 InvokenItem 似乎不行（）它返回的是这个 Item 的 Content
            NavigationViewItem ItemContent = args.InvokedItemContainer as NavigationViewItem;
            if (ItemContent == null)
            {
                return;
            }

            switch (ItemContent.Tag)
            {
                case "ExerciseOne_Page":
                    ContentFrame.Navigate(typeof(ExerciseOnePage));
                    break;
                case "ExerciseTwo_Page":
                    ContentFrame.Navigate(typeof(ExerciseTwoPage));
                    break;
                default:
                    break;
            }
        }

        private void NavView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
        }

    }
}
