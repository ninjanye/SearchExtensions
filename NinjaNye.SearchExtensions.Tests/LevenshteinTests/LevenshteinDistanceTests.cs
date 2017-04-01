using Xunit;
using NinjaNye.SearchExtensions.Levenshtein;

namespace NinjaNye.SearchExtensions.Tests.LevenshteinTests
{
    public class LevenshteinDistanceTests
    {
        [Fact]
        public void LevenshteinDistance_DefaultBehaviour_DoesNotThrowAnException()
        {
            //Arrange
            
            //Act
            LevenshteinProcessor.LevenshteinDistance("", "");

            //Assert
            Assert.True(true, "No exception thrown");
        }

        [Fact]
        public void LevenshteinDistance_AcceptsTwoStrings_ReturnsUnsignedInteger()
        {
            //Arrange

            //Act
            var result = LevenshteinProcessor.LevenshteinDistance("string", "string");

            //Assert
            Assert.IsType<int>(result);
        }

        [Fact]
        public void LevenshteinDistance_FirstStringIsEmpty_ReturnLengthOfSecondString()
        {
            //Arrange

            //Act
            var result = LevenshteinProcessor.LevenshteinDistance(string.Empty, "string");

            //Assert
            Assert.Equal(6, result);
        }

        [Fact]
        public void LevenshteinDistance_SecondStringIsEmpty_ReturnLengthOfFirstString()
        {
            //Arrange

            //Act
            var result = LevenshteinProcessor.LevenshteinDistance("test", string.Empty);

            //Assert
            Assert.Equal(4, result);
        }

        [Fact]
        public void LevenshteinDistance_StringsAreEqual_ReturnZero()
        {
            //Arrange

            //Act
            var result = LevenshteinProcessor.LevenshteinDistance("test", "test");

            //Assert
            Assert.Equal(0, result);
        }

        [Fact]
        public void LevenshteinDistance_BothStringsNull_ReturnZero()
        {
            //Arrange

            //Act
            var result = LevenshteinProcessor.LevenshteinDistance(null, null);

            //Assert
            Assert.Equal(0, result);
        }

        [Fact]
        public void LevenshteinDistance_StringsDiffer_ReturnGreaterThanZero()
        {
            //Arrange

            //Act
            var result = LevenshteinProcessor.LevenshteinDistance("Ninja", "Nye");

            //Assert
            Assert.True(result > 0);
        }

        [Fact]
        public void LevenshteinDistance_StringsAreEqualExceptForCase_ReturnZero()
        {
            //Arrange

            //Act
            var result = LevenshteinProcessor.LevenshteinDistance("test", "TEST");

            //Assert
            Assert.Equal(0, result);
        }

        [Fact]
        public void LevenshteinDistance_StringsDifferByTwoCharacters_ReturnTwo()
        {
            //Arrange

            //Act
            var result = LevenshteinProcessor.LevenshteinDistance("Barry", "Lorry");

            //Assert
            Assert.Equal(2, result);
        }

        [Fact]
        public void LevenshteinDistance_StringsDifferByTwoCharactersAndCasing_ReturnTwo()
        {
            //Arrange

            //Act
            var result = LevenshteinProcessor.LevenshteinDistance("Barry", "LORRY");

            //Assert
            Assert.Equal(2, result);
        }

        [Fact]
        public void LevenshteinDistance_StringsDifferByNonLinearChanges_ReturnTwo()
        {
            //Arrange

            //Act
            var result = LevenshteinProcessor.LevenshteinDistance("house", "use");

            //Assert
            Assert.Equal(2, result);
        }
    }
}
