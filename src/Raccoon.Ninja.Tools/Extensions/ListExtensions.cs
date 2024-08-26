namespace Raccoon.Ninja.Tools.Extensions;

public static class ListExtensions
{
    /// <summary>
    ///     Returns an iterable list containing every item AND it's index.
    /// </summary>
    /// <param name="source">target IEnumerable</param>
    /// <typeparam name="T">data type of the items in the source</typeparam>
    /// <returns>IEnumerable containing tuples with index and title</returns>
    public static IEnumerable<(int index, T item)> ForEachWithIndex<T>(this IEnumerable<T> source)
    {
        return source.Select((item, index) => (index, item));
    }

    /// <summary>
    ///     Checks if the source containsText the specified string, ignoring case.
    /// This is safe to use with any IEnumerable. You can skip <see cref="source"/> or <see cref="containsText"/>
    /// validations, as this method will return false in case of null or empty values.
    ///  
    /// </summary>
    /// <param name="source">The source of strings to search.</param>
    /// <param name="containsText">The string to search for.</param>
    /// <param name="nullValuesAreErrors">
    ///     If true, null values in the source will be treated as errors and will not match the search string.
    ///     If false, null values in the source will be ignored.
    /// </param>
    /// <returns>
    ///     True if the source containsText the specified string (case-insensitive); otherwise, false.
    /// </returns>
    /// <raises>
    /// <see cref="NullReferenceException"/> when <see cref="nullValuesAreErrors"/> is true and a null value is present
    /// in the <see cref="source"/>.
    /// </raises>
    public static bool ContainsCaseInsensitive(this IEnumerable<string> source, string containsText, bool nullValuesAreErrors = true)
    {
        if (source is null || string.IsNullOrWhiteSpace(containsText))
            return false;

        return nullValuesAreErrors
            ? source.Any(i => i.Equals(containsText, StringComparison.InvariantCultureIgnoreCase)) 
            : source.Any(i => i?.Equals(containsText, StringComparison.InvariantCultureIgnoreCase) ?? false);
    }

    /// <summary>
    ///     Replaces the first occurrence of an object in the source.
    /// </summary>
    /// <param name="source">source of T</param>
    /// <param name="oldObj">old object to be replaced by new</param>
    /// <param name="newObj">new/updated object that will replace the old</param>
    /// <typeparam name="T">type of the objects in the source</typeparam>
    /// <returns>true if object is replaced, false if object is not found in source</returns>
    public static bool Replace<T>(this IList<T> source, T oldObj, T newObj)
    {
        var index = source.IndexOf(oldObj);

        if (index == -1) return false;

        source[index] = newObj;

        return true;
    }
}