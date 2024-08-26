using System.Diagnostics.CodeAnalysis;
using Raccoon.Ninja.Tools.OperationResult.ResultError.Interfaces;

namespace Raccoon.Ninja.Tools.OperationResult;

/// <summary>
/// A convenience class to represent the result of a validation operation,
/// where we just need to know if the validation was successful or not, but
/// don't care (or expect) a payload back.
/// </summary>
/// <inheritdoc cref="Result&lt;TPayload&gt;"/>
[ExcludeFromCodeCoverage]
public class ValidationResult: Result<bool>
{
    public ValidationResult(bool value) : base(value)
    { }

    public ValidationResult(IError error) : base(error)
    { }
}