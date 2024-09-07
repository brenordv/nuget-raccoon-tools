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

        #endregion

        #region Map

        [Fact]
        public void Map_ShouldCallSuccessAction_WhenResultIsSuccess()
        {
            // Arrange
            const string payload = "Success payload";
            var result = new Result<string>(payload);
            var successCalled = false;

            // Act
            result.Map(
                _ => successCalled = true,
                _ => throw new Exception("Should not be called")
            );

            // Assert
            successCalled.Should().BeTrue();
        }

        [Fact]
        public void Map_ShouldCallFailureAction_WhenResultIsFailure()
        {
            // Arrange
            var error = new Error("Error message");
            var result = new Result<string>(error);
            var failureCalled = false;

            // Act
            result.Map(
                _ => throw new Exception("Should not be called"),
                _ => failureCalled = true
            );

            // Assert
            failureCalled.Should().BeTrue();
        }

        [Fact]
        public void Map_ShouldThrowInvalidResultMapException_WhenBothErrorAndFailureActionAreNull()
        {
            // Arrange
            var result = new Result<string>((IError)null);

            // Act
            var act = () => result.Map(
                _ => throw new Exception("Should not be called"),
                _ => throw new Exception("Should not be called")
            );

            // Assert
            act.Should().Throw<InvalidResultMapException>();
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