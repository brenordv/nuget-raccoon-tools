using Raccoon.Ninja.Tools.OperationResult.ResultError;

namespace Raccoon.Ninja.Tools.OperationResult.ResultExtensions;

public static class ResultExtensions
{
    /// <summary>
    /// Executes the provided action if the result is a success and the payload is true.
    /// </summary>
    /// <remarks>This serves as a convenience method for cases where the Result&lt;bool&gt; represents the result of
    /// a validation/check method. A way to reduce the boilerplate code in cases where you do something in case of
    /// success or return an error.</remarks>
    /// <example>
    /// <code>
    /// var result = new Result&lt;bool&gt;(true);
    /// var isSuccess = result.TapOnSuccess(() => Console.WriteLine("Success!"));
    /// Console.WriteLine(isSuccess); // Output: True
    /// </code>
    /// </example>
    /// <param name="result">The result to check.</param>
    /// <param name="actionOnSuccess">The action to execute if the result is a success and the payload is true.</param>
    /// <returns>True if the result is a success and the payload is true, otherwise false.</returns>
    public static bool TapOnSuccess(this Result<bool> result, Action actionOnSuccess)
    {
        var isSuccess = false;

        result.Tap(resultSuccess =>
        {
            isSuccess = resultSuccess;
            if (!isSuccess)
                return;
            actionOnSuccess();
        }, error => { });

        return isSuccess;
    }
    
    /// <summary>
    /// Executes the provided asynchronous action if the result is a success and the payload is true.
    /// </summary>
    /// <remarks>This serves as a convenience method for cases where the Result&lt;bool&gt; represents the result of
    /// a validation/check method. A way to reduce the boilerplate code in cases where you do something in case of
    /// success or return an error.</remarks>
    /// <example>
    /// <code>
    /// var result = new Result&lt;bool&gt;(true);
    /// var isSuccess = await result.TapOnSuccessAsync(async () => await Console.Out.WriteLineAsync("Success!"));
    /// Console.WriteLine(isSuccess); // Output: True
    /// </code>
    /// </example>
    /// <param name="result">The result to check.</param>
    /// <param name="actionOnSuccess">The asynchronous action to execute if the result is a success and the payload is true.</param>
    /// <returns>A task representing the asynchronous operation, containing true if the result is a success and the payload is true, otherwise false.</returns>
    public static async Task<bool> TapOnSuccessAsync(this Result<bool> result, Func<Task> actionOnSuccess)
    {
        var isSuccess = false;

        await result.TapAsync(async resultSuccess =>
        {
            isSuccess = resultSuccess;
            if (!isSuccess)
                return;
            await actionOnSuccess();
        }, error => Task.CompletedTask);

        return isSuccess;
    }
    
    /// <summary>
    /// Chains the execution of an action if the result is a success and the payload is true.
    /// Having the payload as true is considered as if the previous check/validation was successful.
    /// </summary>
    /// <remarks>
    /// This method is useful for chaining operations that depend on the success of a previous operation.
    /// It forwards the error from the original result if the result is a failure.
    /// </remarks>
    /// <typeparam name="TChainedPayload">The type of the payload for the chained result.</typeparam>
    /// <param name="result">The result to check.</param>
    /// <param name="actionOnSuccess">The action to execute if the result is a success and the payload is true.</param>
    /// <returns>
    /// A new result containing the payload from the chained action if the original result is a success and the payload is true.
    /// If the original result is a failure, it forwards the original error.
    /// </returns>
    /// <example>
    /// <code>
    /// // Example for a success case
    /// var initialResult = new Result&lt;bool&gt;(true);
    /// var chainedResult = initialResult.ChainOnSuccess(() => new Result&lt;string&gt;("Chained Success"));
    /// Console.WriteLine(chainedResult); // Output: [Result:Success] Chained Success
    ///
    /// // Example for a failure case
    /// var failureResult = new Result&lt;bool&gt;(new Error("Initial error"));
    /// var chainedFailureResult = failureResult.ChainOnSuccess(() => new Result&lt;string&gt;("This won't be executed"));
    /// Console.WriteLine(chainedFailureResult); // Output: [Result:Failure] Initial error
    /// </code>
    /// </example>
    public static Result<TChainedPayload> ChainOnSuccess<TChainedPayload>(this Result<bool> result, Func<Result<TChainedPayload>> actionOnSuccess)
    {
        return result.Process(resultSuccess => resultSuccess 
            // If the result is a success, execute the action.
            ? actionOnSuccess() 
            // If the result is not a success, we return an error so the caller knows what is going on.
            : new Result<TChainedPayload>(ErrorPresets.NotAbleToChainOnSuccess), 

            // If the original result is an error, return the original error.
            error => new Result<TChainedPayload>(error)); 
    }

    /// <summary>
    /// Chains the execution of an asynchronous action if the result is a success and the payload is true.
    /// Having the payload as true is considered as if the previous check/validation was successful.
    /// </summary>
    /// <remarks>
    /// This method is useful for chaining operations that depend on the success of a previous operation.
    /// It forwards the error from the original result if the result is a failure.
    /// </remarks>
    /// <typeparam name="TChainedPayload">The type of the payload for the chained result.</typeparam>
    /// <param name="result">The result to check.</param>
    /// <param name="actionOnSuccess">The asynchronous action to execute if the result is a success and the payload is true.</param>
    /// <returns>
    /// A task representing the asynchronous operation, containing a new result with the payload from the chained action if the original result is a success and the payload is true.
    /// If the original result is a failure, it forwards the original error.
    /// </returns>
    /// <example>
    /// <code>
    /// // Example for a success case
    /// var initialResult = new Result&lt;bool&gt;(true);
    /// var chainedResult = await initialResult.ChainOnSuccessAsync(async () => new Result&lt;string&gt;("Chained Success"));
    /// Console.WriteLine(chainedResult); // Output: [Result:Success] Chained Success
    ///
    /// // Example for a failure case
    /// var failureResult = new Result&lt;bool&gt;(new Error("Initial error"));
    /// var chainedFailureResult = await failureResult.ChainOnSuccessAsync(async () => new Result&lt;string&gt;("This won't be executed"));
    /// Console.WriteLine(chainedFailureResult); // Output: [Result:Failure] Initial error
    /// </code>
    /// </example>
    public static async Task<Result<TChainedPayload>> ChainOnSuccessAsync<TChainedPayload>(
        this Result<bool> result, 
        Func<Task<Result<TChainedPayload>>> actionOnSuccess)
    {
        return await result.ProcessAsync(async resultSuccess => resultSuccess 
            // If the result is a success, execute the action.
            ? await actionOnSuccess() 
            // If the result is not a success, we return an error so the caller knows what is going on.
            : new Result<TChainedPayload>(ErrorPresets.NotAbleToChainOnSuccess), 

            // If the original result is an error, return the original error.
            error => Task.FromResult(new Result<TChainedPayload>(error))); 
    }
}
