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
}