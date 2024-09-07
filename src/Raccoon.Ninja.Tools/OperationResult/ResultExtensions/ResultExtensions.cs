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
}