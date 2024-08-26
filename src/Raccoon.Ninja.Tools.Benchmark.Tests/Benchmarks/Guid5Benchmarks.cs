using System.Diagnostics.CodeAnalysis;
using BenchmarkDotNet.Attributes;
using Raccoon.Ninja.Tools.Uuid;

namespace Raccoon.Ninja.Tools.Benchmark.Tests.Benchmarks;

[MemoryDiagnoser, RankColumn]
[ExcludeFromCodeCoverage]
public class Guid5Benchmarks
{
    private const string Arg1 = "benchmark";
    private const int Arg2 = 42;
    private const double Arg3 = 3.14159;
    private static readonly string Arg4 = new ('a', 1000);
    
    [Benchmark(Baseline = true)]
    public Guid RegularGuid()
    {
        return Guid.NewGuid();
    }

    [Benchmark]
    public Guid Guid5WithOneArg()
    {
        return Guid5.NewGuid(Arg1);
    }

    [Benchmark]
    public Guid Guid5WithTwoArgsMixed()
    {
        return Guid5.NewGuid(Arg1, Arg2);
    }

    [Benchmark]
    public Guid Guid5WithTwoArgsOnlyStrings()
    {
        return Guid5.NewGuid(Arg1, Arg1);
    }

    [Benchmark]
    public Guid Guid5WithThreeArgsMixed()
    {
        return Guid5.NewGuid(Arg1, Arg2, Arg3);
    }

    [Benchmark]
    public Guid Guid5WithThreeArgsOnlyStrings()
    {
        return Guid5.NewGuid(Arg1, Arg1, Arg1);
    }
    
    [Benchmark]
    public Guid Guid5WithFourArgsMixedIncLongString()
    {
        return Guid5.NewGuid(Arg1, Arg2, Arg3, Arg4);
    }
    
    [Benchmark]
    public Guid Guid5WithFourArgsOnlyStringsIncLongString()
    {
        return Guid5.NewGuid(Arg1, Arg1, Arg1, Arg4);
    }
    
    [Benchmark]
    public Guid Guid5WithLongString()
    {
        return Guid5.NewGuid(Arg4);
    }
}