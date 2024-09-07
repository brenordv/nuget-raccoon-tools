using FluentAssertions;
using Raccoon.Ninja.Tools.OperationResult;
using Raccoon.Ninja.Tools.OperationResult.ResultError;
using Raccoon.Ninja.Tools.OperationResult.ResultExtensions;

namespace Raccoon.Ninja.Tools.Tests.OperationResult.ResultExtensions;

public class ResultExtensionsTests
{
    #region TapOnSuccess

    [Fact]
    public void TapOnSuccess_ShouldExecuteAction_WhenResultIsSuccessAndPayloadIsTrue()
    {
        // Arrange
        var result = new Result<bool>(true);
        var actionExecuted = false;

        // Act
        var isSuccess = result.TapOnSuccess(() => actionExecuted = true);

        // Assert
        isSuccess.Should().BeTrue();
        actionExecuted.Should().BeTrue();
    }

    [Fact]
    public void TapOnSuccess_ShouldNotExecuteAction_WhenResultIsSuccessAndPayloadIsFalse()
    {
        // Arrange
        var result = new Result<bool>(false);
        var actionExecuted = false;

        // Act
        var isSuccess = result.TapOnSuccess(() => actionExecuted = true);

        // Assert
        isSuccess.Should().BeFalse();
        actionExecuted.Should().BeFalse();
    }

    [Fact]
    public void TapOnSuccess_ShouldNotExecuteAction_WhenResultIsFailure()
    {
        // Arrange
        var error = new Error("An error occurred");
        var result = new Result<bool>(error);
        var actionExecuted = false;

        // Act
        var isSuccess = result.TapOnSuccess(() => actionExecuted = true);

        // Assert
        isSuccess.Should().BeFalse();
        actionExecuted.Should().BeFalse();
    }

    #endregion

    #region TagOnSuccessAsync

    [Fact]
    public async Task TapOnSuccessAsync_ShouldExecuteAction_WhenResultIsSuccessAndPayloadIsTrue()
    {
        // Arrange
        var result = new Result<bool>(true);
        var actionExecuted = false;

        // Act
        var isSuccess = await result.TapOnSuccessAsync(async () =>
        {
            actionExecuted = true;
            await Task.CompletedTask;
        });

        // Assert
        isSuccess.Should().BeTrue();
        actionExecuted.Should().BeTrue();
    }

    [Fact]
    public async Task TapOnSuccessAsync_ShouldNotExecuteAction_WhenResultIsSuccessAndPayloadIsFalse()
    {
        // Arrange
        var result = new Result<bool>(false);
        var actionExecuted = false;

        // Act
        var isSuccess = await result.TapOnSuccessAsync(async () =>
        {
            actionExecuted = true;
            await Task.CompletedTask;
        });

        // Assert
        isSuccess.Should().BeFalse();
        actionExecuted.Should().BeFalse();
    }

    [Fact]
    public async Task TapOnSuccessAsync_ShouldNotExecuteAction_WhenResultIsFailure()
    {
        // Arrange
        var error = new Error("An error occurred");
        var result = new Result<bool>(error);
        var actionExecuted = false;

        // Act
        var isSuccess = await result.TapOnSuccessAsync(async () =>
        {
            actionExecuted = true;
            await Task.CompletedTask;
        });

        // Assert
        isSuccess.Should().BeFalse();
        actionExecuted.Should().BeFalse();
    }

    #endregion
    
    #region ChainOnSuccess

    [Fact]
    public void ChainOnSuccess_ShouldReturnChainedResult_WhenResultIsSuccessAndPayloadIsTrue()
    {
        // Arrange
        var initialResult = new Result<bool>(true);

        // Act
        var chainedResult = initialResult.ChainOnSuccess(() => new Result<string>("Chained Success"));

        // Assert
        chainedResult.Tap(
            payload => payload.Should().Be("Chained Success"),
            error => throw new Exception("Should not be an error")
        );
    }

    [Fact]
    public void ChainOnSuccess_ShouldNotExecuteAction_WhenResultIsSuccessAndPayloadIsFalse()
    {
        // Arrange
        var initialResult = new Result<bool>(false);

        // Act
        var chainedResult = initialResult.ChainOnSuccess(() => new Result<string>("This won't be executed"));

        // Assert
        chainedResult.Tap(
            payload => throw new Exception("This won't be executed"),
            error => error.Should().BeEquivalentTo(ErrorPresets.NotAbleToChainOnSuccess)
        );
    }

    [Fact]
    public void ChainOnSuccess_ShouldForwardError_WhenResultIsFailure()
    {
        // Arrange
        var error = new Error("Initial error");
        var initialResult = new Result<bool>(error);

        // Act
        var chainedResult = initialResult.ChainOnSuccess(() => new Result<string>("This won't be executed"));

        // Assert
        chainedResult.Tap(
            payload => throw new Exception("This won't be executed"),
            resultError => error.Should().Be(resultError)
        );
    }

    [Fact]
    public void ChainOnSuccess_ShouldHandleNullActionOnSuccess()
    {
        // Arrange
        var initialResult = new Result<bool>(true);

        // Act
        Action act = () => initialResult.ChainOnSuccess<string>(null);

        // Assert
        act.Should().Throw<NullReferenceException>();
    }

    #endregion
    
    #region ChainOnSuccessAsync

    [Fact]
    public async Task ChainOnSuccessAsync_ShouldReturnChainedResult_WhenResultIsSuccessAndPayloadIsTrue()
    {
        // Arrange
        var initialResult = new Result<bool>(true);

        // Act
        var chainedResult = await initialResult.ChainOnSuccessAsync(async () =>
        {
            await Task.CompletedTask;
            return new Result<string>("Chained Success");
        });

        // Assert
        chainedResult.Tap(
            payload => payload.Should().Be("Chained Success"),
            error => throw new Exception("Should not be an error")
        );
    }

    [Fact]
    public async Task ChainOnSuccessAsync_ShouldNotExecuteAction_WhenResultIsSuccessAndPayloadIsFalse()
    {
        // Arrange
        var initialResult = new Result<bool>(false);

        // Act
        var chainedResult = await initialResult.ChainOnSuccessAsync(async () =>
        {
            await Task.CompletedTask;
            return new Result<string>("This won't be executed");
        });

        // Assert
        chainedResult.Tap(
            payload => throw new Exception("This won't be executed"),
            error => error.Should().BeEquivalentTo(ErrorPresets.NotAbleToChainOnSuccess)
        );
    }

    [Fact]
    public async Task ChainOnSuccessAsync_ShouldForwardError_WhenResultIsFailure()
    {
        // Arrange
        var error = new Error("Initial error");
        var initialResult = new Result<bool>(error);

        // Act
        var chainedResult = await initialResult.ChainOnSuccessAsync(async () =>
        {
            await Task.CompletedTask;
            return new Result<string>("This won't be executed");
        });

        // Assert
        chainedResult.Tap(
            payload => throw new Exception("This won't be executed"),
            resultError => error.Should().Be(resultError)
        );
    }

    [Fact]
    public async Task ChainOnSuccessAsync_ShouldHandleNullActionOnSuccess()
    {
        // Arrange
        var initialResult = new Result<bool>(true);

        // Act
        Func<Task> act = async () => await initialResult.ChainOnSuccessAsync<string>(null);

        // Assert
        await act.Should().ThrowAsync<NullReferenceException>();
    }

    #endregion
}