# Operation Result
The `Result<TPayload>` class represents the result of an operation, which can either be a success with a payload or a
failure with an error. This class provides methods to map and process the result based on its success or failure state.

It's a somewhat functional approach to handling operation results, not fully adhering to the functional programming
paradigm but providing some of its benefits.

## Methods
### Result<T>
- `Tap`: Taps into the result instance and executes the appropriate action based on whether it is a success or a failure.
- `TapAsync`: Taps into the result instance and executes the appropriate asynchronous action based on whether it is a success or a failure.
- `Process`: Processes the result and returns a value based on whether it is a success or a failure.
- `ProcessAsync`: Processes the result asynchronously and returns a value based on whether it is a success or a failure.
- `ForwardError`: Forwards the error from the current result to a new result with a different payload type.
- `[Result<bool> Extension] TapOnSuccess`: Executes the provided action if the result is a success and the payload is true.
- `[Result<bool> Extension] TapOnSuccessAsync`: Executes the provided asynchronous action if the result is a success and the payload is true.
- `[Result<bool> Extension] ChainOnSuccess`: Chains the provided function if the result is a success and the payload is true.
- `[Result<bool> Extension] ChainOnSuccessAsync`: Chains the provided asynchronous function if the result is a success and the payload is true.

### Error
- `WithExcception`: Creates a new error instance with the provided exception. This allows you to re-use an error object, by adding a new piece of information to it (the exception)

## Example Usage
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

## Performance
### Instantiation
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

### Usage
For the usage benchmark test, I create the following scenarios:
1. `ThrowExceptionOnError`: When an error occurs, an exception is thrown and captured by the caller;
2. `ReturnResultOnError`: When an error occurs, the result is returned to the caller (instead of throwing an exception);
3. `ReturnNullOnExceptionCaught`: When an error occurs, the result is returned as `null` to the caller;
4. `ReturnImplicitResultOnExceptionCaught`: When an error occurs, the exception is implicitly converted to the Result type and returned to the caller;
5. `ReturnExplicitResultOnExceptionCaught`: When an error occurs, the exception is explicitly converted to the Result type and returned to the caller;
6. `ReturnSuccessList`: When the operation is successful, a list of string is returned to the caller;
7. `ReturnSuccessResultList`: When the operation is successful, a list of string is returned as a Result type to the caller. The converstion is done implicitly.

```text
BenchmarkDotNet v0.14.0, Windows 10 (10.0.19045.4780/22H2/2022Update)
Intel Core i7-9750H CPU 2.60GHz, 1 CPU, 12 logical and 6 physical cores
.NET SDK 8.0.401
[Host]     : .NET 8.0.8 (8.0.824.36612), X64 RyuJIT AVX2
DefaultJob : .NET 8.0.8 (8.0.824.36612), X64 RyuJIT AVX2
```

| Method                                |         Mean |       Error |      StdDev |       Median | Ratio | RatioSD | Rank |   Gen0 | Allocated | Alloc Ratio | 
|---------------------------------------|-------------:|------------:|------------:|-------------:|------:|--------:|-----:|-------:|----------:|------------:| 
| ThrowExceptionOnError                 | 8,339.272 ns | 211.8029 ns | 614.4788 ns | 8,358.530 ns | 1.005 |    0.11 |    4 | 0.0458 |     344 B |        1.00 |
| ReturnResultOnError                   |     8.545 ns |   0.3371 ns |   0.9834 ns |     8.411 ns | 0.001 |    0.00 |    1 | 0.0051 |      32 B |        0.09 | 
| ReturnNullOnExceptionCaught           | 6,147.746 ns | 121.6040 ns | 318.2166 ns | 6,139.373 ns | 0.741 |    0.07 |    3 | 0.0305 |     224 B |        0.65 | 
| ReturnImplicitResultOnExceptionCaught | 6,095.540 ns | 159.0650 ns | 461.4763 ns | 5,964.967 ns | 0.735 |    0.08 |    3 | 0.0381 |     256 B |        0.74 | 
| ReturnExplicitResultOnExceptionCaught | 6,280.372 ns | 124.5711 ns | 332.5055 ns | 6,357.890 ns | 0.757 |    0.07 |    3 | 0.0381 |     256 B |        0.74 | 
| ReturnSuccessList                     |    22.468 ns |   0.5101 ns |   0.6451 ns |    22.420 ns | 0.003 |    0.00 |    2 | 0.0140 |      88 B |        0.26 | 
| ReturnSuccessResultList               |    23.845 ns |   0.5110 ns |   1.4074 ns |    23.536 ns | 0.003 |    0.00 |    2 | 0.0140 |      88 B |        0.26 | 

In general terms, using the Operation Result to handle the results, in case of errors, is way faster than throwing an
exception and uses less memory (because of the overhead involved in the exception handling). So that's a great idea when
you don't need details from an exception and just want to communicate an error.

When capturing an unexpected exception, and returning a result (instead of null, empty list or something like that),
from the benchmark, it seems like there's no significant difference between the approaches.

For cases of success, the overhead is minimal (about 1.3ns), which isn't really significant, and with no extra memory
allocation.

From this, seems like the Operation Result is a good choice for handling errors and returning results in a standardized
way.
