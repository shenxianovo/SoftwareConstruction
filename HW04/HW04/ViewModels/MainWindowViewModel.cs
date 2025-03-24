using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HW04.Library.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HW04.ViewModels
{
    public partial class MainWindowViewModel : ObservableObject
    {
        private FileIO _fileIO;

        [ObservableProperty]
        private string _concatenatedFileContent = "空空如也~";

        public List<string> PickedFilePaths { get; set; } = new List<string>();

        public MainWindowViewModel()
        {
            _fileIO = new FileIO();
        }

        public void Concatenate()
        {
            byte[] content = _fileIO.Concatenate(PickedFilePaths.ToArray());
            ConcatenatedFileContent = Encoding.UTF8.GetString(content.ToArray());
            PickedFilePaths.Clear();
        }
    }
}
