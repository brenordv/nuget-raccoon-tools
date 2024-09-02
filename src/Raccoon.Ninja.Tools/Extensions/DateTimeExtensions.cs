namespace Raccoon.Ninja.Tools.Extensions;

public static class DateTimeExtensions
{
    /// <summary>
    /// Calculates the number of days since the specified date.
    /// </summary>
    /// <remarks>
    /// This method is UTC sensitive, so if the date being used is UTC, the current date will also be UTC.
    /// The detection is made using the DateTimeKind property of the date.
    /// </remarks>
    /// <param name="date">The date to calculate the days since.</param>
    /// <param name="currentDate">The current date to compare against. If null (not provided), the current date and
    /// time will be used. (Default: current datetime)</param>
    /// <param name="allowMixedDateTimeKind">Indicates whether to allow mixed DateTimeKind values between the date
    /// and currentDate. (Default: false)</param>
    /// <returns>The number of days between the specified date and the current date.</returns>
    /// <example>
    /// <code>
    /// var pastDate = new DateTime(2020, 1, 1);
    /// var daysSince = pastDate.DaysSince(); // Calculates days since January 1, 2020 to today
    /// </code>
    /// </example>
    /// <exception cref="ArgumentException">Thrown when the date and currentDate have different DateTimeKind values and allowMixedDateTimeKind is false.</exception>
    public static int DaysSince(this DateTime date, DateTime? currentDate = null, bool allowMixedDateTimeKind = false)
    {
        EnsureCanCalculateDaysSince(date, currentDate, allowMixedDateTimeKind);

        var now = ParseNow(currentDate, date.Kind == DateTimeKind.Utc);

        return (now - date).Days;
    }

    /// <summary>
    /// Ensures that the calculation of days since the specified date can be performed.
    /// </summary>
    /// <param name="date">The date to calculate the days since.</param>
    /// <param name="currentDate">The current date to compare against. If null (not provided), the current date and time will be used.</param>
    /// <param name="allowMixedDateTimeKind">Indicates whether to allow mixed DateTimeKind values between the date and currentDate.</param>
    /// <exception cref="ArgumentException">Thrown when the date and currentDate have different DateTimeKind values and allowMixedDateTimeKind is false.</exception>

    private static void EnsureCanCalculateDaysSince(DateTime date, DateTime? currentDate, bool allowMixedDateTimeKind)
    {
        if (allowMixedDateTimeKind || currentDate is null) return;

        if (date.Kind == currentDate.Value.Kind) return;
        
        throw new ArgumentException("The date and current date must have the same DateTimeKind.");
         
    }

    /// <summary>
    /// Parses the current date, defaulting to either UTC or local time.
    /// </summary>
    /// <param name="currentDate">The current date to use. If null, the current date and time will be used.</param>
    /// <param name="isUtc">Indicates whether to use UTC time or local time.</param>
    /// <returns>The parsed current date.</returns>
    /// <example>
    /// <code>
    /// DateTime? currentDate = null;
    /// DateTime now = ParseNow(currentDate, true); // Returns DateTime.UtcNow
    /// </code>
    /// </example>
    private static DateTime ParseNow(DateTime? currentDate, bool isUtc)
    {
        return currentDate ?? (isUtc ? DateTime.UtcNow : DateTime.Now);
    }
}