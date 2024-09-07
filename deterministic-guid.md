# Deterministic Guid
The `Guid5` class provides a method to generate deterministic GUIDs (UUID v5) based on the input arguments.
This means that the same set of input arguments will always produce the same GUID, which can be useful for scenarios
where consistent identifiers are needed across different systems or runs.

## Example Usage
```csharp
using Raccoon.Ninja.Tools.Uuid;
using System;

class Program
{
    static void Main()
    {
        // Example with one argument
        Guid guid1 = Guid5.NewGuid("example");
        Console.WriteLine(guid1);

        // Example with multiple arguments
        Guid guid2 = Guid5.NewGuid("example", 123, true);
        Console.WriteLine(guid2);

        // Example with different types of arguments
        Guid guid3 = Guid5.NewGuid("example", DateTime.Now);
        Console.WriteLine(guid3);
    }
}
```

## Performance
Based on the benchmark results, generating a deterministic GUID using the `Guid5` class is significantly slower than
creating a regular GUID (UUID v4). Here are some key points:

- Creating a regular GUID (`RegularGuid`) takes approximately 69.41 nanoseconds.
- Generating a GUID with one argument (`Guid5WithOneArg`) takes approximately 558 nanoseconds, which is about
  6 times slower than creating a regular GUID.
- The performance decreases further as more arguments are added or when longer strings are used.
  For example, generating a GUID with four arguments including a string of 10,000 chars
  (`Guid5WithFourArgsMixedIncLongString`).

While the `Guid5` class provides the benefit of deterministic GUIDs, it comes with a performance cost compared to
generating regular GUIDs.

Keep in mind that we're talking about nanoseconds here, so the performance difference may not be significant, depending
on your application, and you have the benefit of deterministic GUIDs.

> You can execute the benchmarks yourself by running the `Guid5Benchmarks` class in the
> `Raccoon.Ninja.Tools.Benchmark.Tests` project.

BenchmarkDotNet v0.14.0, Windows 10 (10.0.19045.4780/22H2/2022Update)
Intel Core i7-9750H CPU 2.60GHz, 1 CPU, 12 logical and 6 physical cores
.NET SDK 8.0.401
[Host]     : .NET 8.0.8 (8.0.824.36612), X64 RyuJIT AVX2
DefaultJob : .NET 8.0.8 (8.0.824.36612), X64 RyuJIT AVX2

| Method                                    |        Mean |      Error |    StdDev |      Median | Ratio | RatioSD | Rank |   Gen0 | Allocated | Alloc Ratio |
|-------------------------------------------|------------:|-----------:|----------:|------------:|------:|--------:|-----:|-------:|----------:|------------:|  
| RegularGuid                               |    88.35 ns |   3.445 ns |  10.16 ns |    88.18 ns |  1.01 |    0.17 |    1 |      - |         - |          NA |  
| Guid5WithOneArg                           |   558.56 ns |  14.021 ns |  41.34 ns |   548.36 ns |  6.41 |    0.88 |    2 | 0.0353 |     224 B |          NA |  
| Guid5WithTwoArgsMixed                     |   612.78 ns |  15.601 ns |  46.00 ns |   625.46 ns |  7.03 |    0.97 |    2 | 0.0477 |     304 B |          NA |  
| Guid5WithTwoArgsOnlyStrings               |   598.57 ns |  18.596 ns |  54.83 ns |   593.49 ns |  6.87 |    1.01 |    2 | 0.0477 |     304 B |          NA |  
| Guid5WithThreeArgsMixed                   |   779.63 ns |  19.215 ns |  56.66 ns |   756.28 ns |  8.94 |    1.22 |    4 | 0.0629 |     400 B |          NA |  
| Guid5WithThreeArgsOnlyStrings             |   665.40 ns |  17.160 ns |  50.60 ns |   673.00 ns |  7.63 |    1.06 |    3 | 0.0534 |     336 B |          NA |
| Guid5WithFourArgsMixedIncLongString       | 6,577.32 ns | 198.268 ns | 584.60 ns | 6,485.69 ns | 75.44 |   11.02 |    6 | 0.5417 |    3408 B |          NA |  
| Guid5WithFourArgsOnlyStringsIncLongString | 6,308.70 ns | 167.784 ns | 494.71 ns | 6,168.23 ns | 72.36 |   10.12 |    6 | 0.5341 |    3352 B |          NA |  
| Guid5WithLongString                       | 5,741.92 ns | 168.282 ns | 496.18 ns | 5,824.81 ns | 65.85 |    9.52 |    5 | 0.1907 |    1208 B |          NA |  
