using CommunityToolkit.Mvvm.ComponentModel;
using HW05.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HW05;

public partial class MainViewModel : ObservableObject
{
    private readonly Main model;

    [ObservableProperty]
    private string text = "0";

    public MainViewModel(Main model)
    {
        this.model = model;
    }
}
