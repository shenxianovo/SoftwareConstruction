using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.UI.Xaml.Shapes;
using System.Runtime.InteropServices;
using Microsoft.UI.Windowing;
using Windows.Graphics;
using Microsoft.UI.Input;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using HW05.Services;
using HW05.Models;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace HW05
{
    public partial class App : Application
    {
        public IHost Host { get; }
        public static Window MainWindow { get; } = new MainWindow();

        public static T GetService<T>() where T : class
        {
            if ((App.Current as App)!.Host.Services.GetService(typeof(T)) is not T service)
            {
                throw new ArgumentException($"{typeof(T)} needs to be registered in ConfigureServices within App.xaml.cs.");
            }

            return service;
        }

        public App()
        {
            this.InitializeComponent();

            Host = Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    // Services
                    services.AddTransient<ICalculator, Calculator>();

                    // Views and ViewModels
                    services.AddSingleton<Main>();
                    services.AddTransient<MainViewModel>();
                })
                .Build();
        }

        [DllImport("user32.dll")]
        static extern int GetDpiForWindow(IntPtr hwnd);

        protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {

            IntPtr hWnd = WinRT.Interop.WindowNative.GetWindowHandle(MainWindow);
            var windowId = Microsoft.UI.Win32Interop.GetWindowIdFromWindow(hWnd);
            var appWindow = AppWindow.GetFromWindowId(windowId);

            // disable Resize
            int scale = GetDpiForWindow(hWnd);
            int width = (int)(300 * 1.75);
            int height = (int)(450 * 1.83);

            appWindow.Resize(new SizeInt32(width, height));

            var presenter = appWindow.Presenter as OverlappedPresenter;
            presenter!.IsResizable = false;
            presenter!.IsMaximizable = false;

            MainWindow.ExtendsContentIntoTitleBar = true;
            MainWindow.Activate();
        }
    }
}
