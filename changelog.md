# Changelog

## [1.2.0] - Aug, 2024
- Changed `Result` from class to `readonly struct`;
- Create the convenience method `Result<T>.ForwardError<TOher>()`, which will forward the error of the current 
`Result<T>` to a new `Result<TOther>`, if the current `Result<T>` is an error;

## [1.1.0] - Aug, 2024
- Added new List extension method: `SafeAll`: This method safely checks if all elements in a non-null, non-empty 
sequence satisfies a given condition, returning false for null or empty sources.

## [1.0.0] - Aug, 2024
- Initial release
