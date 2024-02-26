// See https://aka.ms/new-console-template for more information

using DevScratch;
using ReForge.Union;

Console.WriteLine("ReForge Discriminated Union Example");

Shape shape = new Shape.Circle(5.0);
Console.WriteLine(shape);

var area = shape.Match(
    c => Math.PI * c.Radius * c.Radius,
    r => r.Width * r.Height
);
Console.WriteLine($"Area: {area}");

// Use the Is<T> method to check if the shape is a Circle
Console.WriteLine(shape.Is<Shape.Circle>() ? "The shape is a Circle." : "The shape is not a Circle.");

// Use the As<T> method to cast the shape to a Circle
var circle = shape.As<Shape.Circle>();
Console.WriteLine(circle is not null ? $"The radius of the circle is {circle.Radius}." : "The shape is not a Circle.");

// Use the TryAs<T> method to cast the shape to a Circle
Console.WriteLine(shape.TryAs(out Shape.Rectangle? rect)
    ? $"This is a Rectangle with width {rect.Width} and height {rect.Height}."
    : "The shape is not a Rectangle.");


shape = new Shape.Rectangle(5.0, 10.0);
Console.WriteLine(shape);

area = shape.Match(
    c => Math.PI * c.Radius * c.Radius,
    r => r.Width * r.Height
);

Console.WriteLine($"Area: {area}");

// Use the ReForge.Union Source Generator to create a Discriminated Union from the following code
namespace DevScratch
{
    [Union]
    internal abstract partial record Shape
    {
        [Variant]
        internal partial record Circle(double Radius);

        [Variant]
        public partial record Rectangle(double Width, double Height);

        [Variant]
        public partial record Triangle(double Side1, double Side2, double Side3);
    }
}