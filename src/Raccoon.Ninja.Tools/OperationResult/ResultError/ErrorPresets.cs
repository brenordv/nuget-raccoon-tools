using System.Diagnostics.CodeAnalysis;
using Raccoon.Ninja.Tools.OperationResult.ResultError.Interfaces;

namespace Raccoon.Ninja.Tools.OperationResult.ResultError;

/// <summary>
/// A class to hold common errors that will be used across the application.
/// </summary>
[ExcludeFromCodeCoverage]
public static class ErrorPresets
{
    public static readonly IError NotAbleToChainOnSuccess =
        new Error("Previous check was not successful, but no errors were returned. Cannot chain with next step.");
}