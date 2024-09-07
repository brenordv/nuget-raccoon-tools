# List Extensions
The `ListExtensions` class provides several useful extension methods for working with lists and other enumerable collections.

## `SafeAll<T>`
Determines whether all elements of a sequence satisfy a condition safely.

**Parameters:**
- `source` (IEnumerable\<T\>): An enumerable to test.
- `predicate` (Func\<T, bool\>): A function to test each element for a condition.

**Returns:**
- bool: True if every element of the source sequence passes the test in the specified predicate. If source is empty or null, returns false.

**Example Usage:**
```csharp
var numbers = new List<int> { 1, 2, 3, 4, 5 };
bool allEven = numbers.SafeAll(n => n % 2 == 0); // Output: False

var emptyList = new List<int>();
bool allEvenEmpty = emptyList.SafeAll(n => n % 2 == 0); // Output: False

List<int> nullList = null;
bool allEvenNull = nullList.SafeAll(n => n % 2 == 0); // Output: False

var numbers2 = new List<int> { 2, 4, 6, 8 };
bool allEven2 = numbers2.SafeAll(n => n % 2 == 0); // Output: True
```

## `ForEachWithIndex<T>`
Returns an iterable list containing every item and its index.

**Parameters:**
- `source` (IEnumerable\<T\>): The target enumerable collection.

**Returns:**
- IEnumerable\<(int index, T item)\>: An enumerable containing tuples with the index and item.

**Example Usage:**
```csharp
var list = new List<string> { "a", "b", "c" };
foreach (var (index, item) in list.ForEachWithIndex())
{
    Console.WriteLine($"Index: {index}, Item: {item}");
}
```

## `ContainsCaseInsensitive`
Checks if the source contains the specified string, ignoring case.

**Parameters:**
- `source` (IEnumerable\<string\>): The source of strings to search.
- `containsText` (string): The string to search for.
- `nullValuesAreErrors` (bool): If true, null values in the source will be treated as errors and will not match the search string. If false, null values in the source will be ignored.

**Returns:**
- bool: True if the source contains the specified string (case-insensitive); otherwise, false.

**Example Usage:**
```csharp
var list = new List<string> { "Hello", "world" };
bool containsHello = list.ContainsCaseInsensitive("hello");
Console.WriteLine(containsHello); // Output: True
```

## `Replace<T>`
Replaces the first occurrence of an object in the source.

**Parameters:**
- `source` (IList\<T\>): The source list.
- `oldObj` (T): The old object to be replaced.
- `newObj` (T): The new object that will replace the old one.

**Returns:**
- bool: True if the object is replaced; false if the object is not found in the source.

**Example Usage:**
```csharp
var list = new List<int> { 1, 2, 3 };
bool replaced = list.Replace(2, 4);
Console.WriteLine(replaced); // Output: True
Console.WriteLine(string.Join(", ", list)); // Output: 1, 4, 3
```

## `HasElements<T>`
Determines whether the specified collection has any elements.

**Parameters:**
- `list` (ICollection\<T\>): The collection to check for elements.

**Returns:**
- bool: True if the collection is not null and contains one or more elements; otherwise, false.

**Example Usage:**
```csharp
var numbers = new List<int> { 1, 2, 3 };
bool hasElements = numbers.HasElements(); // Output: True

var emptyList = new List<int>();
bool hasElements = emptyList.HasElements(); // Output: False

List<string> nullList = null;
bool hasElements = nullList.HasElements(); // Output: False
```

## `Shuffle<T>`
Shuffles the list in place using the Fisher-Yates algorithm.

**Parameters:**
- `list` (IList\<T\>): The list to shuffle.

**Example Usage:**
```csharp
var numbers = new List<int> { 1, 2, 3, 4, 5 };
numbers.Shuffle();
// numbers is now shuffled, e.g., { 3, 1, 5, 2, 4 }
```

## `Random<T>`
Gets a random item from the list.

**Parameters:**
- `list` (IList\<T\>): The list to get a random item from.

**Returns:**
- T: A random item from the list.

**Example Usage:**
```csharp
var numbers = new List<int> { 1, 2, 3, 4, 5 };
int randomItem = numbers.Random();
// randomItem is now one of the elements in the list, e.g., 3
```

## `PopLast<T>`
Removes and returns the last item from the list.

**Parameters:**
- `list` (IList\<T\>): The list to remove the last item from.

**Returns:**
- T: The last item from the list.

**Example Usage:**
```csharp
var numbers = new List<int> { 1, 2, 3, 4, 5 };
int lastItem = numbers.PopLast();
// lastItem is now 5, and numbers is now { 1, 2, 3, 4 }
```

## `PopFirst<T>`
Removes and returns the first item from the list.

**Parameters:**
- `list` (IList\<T\>): The list to remove the first item from.

**Returns:**
- T: The first item from the list.

**Example Usage:**
```csharp
var numbers = new List<int> { 1, 2, 3, 4, 5 };
int firstItem = numbers.PopFirst();
// firstItem is now 1, and numbers is now { 2, 3, 4, 5 }
```

## `IndexOfMax<T>`
Gets the index of the maximum element in the list.

**Parameters:**
- `list` (IList\<T\>): The list to find the maximum element in.

**Returns:**
- int: The index of the maximum element in the list.

**Example Usage:**
```csharp
var numbers = new List<int> { 1, 3, 2, 5, 4 };
int maxIndex = numbers.IndexOfMax();
// maxIndex is now 3, as the maximum element is 5 at index 3
```

## `IndexOfMin<T>`
Gets the index of the minimum element in the list.

**Parameters:**
- `list` (IList\<T\>): The list to find the minimum element in.

**Returns:**
- int: The index of the minimum element in the list.

**Example Usage:**
```csharp
var numbers = new List<int> { 1, 3, 2, 5, 4 };
int minIndex = numbers.IndexOfMin();
// minIndex is now 0, as the minimum element is 1 at index 0
```

## `RemoveDuplicates<T>`
Removes duplicates from the list while preserving order.

**Parameters:**
- `list` (IList\<T\>): The list to remove duplicates from.

**Example Usage:**
```csharp
var numbers = new List<int> { 1, 2, 2, 3, 4, 4, 5 };
numbers.RemoveDuplicates();
// numbers is now { 1, 2, 3, 4, 5 }
```
