# Changelog

## [1.8.0] - Sep, 2024
- Added `WithMessage` to the `Error` class to allow the error message to be changed. It will allow for more flexibility when
  handling errors;

## [1.7.0] - Sep, 2024
- Added `ChainOnSuccess` and `ChainOnSuccessAsync` extension  methods to help reduce boilerplate;

## [1.6.0] - Sep, 2024
- Added `TapOnSuccess` and `TapOnSuccessAsync` extension  methods to help reduce boilerplate;

## [1.5.0] - Sep, 2024
- Renamed method `Map` to `Tap` to better reflect its purpose;
- Added `TapAsync` method to `Result<T>` class to allow side effects without changing the result;
- Added `ProcessAsync` method to `Result<T>` class to allow processing the result asynchronously;
- Organized the project documentation;

## [1.4.0] - Sep, 2024
- Added DateTime extension methods:
  - `DaysSince`: Returns the number of days since a given date (default: current date time);

## [1.3.0] - Aug, 2024
-  Added the following ListExtension methods:
  - `Shuffle`: This method shuffles the elements of a list in place;
  - `Random`: This method returns a random element from a list;
  - `PopLast`: This method removes and returns the last element of a list;
  - `PopFirst`: This method removes and returns the first element of a list;
  - `IndexOfMax`: This method returns the index of the maximum element in a list;
  - `IndexOfMin`: This method returns the index of the minimum element in a list;
  - `RemoveDuplicates`: This method removes all duplicates from a list;

## [1.2.0] - Aug, 2024
- Changed `Result` from class to `readonly struct`;
- Create the convenience method `Result<T>.ForwardError<TOher>()`, which will forward the error of the current 
`Result<T>` to a new `Result<TOther>`, if the current `Result<T>` is an error;

## [1.1.0] - Aug, 2024
- Added new List extension method: `SafeAll`: This method safely checks if all elements in a non-null, non-empty 
sequence satisfies a given condition, returning false for null or empty sources.

## [1.0.0] - Aug, 2024
- Initial release
