# Racoon Ninja Tools
[![Build](https://github.com/brenordv/nuget-raccoon-tools/actions/workflows/qa-on-pull-requests.yml/badge.svg?branch=master)](https://github.com/brenordv/nuget-raccoon-tools/actions/workflows/qa-on-pull-requests.yml)
[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=brenordv_nuget-raccoon-tools&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=brenordv_nuget-raccoon-tools)
[![Bugs](https://sonarcloud.io/api/project_badges/measure?project=brenordv_nuget-raccoon-tools&metric=bugs)](https://sonarcloud.io/summary/new_code?id=brenordv_nuget-raccoon-tools)
[![Code Smells](https://sonarcloud.io/api/project_badges/measure?project=brenordv_nuget-raccoon-tools&metric=code_smells)](https://sonarcloud.io/summary/new_code?id=brenordv_nuget-raccoon-tools)
[![Coverage](https://sonarcloud.io/api/project_badges/measure?project=brenordv_nuget-raccoon-tools&metric=coverage)](https://sonarcloud.io/summary/new_code?id=brenordv_nuget-raccoon-tools)
[![Duplicated Lines (%)](https://sonarcloud.io/api/project_badges/measure?project=brenordv_nuget-raccoon-tools&metric=duplicated_lines_density)](https://sonarcloud.io/summary/new_code?id=brenordv_nuget-raccoon-tools)
[![Lines of Code](https://sonarcloud.io/api/project_badges/measure?project=brenordv_nuget-raccoon-tools&metric=ncloc)](https://sonarcloud.io/summary/new_code?id=brenordv_nuget-raccoon-tools)
[![Reliability Rating](https://sonarcloud.io/api/project_badges/measure?project=brenordv_nuget-raccoon-tools&metric=reliability_rating)](https://sonarcloud.io/summary/new_code?id=brenordv_nuget-raccoon-tools)
[![Reliability Rating](https://sonarcloud.io/api/project_badges/measure?project=brenordv_nuget-raccoon-tools&metric=reliability_rating)](https://sonarcloud.io/summary/new_code?id=brenordv_nuget-raccoon-tools)
[![Technical Debt](https://sonarcloud.io/api/project_badges/measure?project=brenordv_nuget-raccoon-tools&metric=sqale_index)](https://sonarcloud.io/summary/new_code?id=brenordv_nuget-raccoon-tools)
[![Maintainability Rating](https://sonarcloud.io/api/project_badges/measure?project=brenordv_nuget-raccoon-tools&metric=sqale_rating)](https://sonarcloud.io/summary/new_code?id=brenordv_nuget-raccoon-tools)
[![Vulnerabilities](https://sonarcloud.io/api/project_badges/measure?project=brenordv_nuget-raccoon-tools&metric=vulnerabilities)](https://sonarcloud.io/summary/new_code?id=brenordv_nuget-raccoon-tools)

## Description
This is a collection of helpers and tools I find useful enough to reuse in multiple projects.
I hope this can help other people too. :) 

## Changelog
Check the [changelog](changelog.md) for the latest updates.

# Features
## Deterministic Guid
The `Guid5` class provides a method to generate deterministic GUIDs (UUID v5) based on the input arguments. 
This means that the same set of input arguments will always produce the same GUID, which can be useful for scenarios 
where consistent identifiers are needed across different systems or runs.

### Example Usage
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

### Performance
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


## List Extensions
The `ListExtensions` class provides several useful extension methods for working with lists and other enumerable 
collections.

### Methods
#### `SafeAll<T>`
Determines whether all elements of a sequence satisfy a condition safely.

**Parameters:**
- `source` (IEnumerable\<T\>): An enumerable to test.
- `predicate` (Func\<T, bool\>): A function to test each element for a condition.

**Returns:**
- bool: True if every element of the source sequence passes the test in the specified predicate. If source is empty or 
null, returns false.

**Example Usage:**
```csharp
var numbers = new List<int> { 1, 2, 3, 4, 5 };
bool allEven = numbers.SafeAll(n => n % 2 == 0); // Output: False

var emptyList = new List<int>();
bool allEvenEmpty = emptyList.SafeAll(n => n % 2 == 0); // Output: False

List<int> nullList = null;
bool allEvenNull = nullList.SafeAll(n => n % 2 == 0); // Output: False

var numbers2 = new List<int> { 2, 4, 6, 8 };
bool allEven2 = numbers2.SafeAll(n => n % 2 == 0); // Output: True
```

#### `ForEachWithIndex<T>`
Returns an iterable list containing every item and its index.

**Parameters:**
- `source` (IEnumerable\<T\>): The target enumerable collection.

**Returns:**
- IEnumerable\<(int index, T item)\>: An enumerable containing tuples with the index and item.

**Example Usage:**
```csharp
var list = new List<string> { "a", "b", "c" };
foreach (var (index, item) in list.ForEachWithIndex())
{
    Console.WriteLine($"Index: {index}, Item: {item}");
}
```

#### `ContainsCaseInsensitive`
Checks if the source contains the specified string, ignoring case.

**Parameters:**
- `source` (IEnumerable\<string\>): The source of strings to search.
- `containsText` (string): The string to search for.
- `nullValuesAreErrors` (bool): If true, null values in the source will be treated as errors and will not match the 
search string. If false, null values in the source will be ignored.

**Returns:**
- bool: True if the source contains the specified string (case-insensitive); otherwise, false.

**Example Usage:**
```csharp
var list = new List<string> { "Hello", "world" };
bool containsHello = list.ContainsCaseInsensitive("hello");
Console.WriteLine(containsHello); // Output: True
```

#### `Replace<T>`
Replaces the first occurrence of an object in the source.

**Parameters:**
- `source` (IList\<T\>): The source list.
- `oldObj` (T): The old object to be replaced.
- `newObj` (T): The new object that will replace the old one.

**Returns:**
- bool: True if the object is replaced; false if the object is not found in the source.

**Example Usage:**
```csharp
var list = new List<int> { 1, 2, 3 };
bool replaced = list.Replace(2, 4);
Console.WriteLine(replaced); // Output: True
Console.WriteLine(string.Join(", ", list)); // Output: 1, 4, 3
```

## String Extensions
The `StringExtensions` class provides several useful extension methods for working with strings.

### Methods
#### `Minify`
Minifies a text by replacing spaces, tabs, and line breaks with a single space.

**Parameters:**
- `bigText` (string): The text to be minified.

**Returns:**
- string: The minified text.

**Example Usage:**
```csharp
string text = @"This is a   test.

                New line.";
string minifiedText = text.Minify();
Console.WriteLine(minifiedText); // Output: "This is a test. New line."
```

#### `StripAccents`
Removes all diacritics (accents) from a string.

**Parameters:**
- `text` (string): The text from which to remove diacritics.

**Returns:**
- string: The text without diacritics.

**Example Usage:**
```csharp
string text = "Café";
string strippedText = text.StripAccents();
Console.WriteLine(strippedText); // Output: "Cafe"
```

#### `OnlyDigits`
Removes everything that is not a digit from a string.

**Parameters:**
- `text` (string): The target string.

**Returns:**
- string: A string containing only digits.

**Example Usage:**
```csharp
string text = "Phone: 123-456-7890";
string digits = text.OnlyDigits();
Console.WriteLine(digits); // Output: "1234567890"
```

## Operation Result
The `Result<TPayload>` class represents the result of an operation, which can either be a success with a payload or a 
failure with an error. This class provides methods to map and process the result based on its success or failure state.

It's a somewhat functional approach to handling operation results, not fully adhering to the functional programming 
paradigm but providing some of its benefits.

### Example Usage
```csharp
using Raccoon.Ninja.Tools.OperationResult;
using Raccoon.Ninja.Tools.OperationResult.ResultError;

class Program
{
    static void Main()
    {
        // Example mapping result.
        var result = DoSomething();
        result.Map(
            success => Console.WriteLine($"Success! Here's the result: {success}"), // Only executed if successful
            error => Console.WriteLine($"Oh no! It failed! Error: {error.ErrorMessage}") // Only executed if failed
        );

        // Example processing result and getting an object back.
        var result2 = DoSomethingElseAndReturnPayload();
        var processedPayload = failureResult.Process(
            success => $"Processed: {success}", // Only executed if successful
            failure => $"Failed: {failure.ErrorMessage}" // Only executed if failed
        );
        Console.WriteLine(processedPayload);
    }
}
```

### Performance
Quick update here. I caved in and changed from `class` to `readonly struct`. I think the performance benefits, thread
safety, and immutability are worth it. Thankfully, this won't affect anyone, because the contracts are the same.

Below are the results of the benchmark tests for instantiating Classes, Struct, Readonly Struct, and Records.
I'm aware that they are not the best, but it's good enough for a brief comparison.

| Method            |      Mean |     Error |    StdDev |    Median | Rank |   Gen0 | Allocated |
|-------------------|----------:|----------:|----------:|----------:|-----:|-------:|----------:|
| NewClass          | 5.7458 ns | 0.2447 ns | 0.7216 ns | 5.6876 ns |    2 | 0.0051 |      32 B |
| NewStruct         | 0.0688 ns | 0.0320 ns | 0.0938 ns | 0.0008 ns |    1 |      - |         - |
| NewReadonlyStruct | 0.0795 ns | 0.0341 ns | 0.0990 ns | 0.0370 ns |    1 |      - |         - |
| NewRecord         | 5.6249 ns | 0.2198 ns | 0.6482 ns | 5.5565 ns |    2 | 0.0051 |      32 B |

To no one's surprise, instantiating a `struct` or `readonly struct` is by far the fastest option and won't allocate any
memory.

## Other
Shamelessly plugging the link to my site: https://raccoon.ninja