using System.Diagnostics.CodeAnalysis;

namespace Raccoon.Ninja.Tools.OperationResult.Exceptions;

[ExcludeFromCodeCoverage]
public class OperationResultException: Exception
{
    public OperationResultException()
    { }

    public OperationResultException(string message): base(message)
    { }
    
    public OperationResultException(string message, Exception innerException): base(message, innerException)
    { }
}