using FluentAssertions;
using Raccoon.Ninja.Tools.Extensions;

namespace Raccoon.Ninja.Tools.Tests.Extensions;

public class StringExtensionsTests
{
        [Fact]
        public void Minify_Success()
        {
            // Arrange
            const string text = """
                                this  is  some      really      big      string
                                that  spans  through 
                                multiple 
                                lines      
                                """;

            // Act
            var result = text.Minify();
            
            // Assert
            result.Should().Be("this is some really big string that spans through multiple lines");
        }

        [Fact]
        public void Minify_Success_AlreadyMinified()
        {
            // Arrange
            const string text = "a small text";

            // Act
            var result = text.Minify();
            
            // Assert
            result.Should().Be("a small text");
        }

        [Theory]
        [MemberData(nameof(GetNullEmptyOrWhiteSpaceStrings))]
        public void Minify_Success_Empty(string text)
        {
            // Arrange, Act & Assert
            text.Minify().Should().Be(text);
        }

        [Theory]
        [MemberData(nameof(GetStringsWithAndWithoutAccent))]
        public void StripAccents_Success(string accented, string stripped)
        {
            // Arrange, Act & Assert
            accented.StripAccents().Should().Be(stripped);
            accented.ToUpper().StripAccents().Should().Be(stripped.ToUpper());
        }

        [Theory]
        [MemberData(nameof(GetJumbledStringsWithDigits))]
        public void OnlyDigits_Success(string value, string expected)
        {
            // Arrange, Act & Assert
            value.OnlyDigits().Should().Be(expected);
        }

        #region Test Helpers
        public static TheoryData<string> GetNullEmptyOrWhiteSpaceStrings()
        {
            return new TheoryData<string>
            {
                null,
                "",
                "    ",
                "  ",
                " ",
            };
        }

        public static TheoryData<string, string> GetStringsWithAndWithoutAccent()
        {
            var dataSet = new TheoryData<string, string>();
            foreach (var (accented, stripped) in GetAccents())
            {
                dataSet.Add(accented, stripped);
            }

            return dataSet;
        }

        public static TheoryData<string, string> GetJumbledStringsWithDigits()
        {
            return new TheoryData<string, string>
            {
                {"A1", "1"},
                {"    A1", "1"},
                {"A   1", "1"},
                {"A1    ", "1"},
                {"1", "1"},
                {"...", ""},
                {" ", ""},
                {"ÇÁ^;/~3$#@%¨&*()!?-+§°", "3"}
            };
        }
        
        private static IEnumerable<(string, string)> GetAccents()
        {
            yield return ("á", "a");
            yield return ("ã", "a");
            yield return ("à", "a");
            yield return ("â", "a");
            yield return ("ä", "a");
            yield return ("a", "a");
            yield return ("é", "e");
            yield return ("è", "e");
            yield return ("ê", "e");
            yield return ("ë", "e");
            yield return ("e", "e");
            yield return ("í", "i");
            yield return ("ì", "i");
            yield return ("ï", "i");
            yield return ("î", "i");
            yield return ("i", "i");
            yield return ("ó", "o");
            yield return ("ò", "o");
            yield return ("õ", "o");
            yield return ("ö", "o");
            yield return ("ô", "o");
            yield return ("o", "o");
            yield return ("ú", "u");
            yield return ("ù", "u");
            yield return ("ü", "u");
            yield return ("û", "u");
            yield return ("u", "u");
            yield return ("ÿ", "y");
            yield return ("ç", "c");
        }
        #endregion
}