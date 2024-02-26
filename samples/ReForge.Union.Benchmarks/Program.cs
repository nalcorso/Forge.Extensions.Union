// Test app to check performance characteristics of various implementation choices.
// Note: ReForge.Union is not designed to be an overly performant library, however it is necessary to understand the
// the performance characteristics of the library to make informed decisions about its implementation.

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using ReForge.Union.Benchmarks;

BenchmarkRunner.Run<Benchmarks>();

namespace ReForge.Union.Benchmarks
{

    [MemoryDiagnoser]
    public class Benchmarks
    {
        private const int Iterations = 1000000;

        [Benchmark]
        public double EnumerationBased()
        {
            var shape = new ReForge.Union.Benchmarks.EnumerationBased.Shape
            {
                Type = ReForge.Union.Benchmarks.EnumerationBased.Shape.ShapeType.Circle,
                Radius = 10
            };
            double area = 0;
            for (int i = 0; i < Iterations; i++)
            {
                area = shape.Area;
            }

            return area;
        }

        [Benchmark]
        public double InheritanceBased()
        {
            var shape = new ReForge.Union.Benchmarks.InheritanceBased.Circle
            {
                Radius = 10
            };
            double area = 0;
            for (int i = 0; i < Iterations; i++)
            {
                area = shape.Area;
            }

            return area;
        }

        [Benchmark]
        public double InterfaceBased()
        {
            var shape = new ReForge.Union.Benchmarks.InterfaceBased.Circle(10);
            double area = 0;
            for (int i = 0; i < Iterations; i++)
            {
                area = shape.Area;
            }

            return area;
        }
    }

}