# DateTime Extensions
The `DateTimeExtensions` class provides several useful extension methods for working with `DateTime` objects.

## `DaysSince`
Calculates the number of days since the specified date.

**Parameters:**
- `date` (DateTime): The date to calculate the days since.
- `currentDate` (DateTime?): The current date to compare against. If null (not provided), the current date and time will be used. (Default: current datetime)
- `allowMixedDateTimeKind` (bool): Indicates whether to allow mixed DateTimeKind values between the date and currentDate. (Default: false)

**Returns:**
- int: The number of days between the specified date and the current date.

**Example Usage:**
```csharp
var pastDate = new DateTime(2020, 1, 1);
var daysSince = pastDate.DaysSince(); // Calculates days since January 1, 2020 to today
```
