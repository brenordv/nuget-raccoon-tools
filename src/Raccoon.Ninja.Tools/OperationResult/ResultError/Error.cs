using Raccoon.Ninja.Tools.OperationResult.Exceptions;
using Raccoon.Ninja.Tools.OperationResult.ResultError.Interfaces;

namespace Raccoon.Ninja.Tools.OperationResult.ResultError;

public readonly struct Error : IError
{
    public Exception Exception { get; }
    public string ErrorMessage { get; }

    /// <summary>
    /// This is not allowed. When creating an error, you must provide at least an error message or an exception.
    /// Otherwise, what's the point?
    /// </summary>
    /// <exception cref="OperationResultException"></exception>
    public Error()
    {
        throw new OperationResultException("Default constructor is not allowed.");
    }

    public Error(string errorMessage)
    {
        if (string.IsNullOrWhiteSpace(errorMessage))
        {
            throw new OperationResultException("Error message is null or empty.");
        }

        ErrorMessage = errorMessage;
        Exception = null;
    }

    public Error(Exception exception)
    {
        Exception = exception ?? throw new OperationResultException("Exception is null.");
        ErrorMessage = exception.Message;
    }

    public Error(string errorMessage, Exception exception)
    {
        if (exception is null && string.IsNullOrWhiteSpace(errorMessage))
        {
            throw new OperationResultException("Both error message and exception are null or empty.");
        }

        Exception = exception;
        ErrorMessage = errorMessage ?? exception.Message;
    }

    /// <summary>
    /// Returns a new Error instance with the provided exception.
    /// </summary>
    /// <param name="exception">The exception to associate with the error.</param>
    /// <param name="strict">
    /// If true, throws an OperationResultException if the current Error instance already has an exception.
    /// If false, allows the new exception to replace the existing one.
    /// </param>
    /// <returns>A new Error instance with the provided exception.</returns>
    /// <exception cref="OperationResultException">Thrown if strict is true and the current Error instance already has an exception.</exception>
    /// <example>
    /// <code>
    /// var initialError = new Error("Initial error message");
    /// try
    /// {
    ///     // Some code that throws an exception
    ///     throw new InvalidOperationException("An invalid operation occurred.");
    /// }
    /// catch (Exception ex)
    /// {
    ///     var newError = initialError.WithException(ex);
    ///     Console.WriteLine(newError);
    /// }
    /// </code>
    /// </example>
    public Error WithException(Exception exception, bool strict = true)
    {
        if (strict && Exception is not null)
            throw new OperationResultException("Error already has an exception.");

        if (exception is null)
            throw new OperationResultException("Exception is null.");

        return new Error(ErrorMessage, exception);
    }

    public override string ToString()
    {
        return ErrorMessage ?? Exception.ToString();
    }
}