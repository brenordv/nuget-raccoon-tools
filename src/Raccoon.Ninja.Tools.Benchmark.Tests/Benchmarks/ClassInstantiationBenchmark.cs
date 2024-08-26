using System.Diagnostics.CodeAnalysis;
using BenchmarkDotNet.Attributes;

namespace Raccoon.Ninja.Tools.Benchmark.Tests.Benchmarks;

[MemoryDiagnoser, RankColumn]
[ExcludeFromCodeCoverage]
public class ClassInstantiationBenchmark
{
    [Benchmark]
    public MockClass NewClass()
    {
        return new MockClass();
    }
    
    [Benchmark]
    public MockStruct NewStruct()
    {
        return new MockStruct();
    }
    
    [Benchmark]
    public MockReadonlyStruct NewReadonlyStruct()
    {
        return new MockReadonlyStruct();
    }
    
    [Benchmark]
    public MockRecord NewRecord()
    {
        return new MockRecord();
    }

    public class MockClass
    {
        public object Value { get; init; }
        public ICollection<Exception> Errors { get; init; }
    }

    public struct MockStruct
    {
        public object Value { get; init; }
        public ICollection<Exception> Errors { get; init; }
    }

    public readonly struct MockReadonlyStruct
    {
        public object Value { get; init; }
        public ICollection<Exception> Errors { get; init; }
    }

    public record MockRecord
    {
        public object Value { get; init; }
        public ICollection<Exception> Errors { get; init; }
    }
}