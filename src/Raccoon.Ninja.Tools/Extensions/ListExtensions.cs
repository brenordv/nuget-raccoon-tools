namespace Raccoon.Ninja.Tools.Extensions;

public static class ListExtensions
{
    private static readonly Random Rnd = new ();
    
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
    ///     Returns an iterable list containing every item AND its index.
    /// </summary>
    /// <param name="source">The target IEnumerable.</param>
    /// <typeparam name="T">The data type of the items in the source.</typeparam>
    /// <returns>IEnumerable containing tuples with index and item.</returns>
    /// <example>
    /// var fruits = new List&lt;string&gt; { "Apple", "Banana", "Cherry" };
    /// foreach (var (index, item) in fruits.ForEachWithIndex())
    /// {
    ///     Console.WriteLine($"Index: {index}, Item: {item}");
    /// }
    /// // Output:
    /// // Index: 0, Item: Apple
    /// // Index: 1, Item: Banana
    /// // Index: 2, Item: Cherry
    /// </example>
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
    /// <example>
    /// var words = new List&lt;string&gt; { "Hello", "world", "example" };
    /// bool containsHello = words.ContainsCaseInsensitive("hello"); // Output: True
    /// bool containsTest = words.ContainsCaseInsensitive("test"); // Output: False
    /// </example>
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
    /// <example>
    /// var numbers = new List&lt;int&gt; { 1, 2, 3, 4 };
    /// bool replaced = numbers.Replace(3, 5); // Output: True
    /// // numbers is now { 1, 2, 5, 4 }
    ///
    /// bool notReplaced = numbers.Replace(6, 7); // Output: False
    /// // numbers remains { 1, 2, 5, 4 }
    /// </example>
    public static bool Replace<T>(this IList<T> source, T oldObj, T newObj)
    {
        var index = source.IndexOf(oldObj);

        if (index == -1) return false;

        source[index] = newObj;

        return true;
    }

    /// <summary>
    ///     Determines whether the specified collection has any elements.
    /// For convenience, this method checks if the collection is not null
    /// first, so if it is, it will return false. 
    /// </summary>
    /// <typeparam name="T">The type of elements in the collection.</typeparam>
    /// <param name="list">The collection to check for elements.</param>
    /// <returns>
    ///     True if the collection is not null and contains one or more elements; otherwise, false.
    /// </returns>
    /// <example>
    /// var numbers = new List&lt;int&gt; { 1, 2, 3 };
    /// bool hasElements = numbers.HasElements(); // Output: True
    ///
    /// var emptyList = new List&lt;int&gt;();
    /// bool hasElements = emptyList.HasElements(); // Output: False
    ///
    /// List&lt;string&gt; nullList = null;
    /// bool hasElements = nullList.HasElements(); // Output: False
    /// </example>
    public static bool HasElements<T>(this ICollection<T> list)
    {
        return list is not null && list.Count > 0;
    }
    
    /// <summary>
    ///     Shuffles the list in place using the Fisher-Yates algorithm. 
    /// </summary>
    /// <remarks>This is done on the actual list. If you want to preserve the order, make a copy of the list before
    /// calling this method.</remarks>
    /// <typeparam name="T">The type of elements in the list.</typeparam>
    /// <param name="list">The list to shuffle.</param>
    /// <example>
    /// var numbers = new List&lt;int&gt; { 1, 2, 3, 4, 5 };
    /// numbers.Shuffle();
    /// // numbers is now shuffled, e.g., { 3, 1, 5, 2, 4 }
    /// </example>
    public static void Shuffle<T>(this IList<T> list)
    {
        var n = list.Count;
        while (n > 1)
        {
            n--;
            var k = Rnd.Next(n + 1);
            (list[k], list[n]) = (list[n], list[k]);
        }
    }

    /// <summary>
    ///     Gets a random item from the list.
    /// </summary>
    /// <typeparam name="T">The type of elements in the list.</typeparam>
    /// <param name="list">The list to get a random item from.</param>
    /// <returns>A random item from the list.</returns>
    /// <exception cref="ArgumentException">Thrown when the list is empty or null.</exception>
    /// <example>
    /// var numbers = new List&lt;int&gt; { 1, 2, 3, 4, 5 };
    /// int randomItem = numbers.Random();
    /// // randomItem is now one of the elements in the list, e.g., 3
    /// </example>
    public static T Random<T>(this IList<T> list)
    {
        EnsureListIsWorkable(list);

        return list[Rnd.Next(list.Count)];
    }


    /// <summary>
    ///     Removes and returns the last item from the list.
    /// </summary>
    /// <typeparam name="T">The type of elements in the list.</typeparam>
    /// <param name="list">The list to remove the last item from.</param>
    /// <returns>The last item from the list.</returns>
    /// <exception cref="ArgumentException">Thrown when the list is empty or null.</exception>
    /// <example>
    /// var numbers = new List&lt;int&gt; { 1, 2, 3, 4, 5 };
    /// int lastItem = numbers.PopLast();
    /// // lastItem is now 5, and numbers is now { 1, 2, 3, 4 }
    /// </example>
    public static T PopLast<T>(this IList<T> list)
    {
        return PopItem(list, false);
    }
    
    /// <summary>
    ///     Removes and returns the first item from the list.
    /// </summary>
    /// <typeparam name="T">The type of elements in the list.</typeparam>
    /// <param name="list">The list to remove the first item from.</param>
    /// <returns>The first item from the list.</returns>
    /// <exception cref="ArgumentException">Thrown when the list is empty or null.</exception>
    /// <example>
    /// var numbers = new List&lt;int&gt; { 1, 2, 3, 4, 5 };
    /// int firstItem = numbers.PopFirst();
    /// // firstItem is now 1, and numbers is now { 2, 3, 4, 5 }
    /// </example>
    public static T PopFirst<T>(this IList<T> list)
    {
        return PopItem(list, true);
    }

    /// <summary>
    ///     Gets the index of the maximum element in the list.
    /// </summary>
    /// <typeparam name="T">The type of elements in the list.</typeparam>
    /// <param name="list">The list to find the maximum element in.</param>
    /// <returns>The index of the maximum element in the list.</returns>
    /// <exception cref="ArgumentException">Thrown when the list is empty or null.</exception>
    /// <example>
    /// var numbers = new List&lt;int&gt; { 1, 3, 2, 5, 4 };
    /// int maxIndex = numbers.IndexOfMax();
    /// // maxIndex is now 3, as the maximum element is 5 at index 3
    /// </example>
    public static int IndexOfMax<T>(this IList<T> list) where T : IComparable<T>
    {
        return IndexOfBoundary(list, true);
    }

    /// <summary>
    ///     Gets the index of the minimum element in the list.
    /// </summary>
    /// <typeparam name="T">The type of elements in the list.</typeparam>
    /// <param name="list">The list to find the minimum element in.</param>
    /// <returns>The index of the minimum element in the list.</returns>
    /// <exception cref="ArgumentException">Thrown when the list is empty or null.</exception>
    /// <example>
    /// var numbers = new List&lt;int&gt; { 1, 3, 2, 5, 4 };
    /// int minIndex = numbers.IndexOfMin();
    /// // minIndex is now 0, as the minimum element is 1 at index 0
    /// </example>
    public static int IndexOfMin<T>(this IList<T> list) where T : IComparable<T>
    {
        return IndexOfBoundary(list, false);
    }

    /// <summary>
    ///     Removes duplicates from the list while preserving order.
    /// </summary>
    /// <typeparam name="T">The type of elements in the list.</typeparam>
    /// <param name="list">The list to remove duplicates from.</param>
    /// <example>
    /// var numbers = new List&lt;int&gt; { 1, 2, 2, 3, 4, 4, 5 };
    /// numbers.RemoveDuplicates();
    /// // numbers is now { 1, 2, 3, 4, 5 }
    /// </example>
    public static void RemoveDuplicates<T>(this IList<T> list)
    {
        ArgumentNullException.ThrowIfNull(list);

        if (list.Count == 0) return;

        var seen = new HashSet<T>();
        for (var i = list.Count - 1; i >= 0; i--)
        {
            if (seen.Add(list[i])) continue;

            list.RemoveAt(i);
        }
    }
    
    #region Auxiliary Methods

    /// <summary>
    /// Actual logic for <see cref="PopFirst{T}"/> and <see cref="PopLast{T}"/>.
    /// </summary>
    /// <param name="list">The list to remove the last item from.</param>
    /// <param name="first">If true, will pop the first item. Otherwise, it will be the last.</param>
    /// <typeparam name="T">The type of elements in the list.</typeparam>
    /// <returns>The popped item from the list.</returns>
    /// <exception cref="ArgumentException">Thrown when the list is empty or null.</exception>
    private static T PopItem<T>(IList<T> list, bool first)
    {
        EnsureListIsWorkable(list);

        if (first)
        {
            var firstItem = list[0];
            list.RemoveAt(0);
            return firstItem;
        }

        var lastIndex = list.Count - 1;
        var lastItem = list[lastIndex];
        list.RemoveAt(lastIndex);
        return lastItem;
    }

    /// <summary>
    /// Actual logic for <see cref="IndexOfMax{T}"/> and <see cref="IndexOfMin{T}"/>.
    /// </summary>
    /// <typeparam name="T">The type of elements in the list.</typeparam>
    /// <param name="list">The list to find the maximum element in.</param>
    /// <param name="max">If true, will get the index of the max item. Otherwise, will return from the min item.</param>
    /// <returns>The index of the maximum element in the list.</returns>
    /// <exception cref="ArgumentException">Thrown when the list is empty or null.</exception>
    private static int IndexOfBoundary<T>(IList<T> list, bool max) where T : IComparable<T>
    {
        EnsureListIsWorkable(list);

        var targetIndex = 0;
        for (var i = 1; i < list.Count; i++)
        {
            switch (max)
            {
                case false when list[i].CompareTo(list[targetIndex]) < 0:
                    targetIndex = i;
                    continue;
                case true when list[i].CompareTo(list[targetIndex]) > 0:
                    targetIndex = i;
                    break;
            }
        }
        return targetIndex;
    }

    /// <summary>
    ///  Ensures that the list is not null or empty.
    /// </summary>
    /// <param name="list">List to be checked</param>
    /// <typeparam name="T">Type of the objects in the list</typeparam>
    /// <exception cref="ArgumentException">Thrown if the list is null or empty.</exception>
    private static void EnsureListIsWorkable<T>(ICollection<T> list)
    {
        if (list.HasElements()) return;
        throw new ArgumentException("The list is empty or null.");
    }
    #endregion
}