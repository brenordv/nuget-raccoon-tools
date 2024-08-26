# Racoon Ninja Tools
https://raccoon.ninja

## Description
This is a collection of helpers and tools I find useful enough to reuse in multiple projects.
I hope this can help other people too. :) 


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
- Generating a GUID with one argument (`Guid5WithOneArg`) takes approximately 304.11 nanoseconds, which is about 
4.38 times slower than creating a regular GUID.
- The performance decreases further as more arguments are added or when longer strings are used. 
For example, generating a GUID with four arguments including a string of 10,000 chars 
(`Guid5WithFourArgsMixedIncLongString`) takes approximately 3,127.68 nanoseconds, which is about 45.08 times slower 
- than creating a regular GUID.

In summary, while the `Guid5` class provides the benefit of deterministic GUIDs, it comes with a performance cost 
compared to generating regular GUIDs.

Keep in mind that we're talking about nanoseconds here, so the performance difference may not be significant, depending
on your application and you have the benefit of deterministic GUIDs.

> You can execute the benchmarks yourself by running the `Guid5Benchmarks` class in the 
> `Raccoon.Ninja.Tools.Benchmark.Tests` project.

```
BenchmarkDotNet v0.14.0, Windows 10 (10.0.19045.4780/22H2/2022Update)
Intel Core i7-9750H CPU 2.60GHz, 1 CPU, 12 logical and 6 physical cores
.NET SDK 8.0.401
  [Host]     : .NET 8.0.8 (8.0.824.36612), X64 RyuJIT AVX2
  DefaultJob : .NET 8.0.8 (8.0.824.36612), X64 RyuJIT AVX2
```
| Method                                    |        Mean |     Error |     StdDev |      Median | Ratio | RatioSD | Rank |   Gen0 | Allocated | Alloc Ratio |
|-------------------------------------------|------------:|----------:|-----------:|------------:|------:|--------:|-----:|-------:|----------:|------------:|
| RegularGuid                               |    69.41 ns |  1.324 ns |   1.301 ns |    69.56 ns |  1.00 |    0.03 |    1 |      - |         - |          NA |
| Guid5WithOneArg                           |   304.11 ns |  7.023 ns |  20.707 ns |   294.37 ns |  4.38 |    0.31 |    2 | 0.0238 |     152 B |          NA |
| Guid5WithTwoArgsMixed                     |   350.74 ns | 10.822 ns |  31.396 ns |   341.18 ns |  5.06 |    0.46 |    3 | 0.0367 |     232 B |          NA |
| Guid5WithTwoArgsOnlyStrings               |   358.71 ns |  8.304 ns |  24.486 ns |   352.34 ns |  5.17 |    0.36 |    3 | 0.0367 |     232 B |          NA |
| Guid5WithThreeArgsMixed                   |   551.36 ns | 11.916 ns |  34.948 ns |   552.61 ns |  7.95 |    0.52 |    4 | 0.0520 |     328 B |          NA |
| Guid5WithThreeArgsOnlyStrings             |   399.11 ns |  9.538 ns |  28.122 ns |   399.87 ns |  5.75 |    0.42 |    3 | 0.0420 |     264 B |          NA |
| Guid5WithFourArgsMixedIncLongString       | 3,127.68 ns | 89.626 ns | 262.858 ns | 3,069.89 ns | 45.08 |    3.86 |    7 | 0.5302 |    3336 B |          NA |
| Guid5WithFourArgsOnlyStringsIncLongString | 3,017.14 ns | 98.676 ns | 290.950 ns | 3,030.88 ns | 43.48 |    4.25 |    7 | 0.5226 |    3280 B |          NA |
| Guid5WithLongString                       | 2,500.92 ns | 73.156 ns | 212.240 ns | 2,496.38 ns | 36.04 |    3.12 |    6 | 0.1793 |    1136 B |          NA |


## List Extensions
The `ListExtensions` class provides several useful extension methods for working with lists and other enumerable collections.

### Methods
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
Ok, I know this type of thing should probably be a `readonly struct` to have better performance and avoid too many
objects allocated in memory. However, I wanted the possibility of creating inherited classes without breaking existing
code.

Below are the results of the benchmark tests for instantiating Classes, Struct, Readonly Struct, and Records.
I'm aware that they are not the best, but it's good enough for a brief comparison.

| Method            |      Mean |     Error |    StdDev |    Median | Rank |   Gen0 | Allocated |
|-------------------|----------:|----------:|----------:|----------:|-----:|-------:|----------:|
| NewClass          | 5.7458 ns | 0.2447 ns | 0.7216 ns | 5.6876 ns |    2 | 0.0051 |      32 B |
| NewStruct         | 0.0688 ns | 0.0320 ns | 0.0938 ns | 0.0008 ns |    1 |      - |         - |
| NewReadonlyStruct | 0.0795 ns | 0.0341 ns | 0.0990 ns | 0.0370 ns |    1 |      - |         - |
| NewRecord         | 5.6249 ns | 0.2198 ns | 0.6482 ns | 5.5565 ns |    2 | 0.0051 |      32 B |

To no one's surprise, instantiating a `struct` or `readonly struct` is by far the fastest option and won't allocate any
memory. However, we're talking about nanoseconds here, so the difference may not be significant in most cases.

TBH, I'm bothered by the performance difference, but I think the flexibility is worth it. Maybe someday in the future I
can change that, but for now, I'll keep it as it is.

## Validation Result
This works exactly like the `Result<TPayload>` class, but it's more focused on validation scenarios, meaning that you 
are checking if something is valid or not, and are not interested in any result object.

It is basically a syntactic sugar for the `Result<TPayload>` class, where the payload is always `bool`.
