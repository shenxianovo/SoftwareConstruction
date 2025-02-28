using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace HW02.Models
{
    class ShapeFactory
    {
        private static Random random = new Random();

        // Get all types derived from Shape
        private static List<Type> shapeTypes = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(t => t.IsSubclassOf(typeof(Shape)) && !t.IsAbstract)
            .ToList();

        // Instantiate random shape
        public static Shape CreateRandomShape()
        {
            Type shapeType = shapeTypes[random.Next(shapeTypes.Count)];

            ConstructorInfo? constructor = shapeType.GetConstructors()
                .FirstOrDefault(c => c.GetParameters().Length == 2
                                    || c.GetParameters().Length == 1 ); // Constructor that has 1 or 2 args

            if (constructor == null) throw new InvalidOperationException("No valid constructor found.");

            double width = random.NextDouble() * 10 + 1;

            if (constructor.GetParameters().Length == 1)
                return (Shape)constructor.Invoke(new object[] { width });

            double height = random.NextDouble() * 10 + 1;

            return (Shape)constructor.Invoke(new object[] { width, height });
        }
    }
}
