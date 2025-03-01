using Microsoft.UI.Xaml.Media.Animation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.System.Profile;

namespace HW02.Models
{
    abstract class Shape : IAreaCalculator, IShapeValidator
    {
        protected Shape(double width, double height)
        {
            Width = width;
            Height = height;
        }

        public double Width { get; set; }
        public double Height { get; set; }

        public double VisualWidth => Width * 10;
        public double VisualHeight => Height * 10;

        public double CanvasTop => (200 - VisualHeight) / 2;
        public double CanvasLeft => (200 - VisualWidth) / 2;

        public string ShapeType => GetType().Name;
        public double Area => CalcArea();

        public abstract double CalcArea();
        public bool IsValid()
        {
            // Sanity check
            if (Width > 0 && Height > 0)
                return false;

            if (this is Square || this is Circle)
            {
                return Width == Height;
            }
            return true;
        }
    }

    internal class Rectangle : Shape
    {
        public Rectangle(double width, double height) : base(width, height) { }

        public override double CalcArea() => Width * Height;
    }

    internal class Ellipse : Shape
    {
        public Ellipse(double width, double height) : base(width, height) { }

        public override double CalcArea() => double.Pi * Width * Height;
    }

    internal class Square : Shape
    {
        public Square(double side) : base(side, side) { }

        public override double CalcArea() => Width * Width;
    }

    internal class Circle : Shape
    {
        public Circle(double radius) : base(radius, radius) { }

        public override double CalcArea() => double.Pi * Math.Pow(Width, 2);
    }
}
