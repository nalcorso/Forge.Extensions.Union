using OneOf;

namespace ReForge.Union.Benchmarks.OneOfBased;

public record Circle(double Radius);
public record Rectangle(double Width, double Height);
public record Triangle(double Side1, double Side2, double Side3);

[GenerateOneOf]
public partial class Shape : OneOfBase<Circle, Rectangle, Triangle> { }