namespace ReForge.Union.Benchmarks.EnumerationBased;

public class Shape
{
    public enum ShapeType { Circle, Rectangle, Triangle }
    
    public ShapeType Type { get; init; }
    public double Radius { get; init; }
    public double Width { get; init; }
    public double Height { get; init; }
    public double SideA { get; init; }
    public double SideB { get; init; }
    public double SideC { get; init; }

    public double Area
    {
        get
        {
            return Type switch
            {
                ShapeType.Circle => Math.PI * Radius * Radius,
                ShapeType.Rectangle => Width * Height,
                ShapeType.Triangle => CalculateTriangleArea(),
                _ => throw new NotImplementedException(),
            };
        }
    }
    
    private double CalculateTriangleArea()
    {
        var s = (SideA + SideB + SideC) / 2;
        return Math.Sqrt(s * (s - SideA) * (s - SideB) * (s - SideC));
    }
}