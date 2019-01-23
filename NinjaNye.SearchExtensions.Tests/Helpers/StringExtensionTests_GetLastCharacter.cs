using NinjaNye.SearchExtensions.Soundex.Helpers;
using Xunit;

namespace NinjaNye.SearchExtensions.Tests.Helpers
{
    
    public class StringExtensionTests_GetLastCharacter
    {
        [Fact]
        public void GetLastCharacter_EmptyString_ReturnsNull()
        {
            //Arrange

            //Act
            var character = string.Empty.GetLastCharacter();

            //Assert
            Assert.Null(character);
        }

        [Fact]
        public void GetLastCharacter_ValidString_ReturnedValueIsNotNull()
        {
            //Arrange
            string word = "test";

            //Act
            var character = word.GetLastCharacter();

            //Assert
            Assert.NotEmpty(character);
        }

        [Fact]
        public void GetLastCharacter_ValidString_ReturnedValueIsLastCharacterOnly()
        {
            //Arrange
            string word = "test";

            //Act
            var character = word.GetLastCharacter();

            //Assert
            Assert.Equal(word[word.Length - 1].ToString(), character);
        }

        [Fact]
        public void GetLastCharacter_StringIsWhitespaceOnly_ReturnNull()
        {
            //Arrange
            string word = "   ";

            //Act
            var character = word.GetLastCharacter();

            //Assert
            Assert.Null(character);
        }

        [Fact]
        public void GetLastCharacter_StringEndsWithWhitespace_ReturnLastNonWhitespaceCharacter()
        {
            //Arrange
            string word = "testing ";

            //Act
            var character = word.GetLastCharacter();

            //Assert
            Assert.Equal("g", character);
        }
    }
}
