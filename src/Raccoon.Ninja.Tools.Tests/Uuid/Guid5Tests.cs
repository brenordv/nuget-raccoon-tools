using FluentAssertions;
using Raccoon.Ninja.Tools.Uuid;

namespace Raccoon.Ninja.Tools.Tests.Uuid;

public class Guid5Tests
{
    [Fact]
    public void NewGuid_WithValidArguments_ShouldReturnDeterministicGuid()
    {
        // Arrange
        object[] args = ["test", 123, true];
        
        // Same values, different allocation.
        object[] args2 = ["test", 123, true]; 

        // Act
        var result1 = Guid5.NewGuid(args);
        var result2 = Guid5.NewGuid(args2);
        var result3 = Guid5.NewGuid(args);
        var result4 = Guid5.NewGuid(args2);
        
        // Assert
        result1.Should().Be(result2);
        result2.Should().Be(result3);
        result3.Should().Be(result4);
    }

    [Fact]
    public void NewGuid_WithDifferentArguments_ShouldReturnDifferentGuids()
    {
        // Arrange
        object[] args1 = { "test", 123, true };
        object[] args2 = { "test", 123, false };

        // Act
        var result1 = Guid5.NewGuid(args1);
        var result2 = Guid5.NewGuid(args2);

        // Assert
        result1.Should().NotBe(result2);
    }

    [Fact]
    public void NewGuid_WithNullArguments_ShouldThrowArgumentNullException()
    {
        // Act & Assert
        FluentActions.Invoking(() => Guid5.NewGuid(null))
            .Should().Throw<ArgumentNullException>()
            .WithParameterName("args")
            .WithMessage("Arguments must not be empty. (Parameter 'args')");
    }

    [Fact]
    public void NewGuid_WithEmptyArguments_ShouldThrowArgumentNullException()
    {
        // Act & Assert
        FluentActions.Invoking(() => Guid5.NewGuid())
            .Should().Throw<ArgumentNullException>()
            .WithParameterName("args")
            .WithMessage("Arguments must not be empty. (Parameter 'args')");
    }

    [Fact]
    public void NewGuid_WithSingleArgument_ShouldReturnValidGuid()
    {
        // Arrange
        object arg = "singleArgument";

        // Act
        var result = Guid5.NewGuid(arg);

        // Assert
        result.Should().NotBe(Guid.Empty);
    }

    [Fact]
    public void NewGuid_WithSameArgumentsInDifferentOrder_ShouldReturnDifferentGuids()
    {
        // Arrange
        object[] args1 = { "a", "b", "c" };
        object[] args2 = { "c", "b", "a" };

        // Act
        var result1 = Guid5.NewGuid(args1);
        var result2 = Guid5.NewGuid(args2);

        // Assert
        result1.Should().NotBe(result2);
    }

    [Theory]
    [MemberData(nameof(GetValidArguments))]
    public void NewGuid_WithValidArguments_ShouldReturnValidGuid(params object[] args)
    {
        // Act
        var result = Guid5.NewGuid(args);

        // Assert
        result.Should().NotBe(Guid.Empty);
    }
    
    [Fact]
    public void NewGuid_WithSimilarLongStrings_ShouldReturnValidAndUniqueGuids()
    {
        // Arrange
        var longString1 = new string('a', 1000000);
        var longString2 = new string('a', 1000000) + "b";
        
        // Act
        var result1 = Guid5.NewGuid(longString1);
        var result2 = Guid5.NewGuid(longString2);

        // Assert
        result1.Should().NotBe(Guid.Empty);
        result2.Should().NotBe(Guid.Empty);
        result1.Should().NotBe(result2);
    }
    
    #region Test Helpers
    public static TheoryData<object[]> GetValidArguments()
    {
        return
        [
            new object[] { "test", null, 123 },
            new object[] { "test", 123, true },
            new object[] { new string('a', 1000000) },
            new object[] { "arg1", 42, DateTime.Now, true },
            new object[] { "arg1", 42, DateTime.Now, true, "arg5" },
            new object[] { "!@#$%^&*()" }
        ];
    }
    #endregion
}