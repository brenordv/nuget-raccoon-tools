using Raccoon.Ninja.Tools.OperationResult.Exceptions;
using Raccoon.Ninja.Tools.OperationResult.ResultError;
using Raccoon.Ninja.Tools.OperationResult.ResultError.Interfaces;

namespace Raccoon.Ninja.Tools.OperationResult;

/// <summary>
/// Represents the result of an operation, which can either be a success with a payload or a failure with an error.
/// </summary>
/// <typeparam name="TPayload">The type of the payload expected in case of success.</typeparam>
public class Result<TPayload>
{
    /// <summary>
    /// Internal control to keep track if <see cref="_value"/> was already set.
    /// </summary>
    private readonly bool _valueSet; 
    
    /// <summary>
    /// Resulting payload from the operation.
    /// </summary>
    private readonly TPayload _value;

    /// <summary>
    /// Contains the error that happened during the operation.
    /// </summary>
    private readonly IError _error;

    /// <summary>
    /// Constructor to be used in success cases.
    /// </summary>
    /// <param name="value">Resulting payload.</param>
    public Result(TPayload value)
    {
        _value = value;
        _valueSet = true;
    }

    /// <summary>
    /// Constructor to be used in case of failures.
    /// </summary>
    /// <param name="error">The error object.</param>
    public Result(IError error)
    {
        _error = error;
    }

    /// <summary>
    /// Implicit conversion from success.
    /// </summary>
    /// <param name="value">Resulting payload value</param>
    /// <returns>Converted Result</returns>
    public static implicit operator Result<TPayload>(TPayload value) => new (value);
    
    /// <summary>
    /// Implicit conversion from failure (using an Error object).
    /// </summary>
    /// <param name="error"></param>
    /// <returns>Converted Result</returns>
    public static implicit operator Result<TPayload>(Error error) => new (error);

    /// <summary>
    /// Implicit conversion from failure (using an Exception object).
    /// </summary>
    /// <param name="exception"></param>
    /// <returns>Converted Result</returns>
    public static implicit operator Result<TPayload>(Exception exception) => new (new Error(null, exception));

    /// <summary>
    /// Maps the result to the appropriate action based on whether it is a success or a failure.
    /// </summary>
    /// <example>
    /// <code>
    /// var result = DoSomething();
    ///
    /// result.Map(
    ///     success => Console.WriteLine($"Success! Here's the payload: {success}"),
    ///     error => Console.WriteLine($"Error. Here's the error message: {error.ErrorMessage}")
    /// );
    /// </code>
    /// </example>
    /// <param name="success">The action to execute if the result is a success.</param>
    /// <param name="failure">The action to execute if the result is a failure.</param>
    /// <exception cref="InvalidResultMapException">
    /// Thrown if both the error and the failure action are null, or if the error is
    /// null and the success action is null.
    /// </exception>
    public void Map(Action<TPayload> success, Action<IError> failure)
    {
        if (_error is null && success is not null && _valueSet)
        {
            success(_value);
            return;
        }

        if (_error is null || failure is null) 
            throw new InvalidResultMapException();

        failure(_error);
    }

    /// <summary>
    /// Processes the result and returns a value based on whether it is a success or a failure.
    /// </summary>
    /// <example>
    /// <code>
    /// var result = GetMovies();
    /// var movies = result.Process(
    ///     resultList => resultList,
    ///     error => {
    ///       _logger.LogError(error.ErrorMessage, error.Exception);
    ///       return []; 
    ///     }
    /// );
    /// </code>
    /// </example>
    /// <param name="onSuccess">The function to execute if the result is a success.</param>
    /// <param name="onFailure">The function to execute if the result is a failure.</param>
    /// <typeparam name="TProcessedPayload">The type of the processed payload to return.</typeparam>
    /// <returns>The processed payload based on the result.</returns>
    /// <exception cref="InvalidResultMapException">
    /// Thrown if both the error and the onFailure function are null, or if the error is null and the
    /// onSuccess function is null.
    /// </exception>
    public TProcessedPayload Process<TProcessedPayload>(
        Func<TPayload, TProcessedPayload> onSuccess,
        Func<IError, TProcessedPayload> onFailure)
    {
        if (_error is null && onSuccess is not null && _valueSet)
            return onSuccess(_value);

        if (_error is null || onFailure is null) 
            throw new InvalidResultMapException();

        return onFailure(_error);
    }


    /// <summary>
    /// Returns a string representation of the result, indicating whether it is a success or a failure.
    /// </summary>
    /// <example>
    /// var successResult = new Result&lt;string&gt;("Success payload");
    /// Console.WriteLine(successResult.ToString()); // Output: [Result:Success] Success payload
    ///
    /// var error = new Error("Error message");
    /// var failureResult = new Result&lt;string&gt;(error);
    /// Console.WriteLine(failureResult.ToString()); // Output: [Result:Failure] Error message
    /// </example>
    /// <returns>A string that represents the current object.</returns>
    public override string ToString()
    {
        return _error is null 
            ? $"[Result:Success] {_value}" 
            : $"[Result:Failure] {_error}";
    }
}
