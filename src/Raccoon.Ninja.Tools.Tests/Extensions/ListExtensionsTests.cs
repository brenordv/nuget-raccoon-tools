using FluentAssertions;
using Raccoon.Ninja.Tools.Extensions;

namespace Raccoon.Ninja.Tools.Tests.Extensions;

public class ListExtensionsTests
{
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
    public void ContainsCaseInsensitive_MatchingTextWithDifferentCase_ShouldReturnTrue(string sourceText, string searchText)
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
        Action act = () => source.ContainsCaseInsensitive("sample", nullValuesAreErrors: true);
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