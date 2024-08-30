namespace Raccoon.Ninja.Tools.Extensions;

public static class ListExtensions
{
    /// <summary>
    ///     Determines whether all elements of a sequence satisfy a condition safely.
    /// The difference between this method and the original <see cref="Enumerable.All{TSource}"/>
    /// is that this method checks if the source is null or empty before trying to iterate over it.
    /// The <see cref="Enumerable.All{TSource}"/> method will also return true if the IEnumerable
    /// is empty.
    /// This is usually not what we want, so this extension method works as a convenient way to
    /// check if all items in the source satisfy the condition.
    /// </summary>
    /// <typeparam name="T">The type of the elements of the source.</typeparam>
    /// <param name="source">An IEnumerable to test.</param>
    /// <param name="predicate">A function to test each element for a condition.</param>
    /// <returns>
    ///     True if every element of the source sequence passes the test in the specified predicate, 
    ///     or if the sequence is empty; otherwise, false. Returns false if the source is null.
    /// </returns>
    /// <example>
    /// var numbers = new List&lt;int&gt; { 1, 2, 3, 4, 5 };
    /// bool allEven = numbers.SafeAll(n => n % 2 == 0); // Output: False
    ///
    /// var empty = new List&lt;int&gt;();
    /// bool allEven = numbers.SafeAll(n => n % 2 == 0); // Output: False
    /// </example>
    /// <exception cref="ArgumentNullException">If the predicate is null.</exception>
    public static bool SafeAll<T>(this IEnumerable<T> source, Func<T, bool> predicate)
    {
        ArgumentNullException.ThrowIfNull(predicate);
        
        if (source is null) return false;

        var itemCount = 0;

        foreach (var item in source)
        {
            if (!predicate(item))
                return false;
            itemCount++;
        }

        return itemCount > 0;
    }

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