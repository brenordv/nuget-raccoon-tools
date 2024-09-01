using FluentAssertions;
using Raccoon.Ninja.Tools.Extensions;

namespace Raccoon.Ninja.Tools.Tests.Extensions;

public class ListExtensionsTests
{
    #region SafeAll

    [Fact]
    public void SafeAll_NullSource_ReturnsFalse()
    {
        // Arrange
        IEnumerable<int> source = null;

        // Act
        var result = source.SafeAll(x => true);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void SafeAll_EmptySource_ReturnsFalse()
    {
        // Arrange
        var source = new List<int>();

        // Act
        var result = source.SafeAll(x => true);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void SafeAll_AllElementsSatisfyCondition_ReturnsTrue()
    {
        // Arrange
        var source = new List<int> { 2, 4, 6, 8 };

        // Act
        var result = source.SafeAll(x => x % 2 == 0);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void SafeAll_NotAllElementsSatisfyCondition_ReturnsFalse()
    {
        // Arrange
        var source = new List<int> { 2, 4, 5, 8 };

        // Act
        var result = source.SafeAll(x => x % 2 == 0);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void SafeAll_SingleElementSatisfiesCondition_ReturnsTrue()
    {
        // Arrange
        var source = new List<int> { 2 };

        // Act
        var result = source.SafeAll(x => x % 2 == 0);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void SafeAll_SingleElementDoesNotSatisfyCondition_ReturnsFalse()
    {
        // Arrange
        var source = new List<int> { 1 };

        // Act
        var result = source.SafeAll(x => x % 2 == 0);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void SafeAll_NullPredicate_ThrowsArgumentNullException()
    {
        // Arrange
        var source = new List<int> { 1, 2, 3 };

        // Act & Assert
        source.Invoking(s => s.SafeAll(null))
            .Should().Throw<ArgumentNullException>();
    }

    #endregion

    #region ForEachWithIndex

    [Fact]
    public void ForEachWithIndex_EmptyList_ShouldReturnEmptyEnumerable()
    {
        // Arrange
        var emptyList = new List<string>();

        // Act
        var result = emptyList.ForEachWithIndex();

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public void ForEachWithIndex_SingleItemList_ShouldReturnCorrectIndexAndItem()
    {
        // Arrange
        var singleItemList = new List<string> { "Test" };

        // Act
        var result = singleItemList.ForEachWithIndex().ToList();

        // Assert
        result.Should().HaveCount(1);
        result[0].index.Should().Be(0);
        result[0].item.Should().Be("Test");
    }

    [Theory]
    [MemberData(nameof(GetValidArgumentsForEachWithIndex))]
    public void ForEachWithIndex_MultipleItemList_ShouldReturnCorrectIndexesAndItems(IList<object> items)
    {
        // Arrange
        var expectedCount = items.Count;

        // Act
        var result = items.ForEachWithIndex().ToList();

        // Assert
        result.Should().HaveCount(expectedCount);
        for (var i = 0; i < expectedCount; i++)
        {
            result[i].index.Should().Be(i);
            result[i].item.Should().Be(items[i]);
        }
    }

    [Fact]
    public void ForEachWithIndex_NullSource_ShouldThrowArgumentNullException()
    {
        // Arrange
        IEnumerable<string> nullList = null;

        // Act & Assert
        var action = () => nullList.ForEachWithIndex().ToList();
        action.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void ForEachWithIndex_LargeList_ShouldReturnCorrectIndexesAndItems()
    {
        // Arrange
        var largeList = Enumerable.Range(0, 1000).Select(i => i.ToString()).ToList();

        // Act
        var result = largeList.ForEachWithIndex().ToList();

        // Assert
        result.Should().HaveCount(1000);
        for (var i = 0; i < 1000; i++)
        {
            result[i].index.Should().Be(i);
            result[i].item.Should().Be(i.ToString());
        }
    }

    #endregion

    #region ContainsCaseInsensitive

    [Fact]
    public void ContainsCaseInsensitive_NullSource_ShouldReturnFalse()
    {
        IEnumerable<string> source = null;
        var result = source.ContainsCaseInsensitive("test");
        result.Should().BeFalse();
    }

    [Fact]
    public void ContainsCaseInsensitive_EmptySource_ShouldReturnFalse()
    {
        var source = new List<string>();
        var result = source.ContainsCaseInsensitive("test");
        result.Should().BeFalse();
    }

    [Fact]
    public void ContainsCaseInsensitive_NullOrEmptyContainsText_ShouldReturnFalse()
    {
        var source = new List<string> { "test" };
        source.ContainsCaseInsensitive(null).Should().BeFalse();
        source.ContainsCaseInsensitive("").Should().BeFalse();
        source.ContainsCaseInsensitive("  ").Should().BeFalse();
    }

    [Theory]
    [InlineData("test", "TEST")]
    [InlineData("TEST", "test")]
    [InlineData("TeSt", "tEsT")]
    public void ContainsCaseInsensitive_MatchingTextWithDifferentCase_ShouldReturnTrue(string sourceText,
        string searchText)
    {
        var source = new List<string> { sourceText };
        var result = source.ContainsCaseInsensitive(searchText);
        result.Should().BeTrue();
    }

    [Fact]
    public void ContainsCaseInsensitive_NonMatchingText_ShouldReturnFalse()
    {
        var source = new List<string> { "test", "example", "sample" };
        var result = source.ContainsCaseInsensitive("notfound");
        result.Should().BeFalse();
    }

    [Fact]
    public void ContainsCaseInsensitive_PartialMatch_ShouldReturnFalse()
    {
        var source = new List<string> { "testing", "example", "sample" };
        var result = source.ContainsCaseInsensitive("test");
        result.Should().BeFalse();
    }

    [Fact]
    public void ContainsCaseInsensitive_NullValueInSource_NullValuesAreErrors_ShouldThrowNullReferenceException()
    {
        var source = new List<string> { "test", null, "sample" };
        var act = () => source.ContainsCaseInsensitive("sample", nullValuesAreErrors: true);
        act.Should().Throw<NullReferenceException>();
    }

    [Fact]
    public void ContainsCaseInsensitive_NullValueInSource_NullValuesNotErrors_ShouldNotThrowAndReturnTrue()
    {
        var source = new List<string> { "test", null, "sample" };
        var result = source.ContainsCaseInsensitive("sample", nullValuesAreErrors: false);
        result.Should().BeTrue();
    }

    [Fact]
    public void ContainsCaseInsensitive_NullValueInSource_NullValuesNotErrors_NonMatchingSearch_ShouldReturnFalse()
    {
        var source = new List<string> { "test", null, "sample" };
        var result = source.ContainsCaseInsensitive("notfound", nullValuesAreErrors: false);
        result.Should().BeFalse();
    }

    [Fact]
    public void ContainsCaseInsensitive_EmptyStringInSource_ShouldNotMatch()
    {
        var source = new List<string> { "test", "", "sample" };
        var result = source.ContainsCaseInsensitive("");
        result.Should().BeFalse();
    }

    #endregion

    #region Replace

    [Fact]
    public void Replace_NullSource_ShouldThrowArgumentNullException()
    {
        IList<string> source = null;
        var action = () => source.Replace("old", "new");
        action.Should().Throw<NullReferenceException>();
    }

    [Fact]
    public void Replace_EmptySource_ShouldReturnFalse()
    {
        var source = new List<string>();
        var result = source.Replace("old", "new");
        result.Should().BeFalse();
        source.Should().BeEmpty();
    }

    [Fact]
    public void Replace_ObjectNotFound_ShouldReturnFalse()
    {
        var source = new List<string> { "one", "two", "three" };
        var result = source.Replace("four", "new");
        result.Should().BeFalse();
        source.Should().Equal("one", "two", "three");
    }

    [Fact]
    public void Replace_ObjectFound_ShouldReplaceAndReturnTrue()
    {
        var source = new List<string> { "one", "two", "three" };
        var result = source.Replace("two", "new");
        result.Should().BeTrue();
        source.Should().Equal("one", "new", "three");
    }

    [Fact]
    public void Replace_MultipleOccurrences_ShouldReplaceOnlyFirst()
    {
        var source = new List<string> { "one", "two", "three", "two" };
        var result = source.Replace("two", "new");
        result.Should().BeTrue();
        source.Should().Equal("one", "new", "three", "two");
    }

    [Fact]
    public void Replace_WithNull_ShouldWork()
    {
        var source = new List<string> { "one", "two", "three" };
        var result = source.Replace("two", null);
        result.Should().BeTrue();
        source.Should().Equal("one", null, "three");
    }

    [Fact]
    public void Replace_ReplaceNull_ShouldWork()
    {
        var source = new List<string> { "one", null, "three" };
        var result = source.Replace(null, "new");
        result.Should().BeTrue();
        source.Should().Equal("one", "new", "three");
    }

    [Fact]
    public void Replace_WithSameObject_ShouldReturnTrueButNotModifyList()
    {
        var source = new List<string> { "one", "two", "three" };
        var result = source.Replace("two", "two");
        result.Should().BeTrue();
        source.Should().Equal("one", "two", "three");
    }

    [Fact]
    public void Replace_WithCustomType_ShouldWork()
    {
        var obj1 = new CustomType { Id = 1, Name = "One" };
        var obj2 = new CustomType { Id = 2, Name = "Two" };
        var obj3 = new CustomType { Id = 3, Name = "Three" };
        var newObj = new CustomType { Id = 4, Name = "New" };

        var source = new List<CustomType> { obj1, obj2, obj3 };
        var result = source.Replace(obj2, newObj);
        result.Should().BeTrue();
        source.Should().Equal(obj1, newObj, obj3);
    }

    #endregion

    #region HasElements

    [Fact]
    public void HasElements_NonEmptyList_ReturnsTrue()
    {
        // Arrange
        var list = new List<int> { 1, 2, 3 };

        // Act
        var result = list.HasElements();

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void HasElements_EmptyList_ReturnsFalse()
    {
        // Arrange
        var list = new List<int>();

        // Act
        var result = list.HasElements();

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void HasElements_NullList_ReturnsFalse()
    {
        // Arrange
        List<int> list = null;

        // Act
        var result = list.HasElements();

        // Assert
        result.Should().BeFalse();
    }

    #endregion

    #region Shuffle

    [Fact]
    public void Shuffle_List_ShouldShuffleItems()
    {
        // Arrange
        var list = new List<int> { 1, 2, 3, 4, 5 };
        var originalList = new List<int>(list);

        // Act
        list.Shuffle();

        // Assert
        list.Should().NotEqual(originalList);
        list.Should().HaveCount(originalList.Count);
        list.Should().Contain(originalList);
    }

    #endregion

    #region Random

    [Fact]
    public void Random_List_ShouldReturnRandomItem()
    {
        // Arrange
        var list = new List<int> { 1, 2, 3, 4, 5 };

        // Act
        var randomItem = list.Random();

        // Assert
        list.Should().Contain(randomItem);
    }

    [Fact]
    public void Random_EmptyList_ShouldThrowArgumentException()
    {
        // Arrange
        var list = new List<int>();

        // Act
        var act = () => list.Random();

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("The list is empty or null.");
    }

    [Fact]
    public void Random_NullList_ShouldThrowArgumentException()
    {
        // Arrange
        List<int> list = null;

        // Act
        var act = () => list.Random();

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("The list is empty or null.");
    }

    [Fact]
    public void Random_SingleElementList_ShouldReturnThatElement()
    {
        // Arrange
        var list = new List<int> { 42 };

        // Act
        var randomItem = list.Random();

        // Assert
        randomItem.Should().Be(42);
    }

    #endregion

    #region PopLast

    [Fact]
    public void PopLast_List_ShouldReturnAndRemoveLastItem()
    {
        // Arrange
        var list = new List<int> { 1, 2, 3, 4, 5 };

        // Act
        var lastItem = list.PopLast();

        // Assert
        lastItem.Should().Be(5);
        list.Should().Equal(1, 2, 3, 4);
    }

    [Fact]
    public void PopLast_EmptyList_ShouldThrowArgumentException()
    {
        // Arrange
        var list = new List<int>();

        // Act
        var act = () => list.PopLast();

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("The list is empty or null.");
    }

    [Fact]
    public void PopLast_SingleElementList_ShouldReturnAndRemoveElement()
    {
        // Arrange
        var list = new List<int> { 42 };

        // Act
        var lastItem = list.PopLast();

        // Assert
        lastItem.Should().Be(42);
        list.Should().BeEmpty();
    }

    [Fact]
    public void PopLast_NullList_ShouldThrowArgumentException()
    {
        // Arrange
        List<int> list = null;

        // Act
        var act = () => list.PopLast();

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("The list is empty or null.");
    }

    #endregion

    #region PopFirst
    [Fact]
    public void PopFirst_List_ShouldReturnAndRemoveFirstItem()
    {
        // Arrange
        var list = new List<int> { 1, 2, 3, 4, 5 };

        // Act
        var firstItem = list.PopFirst();

        // Assert
        firstItem.Should().Be(1);
        list.Should().Equal(2, 3, 4, 5);
    }

    [Fact]
    public void PopFirst_EmptyList_ShouldThrowArgumentException()
    {
        // Arrange
        var list = new List<int>();

        // Act
        var act = () => list.PopFirst();

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("The list is empty or null.");
    }

    [Fact]
    public void PopFirst_SingleElementList_ShouldReturnAndRemoveElement()
    {
        // Arrange
        var list = new List<int> { 42 };

        // Act
        var firstItem = list.PopFirst();

        // Assert
        firstItem.Should().Be(42);
        list.Should().BeEmpty();
    }

    [Fact]
    public void PopFirst_NullList_ShouldThrowArgumentException()
    {
        // Arrange
        List<int> list = null;

        // Act
        var act = () => list.PopFirst();

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("The list is empty or null.");
    }
    #endregion
 
    #region IndexOfMax
    [Fact]
    public void IndexOfMax_List_ShouldReturnIndexOfMaxItem()
    {
        // Arrange
        var list = new List<int> { 1, 3, 2, 5, 4 };

        // Act
        var maxIndex = list.IndexOfMax();

        // Assert
        maxIndex.Should().Be(3);
    }
    
    [Fact]
    public void IndexOfMax_NullList_ShouldThrowArgumentException()
    {
        // Arrange
        List<int> list = null;

        // Act
        var act = () => list.IndexOfMax();

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("The list is empty or null.");
    }

    [Fact]
    public void IndexOfMax_EmptyList_ShouldThrowArgumentException()
    {
        // Arrange
        var list = new List<int>();

        // Act
        var act = () => list.IndexOfMax();

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("The list is empty or null.");
    }

    [Fact]
    public void IndexOfMax_SingleElementList_ShouldReturnZero()
    {
        // Arrange
        var list = new List<int> { 42 };

        // Act
        var result = list.IndexOfMax();

        // Assert
        result.Should().Be(0);
    }

    [Fact]
    public void IndexOfMax_MultipleElementsList_ShouldReturnIndexOfMaxItem()
    {
        // Arrange
        var list = new List<int> { 1, 3, 2, 5, 4 };

        // Act
        var result = list.IndexOfMax();

        // Assert
        result.Should().Be(3);
    }

    [Fact]
    public void IndexOfMax_MultipleElementsWithDuplicateMaxValues_ShouldReturnFirstIndexOfMaxItem()
    {
        // Arrange
        var list = new List<int> { 1, 5, 3, 5, 2 };

        // Act
        var result = list.IndexOfMax();

        // Assert
        result.Should().Be(1);
    }
    #endregion

    #region IndexOfMin
    [Fact]
    public void IndexOfMin_List_ShouldReturnIndexOfMinItem()
    {
        // Arrange
        var list = new List<int> { 1, 3, 2, 5, 4 };

        // Act
        var minIndex = list.IndexOfMin();

        // Assert
        minIndex.Should().Be(0);
    }

    [Fact]
    public void IndexOfMin_EmptyList_ShouldThrowArgumentException()
    {
        // Arrange
        var list = new List<int>();

        // Act
        var act = () => list.IndexOfMin();

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("The list is empty or null.");
    }

    [Fact]
    public void IndexOfMin_SingleElementList_ShouldReturnZero()
    {
        // Arrange
        var list = new List<int> { 42 };

        // Act
        var minIndex = list.IndexOfMin();

        // Assert
        minIndex.Should().Be(0);
    }

    [Fact]
    public void IndexOfMin_NullList_ShouldThrowArgumentException()
    {
        // Arrange
        List<int> list = null;

        // Act
        var act = () => list.IndexOfMin();

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("The list is empty or null.");
    }

    [Fact]
    public void IndexOfMin_MultipleElementsList_ShouldReturnIndexOfMinItem()
    {
        // Arrange
        var list = new List<int> { 3, 1, 4, 1, 5 };

        // Act
        var minIndex = list.IndexOfMin();

        // Assert
        minIndex.Should().Be(1);
    }

    [Fact]
    public void IndexOfMin_MultipleElementsWithDuplicateMinValues_ShouldReturnFirstIndexOfMinItem()
    {
        // Arrange
        var list = new List<int> { 3, 1, 4, 1, 5, 1 };

        // Act
        var minIndex = list.IndexOfMin();

        // Assert
        minIndex.Should().Be(1);
    }
    #endregion

    #region RemoveDuplicates
    [Fact]
    public void RemoveDuplicates_List_ShouldRemoveDuplicateItems()
    {
        // Arrange
        var list = new List<int> { 1, 2, 2, 3, 4, 4, 5 };

        // Act
        list.RemoveDuplicates();

        // Assert
        list.Should().Equal(1, 2, 3, 4, 5);
    }

    [Fact]
    public void RemoveDuplicates_EmptyList_ShouldRemainEmpty()
    {
        // Arrange
        var list = new List<int>();

        // Act
        list.RemoveDuplicates();

        // Assert
        list.Should().BeEmpty();
    }

    [Fact]
    public void RemoveDuplicates_SingleElementList_ShouldRemainUnchanged()
    {
        // Arrange
        var list = new List<int> { 1 };

        // Act
        list.RemoveDuplicates();

        // Assert
        list.Should().Equal(1);
    }

    [Fact]
    public void RemoveDuplicates_ListWithNoDuplicates_ShouldRemainUnchanged()
    {
        // Arrange
        var list = new List<int> { 1, 2, 3, 4, 5 };

        // Act
        list.RemoveDuplicates();

        // Assert
        list.Should().Equal(1, 2, 3, 4, 5);
    }

    [Fact]
    public void RemoveDuplicates_ListWithConsecutiveDuplicates_ShouldRemoveDuplicates()
    {
        // Arrange
        var list = new List<int> { 1, 1, 2, 2, 3, 3, 4, 4, 5, 5 };

        // Act
        list.RemoveDuplicates();

        // Assert
        list.Should().Equal(1, 2, 3, 4, 5);
    }

    [Fact]
    public void RemoveDuplicates_ListWithNonConsecutiveDuplicates_ShouldRemoveDuplicates()
    {
        // Arrange
        var list = new List<int> { 1, 2, 1, 3, 2, 4, 3, 5, 4, 5 };

        // Act
        list.RemoveDuplicates();

        // Assert
        list.Should().Equal(1, 2, 3, 4, 5);
    }

    [Fact]
    public void RemoveDuplicates_ListWithAllElementsSame_ShouldRemoveAllButOne()
    {
        // Arrange
        var list = new List<int> { 1, 1, 1, 1, 1 };

        // Act
        list.RemoveDuplicates();

        // Assert
        list.Should().Equal(1);
    }

    [Fact]
    public void RemoveDuplicates_ListWithNullValues_ShouldRemoveDuplicates()
    {
        // Arrange
        var list = new List<string> { "a", "b", null, "a", null, "b", "c" };

        // Act
        list.RemoveDuplicates();

        // Assert
        list.Should().Equal("a", null, "b", "c");
    }
    #endregion

    #region Test Helpers

    public static TheoryData<IList<object>> GetValidArgumentsForEachWithIndex()
    {
        return
        [
            new object[] { 10, 20, 30 },
            new object[] { 42.2, 33.5, 11.2 },
            new object[] { "Foo", "Bar", "FooBar" }
        ];
    }

    private class CustomType
    {
        public int Id { get; init; }
        public string Name { get; init; }

        public override bool Equals(object obj)
        {
            return obj is CustomType type &&
                   Id == type.Id &&
                   Name == type.Name;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Name);
        }
    }

    #endregion
}