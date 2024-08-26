using System.Diagnostics.CodeAnalysis;

namespace Raccoon.Ninja.Tools.OperationResult.Exceptions;

[ExcludeFromCodeCoverage]
public class InvalidResultMapException : OperationResultException
{
    public InvalidResultMapException(string message = null)
        : base(message ?? "Result object was in an invalid state. Map function couldn't figure out what to do.")
    {
    }
}