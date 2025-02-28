using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HW02.Models;
using HW02.Views;

namespace HW02.ViewModels
{
    partial class ShapesViewModel : ObservableObject
    {
        private ShapesPage view;

        [ObservableProperty]
        private ObservableCollection<Shape>? _shapes;

        [ObservableProperty]
        double _areaSum = 0;

        [RelayCommand]
        private void CreateRandomShape()
        {
            Shape shape = ShapeFactory.CreateRandomShape();

            if (Shapes.Count < 10)
            {
                Shapes.Add(shape);
                AreaSum += shape.Area;
            }
            else
            {
                AreaSum -= Shapes[0].Area;
                Shapes.RemoveAt(0);
                Shapes.Add(shape);
                AreaSum += shape.Area;
            }
        }

        [RelayCommand]
        private void CreateRamdomShapes()
        {
            Shapes!.Clear();
            AreaSum = 0;
            for (int i = 0; i < 10; ++i)
            {
                Shape shape = ShapeFactory.CreateRandomShape();
                AreaSum += shape.Area;
                Shapes.Add(shape);
            }
        }

        public ShapesViewModel(ShapesPage view)
        {
            this.view = view;
            Shapes = new ObservableCollection<Shape>();
        }
    }
}
