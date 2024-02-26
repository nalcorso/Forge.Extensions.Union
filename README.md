# ReForge.Union

> Warning: This project is in the early stages of development and should be used with caution.

ReForge.Union is a source generator for C# that provides an easy way to implement basic Discriminated Unions in your code.

## Getting Started

To use ReForge.Union, you need to install the NuGet package. You can do this through the NuGet package manager, or by using the dotnet CLI:

```shell
dotnet add package ReForge.Union
```

## Usage

To create a Discriminated Union, you need to define an abstract record and mark it with the `[Union]` attribute. Each variant of the union is a nested record marked with the `[Variant]` attribute.

> Note: The `partial` keyword is required for the union and its variants.

> Note: The variants must be of the same type (record, class, or struct) as the union.

```csharp
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
```

The source generator will automatically generate the following methods for the union:

- `Is<T>()`: Checks if the union is of a specific variant.
- `As<T>()`: Casts the union to a specific variant.
- `TryAs<T>(out T? value)`: Tries to cast the union to a specific variant.
- `Match<T>(Func<Circle, T>? circleFunc = null, Func<Rectangle, T>? rectangleFunc = null, Func<Triangle, T>? triangleFunc = null)`: Matches the union to a specific function based on its variant.

You can also define custom methods for the union and its variants by using partial classes.

```csharp
public partial record Shape
{
    public double Area => this.Match(
        circleFunc: circle => Math.PI * circle.Radius * circle.Radius,
        rectangleFunc: rectangle => rectangle.Width * rectangle.Height,
        triangleFunc: triangle =>
        {
            var s = (triangle.Side1 + triangle.Side2 + triangle.Side3) / 2;
            return Math.Sqrt(s * (s - triangle.Side1) * (s - triangle.Side2) * (s - triangle.Side3));
        });
}
```

## Contributing

Thank you for considering contributing to ReForge.Union! Whether you're reporting a bug, discussing a feature request, or contributing code, we appreciate your effort.

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.