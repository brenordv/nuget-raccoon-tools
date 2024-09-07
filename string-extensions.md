# String Extensions
The `StringExtensions` class provides several useful extension methods for working with strings.

## `Minify`
Minifies a text by replacing spaces, tabs, and line breaks with a single space.

**Parameters:**
- `bigText` (string): The text to be minified.

**Returns:**
- string: The minified text.

**Example Usage:**
```csharp
string text = @"This is a   test.

                New line.";
string minifiedText = text.Minify();
Console.WriteLine(minifiedText); // Output: "This is a test. New line."
```

## `StripAccents`
Removes all diacritics (accents) from a string.

**Parameters:**
- `text` (string): The text from which to remove diacritics.

**Returns:**
- string: The text without diacritics.

**Example Usage:**
```csharp
string text = "Café";
string strippedText = text.StripAccents();
Console.WriteLine(strippedText); // Output: "Cafe"
```

## `OnlyDigits`
Removes everything that is not a digit from a string.

**Parameters:**
- `text` (string): The target string.

**Returns:**
- string: A string containing only digits.

**Example Usage:**
```csharp
string text = "Phone: 123-456-7890";
string digits = text.OnlyDigits();
Console.WriteLine(digits); // Output: "1234567890"
```
