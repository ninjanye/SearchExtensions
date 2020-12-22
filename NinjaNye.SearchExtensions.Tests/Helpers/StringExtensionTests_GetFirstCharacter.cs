using NinjaNye.SearchExtensions.Soundex.Helpers;
using Xunit;

namespace NinjaNye.SearchExtensions.Tests.Helpers
{
    public class StringExtensionTests_GetFirstCharacter
    {
        [Fact]
        public void GetFirstCharacter_EmptyString_ReturnsNull()
        {
            //Arrange

            //Act
            var character = string.Empty.GetFirstCharacter();

            //Assert
            Assert.Null(character);
        }

        [Fact]
        public void GetFirstCharacter_ValidString_ReturnedValueIsNotNull()
        {
            //Arrange
            string word = "test";

            //Act
            var character = word.GetFirstCharacter();

            //Assert
            Assert.NotEmpty(character);
        }

        [Fact]
        public void GetFirstCharacter_ValidString_ReturnedValueIsFirstCharacterOnly()
        {
            //Arrange
            string word = "test";

            //Act
            var character = word.GetFirstCharacter();

            //Assert
            Assert.Equal(word[0].ToString(), character);
        }

        [Fact]
        public void GetFirstCharacter_StringIsWhitespaceOnly_ReturnNull()
        {
            //Arrange
            string word = "   ";

            //Act
            var character = word.GetFirstCharacter();

            //Assert
            Assert.Null(character);
        }

        [Fact]
        public void GetFirstCharacter_StringBeginsWithWhitespace_ReturnFirstNonWhitespaceCharacter()
        {
            //Arrange
            string word = " test";

            //Act
            var character = word.GetFirstCharacter();

            //Assert
            Assert.Equal("t", character);
        }
    }
}
