namespace ReForge.Union.Benchmarks.InheritanceBased;

public abstract class Shape
{
    public abstract double Area { get; }
}

public class Circle : Shape
{
    public double Radius { get; init; }
    public override double Area => Math.PI * Radius * Radius;
}
    
public class Rectangle : Shape
{
    public double Width { get; init; }
    public double Height { get; init; }
    public override double Area => Width * Height;
}
    
public class Triangle : Shape
{
    public double Side1 { get; init; }
    public double Side2 { get; init; }
    public double Side3 { get; init; }
    public override double Area
    {
        get
        {
            var s = (Side1 + Side2 + Side3) / 2;
            return Math.Sqrt(s * (s - Side1) * (s - Side2) * (s - Side3));
        }
    }
}