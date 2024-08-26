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

    public override string ToString()
    {
        return ErrorMessage ?? Exception.ToString();
    }
}