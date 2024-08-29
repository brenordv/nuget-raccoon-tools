using BenchmarkDotNet.Running;
using Raccoon.Ninja.Tools.Benchmark.Tests.Benchmarks;

const int benchmarkToRun = 1;


switch (benchmarkToRun)
{
    case 1:
        BenchmarkRunner.Run<Guid5Benchmarks>();
        break;
    case 2:
        BenchmarkRunner.Run<ClassInstantiationBenchmark>();
        break;
    default:
        Console.WriteLine("No benchmark to run.");
        break;
}
