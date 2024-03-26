namespace ReForge.Union.Benchmarks.InterfaceBased;

public interface IShape
{
    double Area { get; }
}

public record Circle(double Radius) : IShape
{
    public double Area => Math.PI * Radius * Radius;
}

public record Rectangle(double Width, double Height) : IShape
{
    public double Area => Width * Height;
}

public record Triangle(double Side1, double Side2, double Side3) : IShape
{
    public double Area
    {
        get
        {
            var s = (Side1 + Side2 + Side3) / 2;
            return Math.Sqrt(s * (s - Side1) * (s - Side2) * (s - Side3));
        }
    }
}