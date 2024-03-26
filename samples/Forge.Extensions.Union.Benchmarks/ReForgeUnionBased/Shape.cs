namespace ReForge.Union.Benchmarks.ReForgeUnionBased;

[Union]
public abstract partial record Shape
{
    [Variant]
    public partial record Circle(double Radius);

    [Variant]
    public partial record Rectangle(double Width, double Height);

    [Variant]
    public partial record Triangle(double Side1, double Side2, double Side3);
}

public partial record Shape
{
    public double Area => Match(
        circleFunc: circle => Math.PI * circle.Radius * circle.Radius,
        rectangleFunc: rectangle => rectangle.Width * rectangle.Height,
        triangleFunc: triangle =>
        {
            var s = (triangle.Side1 + triangle.Side2 + triangle.Side3) / 2;
            return Math.Sqrt(s * (s - triangle.Side1) * (s - triangle.Side2) * (s - triangle.Side3));
        }
    );
}