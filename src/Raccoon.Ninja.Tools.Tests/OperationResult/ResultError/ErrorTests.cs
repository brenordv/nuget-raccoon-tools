using FluentAssertions;
using Raccoon.Ninja.Tools.OperationResult.Exceptions;
using Raccoon.Ninja.Tools.OperationResult.ResultError;


namespace Raccoon.Ninja.Tools.Tests.OperationResult.ResultError
{
    public class ErrorTests
    {
        private const string InvalidCtorArgsMessage = "Both error message and exception are null or empty.";

        #region Constructor

        [Fact]
        public void Constructor_ShouldThrowException_WhenBothErrorMessageAndExceptionAreNullImplicitly()
        {
            // Act
            Action act = () => new Error();

            // Assert
            act.Should().Throw<OperationResultException>()
                .WithMessage("Default constructor is not allowed.");
        }

        [Theory]
        [InlineData(null, null)]
        [InlineData("", null)]
        [InlineData(" ", null)]
        public void Constructor_ShouldThrowException_WhenBothErrorMessageIsNullExplicitly(string errorMessage,
            Exception exception)
        {
            // Act
            Action act = () => new Error(errorMessage, exception);

            // Assert
            act.Should().Throw<OperationResultException>().WithMessage(InvalidCtorArgsMessage);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void Constructor_ShouldThrowException_WhenErrorMessageIsNull(string errorMessage)
        {
            // Act
            Action act = () => new Error(errorMessage);

            // Assert
            act.Should().Throw<OperationResultException>().WithMessage("Error message is null or empty.");
        }

        [Fact]
        public void Constructor_ShouldSetErrorMessage_WhenOnlyErrorMessageIsProvided()
        {
            // Arrange
            const string errorMessage = "An error occurred";

            // Act
            var error = new Error(errorMessage);

            // Assert
            error.ErrorMessage.Should().Be(errorMessage);
            error.Exception.Should().BeNull();
        }

        [Fact]
        public void Constructor_ShouldSetExceptionMessage_WhenOnlyExceptionIsProvided()
        {
            // Arrange
            var exception = new Exception("Exception message");

            // Act
            var error = new Error(exception: exception);

            // Assert
            error.ErrorMessage.Should().Be(exception.Message);
            error.Exception.Should().Be(exception);
        }

        [Fact]
        public void Constructor_ShouldSetErrorMessageAndException_WhenBothAreProvided()
        {
            // Arrange
            const string errorMessage = "An error occurred";
            var exception = new Exception("Exception message");

            // Act
            var error = new Error(errorMessage, exception);

            // Assert
            error.ErrorMessage.Should().Be(errorMessage);
            error.Exception.Should().Be(exception);
        }

        #endregion

        #region WithException

        [Fact]
        public void WithException_ShouldThrowException_WhenStrictAndExceptionAlreadyExists()
        {
            // Arrange
            var initialException = new Exception("Initial exception");
            var error = new Error("Initial error message", initialException);
            var newException = new Exception("New exception");

            // Act
            Action act = () => error.WithException(newException, strict: true);

            // Assert
            act.Should().Throw<OperationResultException>()
                .WithMessage("Error already has an exception.");
        }

        [Fact]
        public void WithException_ShouldReplaceException_WhenNotStrictAndExceptionAlreadyExists()
        {
            // Arrange
            var initialException = new Exception("Initial exception");
            var error = new Error("Initial error message", initialException);
            var newException = new Exception("New exception");

            // Act
            var newError = error.WithException(newException, strict: false);

            // Assert
            newError.Exception.Should().Be(newException);
            newError.ErrorMessage.Should().Be("Initial error message");
        }

        [Fact]
        public void WithException_ShouldSetException_WhenNoExceptionExists()
        {
            // Arrange
            var error = new Error("Initial error message");
            var newException = new Exception("New exception");

            // Act
            var newError = error.WithException(newException);

            // Assert
            newError.Exception.Should().Be(newException);
            newError.ErrorMessage.Should().Be("Initial error message");
        }

        [Fact]
        public void WithException_ShouldThrowException_WhenExceptionIsNull()
        {
            // Arrange
            var error = new Error("Initial error message");

            // Act
            Action act = () => error.WithException(null);

            // Assert
            act.Should().Throw<OperationResultException>()
                .WithMessage("Exception is null.");
        }

        #endregion
        
        #region ToString

        [Fact]
        public void ToString_ShouldReturnErrorMessage_WhenErrorMessageIsSet()
        {
            // Arrange
            const string errorMessage = "An error occurred";
            var error = new Error(errorMessage);

            // Act
            var result = error.ToString();

            // Assert
            result.Should().Be(errorMessage);
        }

        [Fact]
        public void ToString_ShouldReturnExceptionMessage_WhenErrorMessageIsNotSet()
        {
            // Arrange
            var exception = new Exception("Exception message");
            var error = new Error(exception: exception);

            // Act
            var result = error.ToString();

            // Assert
            result.Should().Be(exception.Message);
        }

        #endregion
    }
}