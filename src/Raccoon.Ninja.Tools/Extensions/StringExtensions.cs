using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace Raccoon.Ninja.Tools.Extensions;

public static partial class StringExtensions
{

    /// <summary>
    ///     Regex pattern that will seek one or more spaces, tabs and line breaks.
    /// </summary>
    [GeneratedRegex(@"(\s+|\t+|\r+|\n+)")]
    private static partial Regex BreakLineRegex();
    
    /// <summary>
    ///  Regex pattern that will seek everything that is not a digit.
    /// </summary>
    [GeneratedRegex("[^0-9]")]
    private static partial Regex OnlyDigitsRegex();

    /// <summary>
    ///     Minifies a text.
    ///     Replaces everything that is caught by the Pattern variable and replaces it with one space.
    ///     If null or empty, will return null.
    /// </summary>
    /// <param name="bigText">Text to be minified.</param>
    /// <returns>Minified text.</returns>
    public static string Minify(this string bigText)
    {
        return string.IsNullOrWhiteSpace(bigText)
            ? bigText
            : BreakLineRegex().Replace(bigText, " ").Trim();
    }

    /// <summary>
    ///     Remove accents... well, actually, remove all DIACRITICS from a string,
    ///     but saying 'accents' is easier to remember.
    /// </summary>
    /// <param name="text">Text that will be used to remove diacritics</param>
    /// <returns>Text without diacritics</returns>
    public static string StripAccents(this string text)
    {
        var stFormD = text.Normalize(NormalizationForm.FormD);
        var sb = new StringBuilder();

        foreach (var t in stFormD)
        {
            var uc = CharUnicodeInfo.GetUnicodeCategory(t);
            if (uc == UnicodeCategory.NonSpacingMark) continue;
            sb.Append(t);
        }

        return sb.ToString().Normalize(NormalizationForm.FormC);
    }

    /// <summary>
    ///     Removes everything that's not a digit (0 through 9) from a string.
    /// </summary>
    /// <param name="text">target string</param>
    /// <returns>string containing only numbers</returns>
    public static string OnlyDigits(this string text)
    {
        return OnlyDigitsRegex().Replace(text, "");
    }
}