namespace Raccoon.Ninja.Tools.OperationResult.ResultError.Interfaces;

public interface IError
{
    string ErrorMessage { get; }
    Exception Exception { get; }
}