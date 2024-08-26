using FluentAssertions;
using Raccoon.Ninja.Tools.OperationResult.Exceptions;
using Raccoon.Ninja.Tools.OperationResult.ResultError;


namespace Raccoon.Ninja.Tools.Tests.OperationResult.ResultError
{
    public class ErrorTests
    {
        private const string InvalidCtorArgsMessage = "Both error message and exception are null or empty.";

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
        public void Constructor_ShouldThrowException_WhenBothErrorMessageAndExceptionAreNullExplicitly(string errorMessage, Exception exception)
        {
            // Act
            Action act = () => new Error(errorMessage, exception);

            // Assert
            act.Should().Throw<OperationResultException>().WithMessage(InvalidCtorArgsMessage);
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
    }
}