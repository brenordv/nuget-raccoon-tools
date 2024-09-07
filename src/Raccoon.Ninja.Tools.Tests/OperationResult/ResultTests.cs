using FluentAssertions;
using Raccoon.Ninja.Tools.OperationResult;
using Raccoon.Ninja.Tools.OperationResult.Exceptions;
using Raccoon.Ninja.Tools.OperationResult.ResultError;
using Raccoon.Ninja.Tools.OperationResult.ResultError.Interfaces;

namespace Raccoon.Ninja.Tools.Tests.OperationResult
{
    public class ResultTests
    {
        #region Constructor

        [Fact]
        public void Constructor_ShouldSetPayload_WhenInitializedWithValue()
        {
            // Arrange
            const string payload = "Success payload";

            // Act
            var result = new Result<string>(payload);

            // Assert
            result.Process(
                success => success.Should().Be(payload),
                _ => throw new Exception("Should not be called")
            );
        }

        [Fact]
        public void Constructor_ShouldSetError_WhenInitializedWithError()
        {
            // Arrange
            var error = new Error("Error message");

            // Act
            var result = new Result<string>(error);

            // Assert
            result.Process(
                _ => throw new Exception("Should not be called"),
                failure => failure.Should().Be(error)
            );
        }

        #endregion

        #region Implicit Conversion

        [Fact]
        public void ImplicitConversion_ShouldCreateResultFromPayload()
        {
            // Arrange
            const string payload = "Success payload";

            // Act
            Result<string> result = payload;

            // Assert
            result.Process(
                success => success.Should().Be(payload),
                _ => throw new Exception("Should not be called")
            );
        }

        [Fact]
        public void ImplicitConversion_ShouldCreateResultFromError()
        {
            // Arrange
            var error = new Error("Error message");

            // Act
            Result<string> result = error;

            // Assert
            result.Process(
                _ => throw new Exception("Should not be called"),
                failure => failure.Should().Be(error)
            );
        }

        [Fact]
        public void ImplicitConversion_ShouldCreateResultFromException()
        {
            // Arrange
            var exception = new Exception("Exception message");

            // Act
            Result<string> result = exception;

            // Assert
            result.Process(
                _ => throw new Exception("Should not be called"),
                failure => failure.Exception.Should().Be(exception)
            );
        }
        
        [Fact]
        public void ImplicitConversion_ShouldThrowAnException_WhenConvertingFromNullToNotNullablePayloadType()
        {
            // This is a weird edge case. If you try to assign null to a Result object with a non-nullable type
            // (e.g., Result<int>), the compiler will understand that you're trying to implicitly convert from an
            // error object, and since all arguments of this "error object" is null, it will throw an exception.
            // It's not possible to do this with a nullable type (e.g., Result<int?>), because the compiler won't 
            // know what to do with the null value.

            // Arrange + Act
            var act = () =>
            {
                Result<bool> result = null;
            };

            // Assert
            act.Should().Throw<OperationResultException>()
                .WithMessage("Both error message and exception are null or empty.");
        }

        #endregion

        #region Tap

        [Fact]
        public void Tap_ShouldCallSuccessAction_WhenResultIsSuccess()
        {
            // Arrange
            const string payload = "Success payload";
            var result = new Result<string>(payload);
            var successCalled = false;

            // Act
            result.Tap(
                _ => successCalled = true,
                _ => throw new Exception("Should not be called")
            );

            // Assert
            successCalled.Should().BeTrue();
        }

        [Fact]
        public void Tap_ShouldCallFailureAction_WhenResultIsFailure()
        {
            // Arrange
            var error = new Error("Error message");
            var result = new Result<string>(error);
            var failureCalled = false;

            // Act
            result.Tap(
                _ => throw new Exception("Should not be called"),
                _ => failureCalled = true
            );

            // Assert
            failureCalled.Should().BeTrue();
        }

        [Fact]
        public void Tap_ShouldThrowInvalidResultMapException_WhenBothErrorAndFailureActionAreNull()
        {
            // Arrange
            var result = new Result<string>((IError)null);

            // Act
            var act = () => result.Tap(
                _ => throw new Exception("Should not be called"),
                _ => throw new Exception("Should not be called")
            );

            // Assert
            act.Should().Throw<InvalidResultMapException>();
        }

        #endregion

        #region TapAsync

        [Fact]
        public async Task TapAsync_ShouldCallSuccessAction_WhenResultIsSuccess()
        {
            // Arrange
            const string payload = "Success payload";
            var result = new Result<string>(payload);
            var successCalled = false;

            // Act
            await result.TapAsync(
                async _ =>
                {
                    await Task.CompletedTask;
                    successCalled = true;
                },
                async _ =>
                {
                    await Task.CompletedTask;
                    throw new Exception("Should not be called");
                }
            );

            // Assert
            successCalled.Should().BeTrue();
        }

        [Fact]
        public async Task TapAsync_ShouldCallFailureAction_WhenResultIsFailure()
        {
            // Arrange
            var error = new Error("Error message");
            var result = new Result<string>(error);
            var failureCalled = false;

            // Act
            await result.TapAsync(
                async _ =>
                {
                    await Task.CompletedTask;
                    throw new Exception("Should not be called");
                },
                async _ =>
                {
                    await Task.CompletedTask;
                    failureCalled = true;
                }
            );

            // Assert
            failureCalled.Should().BeTrue();
        }

        [Fact]
        public async Task TapAsync_ShouldThrowInvalidResultMapException_WhenBothErrorAndFailureActionAreNull()
        {
            // Arrange
            var result = new Result<string>((IError)null);

            // Act
            var act = async () => await result.TapAsync(
                async _ =>
                {
                    await Task.CompletedTask;
                    throw new Exception("Should not be called");
                },
                async _ =>
                {
                    await Task.CompletedTask;
                    throw new Exception("Should not be called");
                }
            );

            // Assert
            await act.Should().ThrowAsync<InvalidResultMapException>();
        }

        #endregion
        
        #region Process

        [Fact]
        public void Process_ShouldCallOnSuccessFunction_WhenResultIsSuccess()
        {
            // Arrange
            const string payload = "Success payload";
            var result = new Result<string>(payload);

            // Act
            var processedPayload = result.Process(
                resultPayload => $"Processed: {resultPayload}",
                _ => "Should not be called"
            );

            // Assert
            processedPayload.Should().Be($"Processed: {payload}");
        }

        [Fact]
        public void Process_ShouldCallOnFailureFunction_WhenResultIsFailure()
        {
            // Arrange
            var error = new Error("Error message");
            var result = new Result<string>(error);

            // Act
            var processedPayload = result.Process(
                _ => "Should not be called",
                failure => $"Failed: {failure.ErrorMessage}"
            );

            // Assert
            processedPayload.Should().Be($"Failed: {error.ErrorMessage}");
        }

        [Fact]
        public void Process_ShouldThrowInvalidResultMapException_WhenBothErrorAndOnFailureFunctionAreNull()
        {
            // Arrange
            var result = new Result<string>((IError)null);

            // Act
            Action act = () => result.Process(
                _ => "Should not be called",
                _ => "Should not be called"
            );

            // Assert
            act.Should().Throw<InvalidResultMapException>();
        }

        #endregion

        #region ProcessAsync

        [Fact]
        public async Task ProcessAsync_ShouldCallOnSuccessFunction_WhenResultIsSuccess()
        {
            // Arrange
            const string payload = "Success payload";
            var result = new Result<string>(payload);

            // Act
            var processedPayload = await result.ProcessAsync(
                async resultPayload =>
                {
                    await Task.CompletedTask;
                    return $"Processed: {resultPayload}";
                },
                async _ =>
                {
                    await Task.CompletedTask;
                    return "Should not be called";
                }
            );

            // Assert
            processedPayload.Should().Be($"Processed: {payload}");
        }

        [Fact]
        public async Task ProcessAsync_ShouldCallOnFailureFunction_WhenResultIsFailure()
        {
            // Arrange
            var error = new Error("Error message");
            var result = new Result<string>(error);

            // Act
            var processedPayload = await result.ProcessAsync(
                async _ =>
                {
                    await Task.CompletedTask;
                    return "Should not be called";
                },
                async failure =>
                {
                    await Task.CompletedTask;
                    return $"Failed: {failure.ErrorMessage}";
                }
            );

            // Assert
            processedPayload.Should().Be($"Failed: {error.ErrorMessage}");
        }

        [Fact]
        public async Task ProcessAsync_ShouldThrowInvalidResultMapException_WhenBothErrorAndOnFailureFunctionAreNull()
        {
            // Arrange
            var result = new Result<string>((IError)null);

            // Act
            var act = async () => await result.ProcessAsync(
                async _ =>
                {
                    await Task.CompletedTask;
                    return "Should not be called";
                },
                async _ =>
                {
                    await Task.CompletedTask;
                    return "Should not be called";
                }
            );

            // Assert
            await act.Should().ThrowAsync<InvalidResultMapException>();
        }

        #endregion
        
        #region ToString

        [Fact]
        public void ToString_ShouldReturnSuccessMessage_WhenResultIsSuccess()
        {
            // Arrange
            const string payload = "Success payload";
            var result = new Result<string>(payload);

            // Act
            var resultString = result.ToString();

            // Assert
            resultString.Should().Be($"[Result:Success] {payload}");
        }

        [Fact]
        public void ToString_ShouldReturnFailureMessage_WhenResultIsFailure()
        {
            // Arrange
            var error = new Error("Error message");
            var result = new Result<string>(error);

            // Act
            var resultString = result.ToString();

            // Assert
            resultString.Should().Be($"[Result:Failure] {error}");
        }

        #endregion

        #region ForwardError

        [Fact]
        public void ForwardError_ShouldForwardError_WhenResultIsFailure()
        {
            // Arrange
            var error = new Error("Error message");
            var result = new Result<string>(error);

            // Act
            var forwardedResult = result.ForwardError<int>();

            // Assert
            forwardedResult.Process(
                _ => throw new Exception("Should not be called"),
                failure => failure.Should().Be(error)
            );
        }

        [Fact]
        public void ForwardError_ShouldThrowException_WhenResultIsSuccess()
        {
            // Arrange
            var result = new Result<string>("Success payload");

            // Act
            var act = () => result.ForwardError<int>();

            // Assert
            act.Should().Throw<OperationResultException>()
                .WithMessage("Cannot forward error from a successful result.");
        }

        #endregion
    }
}