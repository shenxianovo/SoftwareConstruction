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
using HW04.ViewModels;
using System.Text;
using Windows.Storage.Pickers;
using Windows.Storage;
using HW04.Helpers;
using System.Diagnostics;
using Windows.Storage.Provider;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace HW04
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();

            ViewModel = new MainWindowViewModel();
        }

        public MainWindowViewModel ViewModel { get; }

        private async void PickFilesButton_Click(object sender, RoutedEventArgs e)
        {
            //disable the button to avoid double-clicking
            var senderButton = sender as Button;
            senderButton!.IsEnabled = false;

            // Clear previous returned file name, if it exists, between iterations of this scenario
            PickFilesOutputTextBlock.Text = "";

            IReadOnlyList<StorageFile> files = await FilePickerHelper.GetFilesAsync(this);
           
            if (files.Count > 0)
            {
                StringBuilder output = new StringBuilder("Picked files:\n");
                foreach (StorageFile file in files)
                {
                    output.Append(file.Path + "\n");
                    ViewModel.PickedFilePaths.Add(file.Path);
                }
                PickFilesOutputTextBlock.Text = output.ToString();
            }
            else
            {
                PickFilesOutputTextBlock.Text = "Operation cancelled.";
            }

            ViewModel.Concatenate();

            //re-enable the button
            senderButton.IsEnabled = true;
        }

        private void OpenDataFolderButton_Click(object sender, RoutedEventArgs e)
        {
            string dataPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data");
            if (Directory.Exists(dataPath))
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = dataPath,
                    UseShellExecute = true,
                    Verb = "open"
                });
            }
        }

        private async void SaveFileButton_Click(object sender, RoutedEventArgs e)
        {
            //disable the button to avoid double-clicking
            var senderButton = sender as Button;
            senderButton!.IsEnabled = false;

            // Clear previous returned file name, if it exists, between iterations of this scenario
            PickFilesOutputTextBlock.Text = "";

            // Create a file picker
            FileSavePicker savePicker = new Windows.Storage.Pickers.FileSavePicker();

            // See the sample code below for how to make the window accessible from the App class.
            var window = this;

            // Retrieve the window handle (HWND) of the current WinUI 3 window.
            var hWnd = WinRT.Interop.WindowNative.GetWindowHandle(window);

            // Initialize the file picker with the window handle (HWND).
            WinRT.Interop.InitializeWithWindow.Initialize(savePicker, hWnd);

            // Set options for your file picker
            savePicker.SuggestedStartLocation = PickerLocationId.Desktop;
            // Dropdown of file types the user can save the file as
            savePicker.FileTypeChoices.Add("Plain Text", new List<string>() { ".txt" });
            // Default file name if the user does not type one in or select a file to replace
            savePicker.SuggestedFileName = "concatenated";

            // Open the picker for the user to pick a file
            StorageFile file = await savePicker.PickSaveFileAsync();
            if (file != null)
            {
                // Prevent updates to the remote version of the file until we finish making changes and call CompleteUpdatesAsync.
                CachedFileManager.DeferUpdates(file);

                // write to file
                await FileIO.WriteTextAsync(file, ViewModel.ConcatenatedFileContent);

                // Let Windows know that we're finished changing the file so the other app can update the remote version of the file.
                // Completing updates may require Windows to ask for user input.
                FileUpdateStatus status = await CachedFileManager.CompleteUpdatesAsync(file);
                if (status == FileUpdateStatus.Complete)
                {
                    PickFilesOutputTextBlock.Text = "File " + file.Name + " was saved.";
                }
                else if (status == FileUpdateStatus.CompleteAndRenamed)
                {
                    PickFilesOutputTextBlock.Text = "File " + file.Name + " was renamed and saved.";
                }
                else
                {
                    PickFilesOutputTextBlock.Text = "File " + file.Name + " couldn't be saved.";
                }
            }
            else
            {
                PickFilesOutputTextBlock.Text = "Operation cancelled.";
            }

            //re-enable the button
            senderButton.IsEnabled = true;
        }
    }
}
