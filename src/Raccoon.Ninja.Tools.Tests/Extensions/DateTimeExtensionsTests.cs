using FluentAssertions;
using Raccoon.Ninja.Tools.Extensions;

namespace Raccoon.Ninja.Tools.Tests.Extensions;

public class DateTimeExtensionsTests
{
    [Theory]
    [MemberData(nameof(GetDateTimeKindValues))]
    public void DaysSince_PastDate_ShouldReturnCorrectDays(DateTimeKind kind)
    {
        // Arrange
        var pastDate = BuildDateTime(kind, 2020, 1, 1);
        var currentDate = GetNow(kind);
        var expectedDays = (currentDate - pastDate).Days;

        // Act
        var result = pastDate.DaysSince();

        // Assert
        result.Should().Be(expectedDays);
    }

    [Theory]
    [MemberData(nameof(GetDateTimeKindValues))]
    public void DaysSince_PastDateWithProvidedCurrentDate_ShouldReturnCorrectDays(DateTimeKind kind)
    {
        // Arrange
        var pastDate = BuildDateTime(kind, 2020, 1, 1);
        var currentDate = BuildDateTime(kind, 2023, 1, 1);
        var expectedDays = (currentDate - pastDate).Days;

        // Act
        var result = pastDate.DaysSince(currentDate);

        // Assert
        result.Should().Be(expectedDays);
    }

    [Theory]
    [MemberData(nameof(GetDateTimeKindValues))]
    public void DaysSince_FutureDate_ShouldReturnNegativeDays(DateTimeKind kind)
    {
        // Arrange
        var currentDate = GetNow(kind);
        var futureDate = BuildDateTime(kind, currentDate.Year + 5, 1, 1);
        var expectedDays = (currentDate - futureDate).Days;

        // Act
        var result = futureDate.DaysSince();

        // Assert
        result.Should().Be(expectedDays);
    }

    [Theory]
    [MemberData(nameof(GetDateTimeKindValues))]
    public void DaysSince_SameDate_ShouldReturnZero(DateTimeKind kind)
    {
        // Arrange
        var date = GetNow(kind);
        const int expectedDays = 0;

        // Act
        var result = date.DaysSince(date);

        // Assert
        result.Should().Be(expectedDays);
    }

    [Theory]
    [MemberData(nameof(GetDateTimeKindValues))]
    public void DaysSince_WithPastDate_ReturnsPositiveDays(DateTimeKind kind)
    {
        //Arrange
        const int expectedDays = 5;
        var pastDate = GetNow(kind).AddDays(-expectedDays);
        
        //Act
        var result = pastDate.DaysSince();
        
        //Assert
        result.Should().Be(expectedDays);
    }

    [Theory]
    [MemberData(nameof(GetDateTimeKindValues))]
    public void DaysSince_WithLeapYear_CalculatesCorrectly(DateTimeKind kind)
    {
        // Arrange
        var leapYearDate = BuildDateTime(kind, 2020, 2, 28);
        var currentDate = BuildDateTime(kind, 2020, 3, 1);
        const int expectedDays = 2; // February 29 is included

        // Act
        var result = leapYearDate.DaysSince(currentDate);

        // Assert
        result.Should().Be(expectedDays);
    }

    [Theory]
    [MemberData(nameof(GetDateTimeKindValues))]
    public void DaysSince_WithLongTimePeriod_CalculatesCorrectly(DateTimeKind kind)
    {
        // Arrange
        var pastDate = BuildDateTime(kind, 1900, 1, 1);
        var currentDate = BuildDateTime(kind, 2100, 1, 1);
        var expectedDays = (currentDate - pastDate).Days;

        // Act
        var result = pastDate.DaysSince(currentDate);

        // Assert
        result.Should().Be(expectedDays);
    }

    [Theory]
    [MemberData(nameof(GetDateTimeKindValues))]
    public void DaysSince_WithMinValue_CalculatesCorrectly(DateTimeKind kind)
    {
        var minDate = DateTime.SpecifyKind(DateTime.MinValue, kind);
        var currentDate = GetNow(kind);
        var result = minDate.DaysSince(currentDate);
        result.Should().Be((currentDate - minDate).Days);
    }

    [Theory]
    [MemberData(nameof(GetDateTimeKindValues))]
    public void DaysSince_WithMaxValue_CalculatesCorrectly(DateTimeKind kind)
    {
        var maxDate = DateTime.SpecifyKind(DateTime.MaxValue, kind);
        var currentDate = GetNow(kind);
        var result = maxDate.DaysSince(currentDate);
        result.Should().Be((currentDate - maxDate).Days);
    }
    
    [Theory]
    [MemberData(nameof(GetDateTimeKindValues))]
    public void EnsureCanCalculateDaysSince_DifferentDateTimeKind_ShouldThrowArgumentException(DateTimeKind kind)
    {
        // Arrange
        var date = BuildDateTime(kind, 2020, 1, 1);
        var differentKind = kind switch
        {
            DateTimeKind.Local => DateTimeKind.Utc,
            DateTimeKind.Utc => DateTimeKind.Local,
            _ => DateTimeKind.Local
        };

        var currentDate = GetNow(differentKind);

        // Act
        Action act = () => date.DaysSince(currentDate);

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("The date and current date must have the same DateTimeKind.");
    }

    [Theory]
    [MemberData(nameof(GetDateTimeKindValues))]
    public void EnsureCanCalculateDaysSince_AllowedDifferentDateTimeKind_ShouldNotThrowArgumentException(DateTimeKind kind)
    {
        // Arrange
        var date = BuildDateTime(kind, 2020, 1, 1);
        var differentKind = kind switch
        {
            DateTimeKind.Local => DateTimeKind.Utc,
            DateTimeKind.Utc => DateTimeKind.Local,
            _ => DateTimeKind.Local
        };

        var currentDate = GetNow(differentKind);

        // Act
        Action act = () => date.DaysSince(currentDate, true);

        // Assert
        act.Should().NotThrow<ArgumentException>();
    }
    
    #region Test Helpers
    public static TheoryData<DateTimeKind> GetDateTimeKindValues()
    {
        return new TheoryData<DateTimeKind>
        {
            DateTimeKind.Local,
            DateTimeKind.Utc,
            DateTimeKind.Unspecified
        };
    }

    private static DateTime BuildDateTime(DateTimeKind kind, int year, int month, int day, int hour = 12,
        int minutes = 0, int seconds = 0)
    {
        return new DateTime(year, month, day, hour, minutes, seconds, kind);
    }

    private static DateTime GetNow(DateTimeKind kind)
    {
        return DateTime.SpecifyKind(DateTime.Now, kind);
    }
    #endregion
}