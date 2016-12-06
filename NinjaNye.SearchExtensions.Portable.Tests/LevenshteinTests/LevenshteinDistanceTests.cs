using NUnit.Framework;
using NinjaNye.SearchExtensions.Portable.Levenshtein;

namespace NinjaNye.SearchExtensions.Portable.Tests.LevenshteinTests
{
    [TestFixture]
    public class LevenshteinDistanceTests
    {
        [Test]
        public void LevenshteinDistance_DefaultBehaviour_DoesNotThrowAnException()
        {
            //Arrange
            
            //Act
            LevenshteinProcessor.LevenshteinDistance("", "");

            //Assert
            Assert.Pass("No exception thrown");
        }

        [Test]
        public void LevenshteinDistance_AcceptsTwoStrings_ReturnsUnsignedInteger()
        {
            //Arrange

            //Act
            var result = LevenshteinProcessor.LevenshteinDistance("string", "string");

            //Assert
            Assert.IsInstanceOf<int>(result);
        }

        [Test]
        public void LevenshteinDistance_FirstStringIsEmpty_ReturnLengthOfSecondString()
        {
            //Arrange

            //Act
            var result = LevenshteinProcessor.LevenshteinDistance(string.Empty, "string");

            //Assert
            Assert.AreEqual(6, result);
        }

        [Test]
        public void LevenshteinDistance_SecondStringIsEmpty_ReturnLengthOfFirstString()
        {
            //Arrange

            //Act
            var result = LevenshteinProcessor.LevenshteinDistance("test", string.Empty);

            //Assert
            Assert.AreEqual(4, result);
        }

        [Test]
        public void LevenshteinDistance_StringsAreEqual_ReturnZero()
        {
            //Arrange

            //Act
            var result = LevenshteinProcessor.LevenshteinDistance("test", "test");

            //Assert
            Assert.AreEqual(0, result);
        }

        [Test]
        public void LevenshteinDistance_BothStringsNull_ReturnZero()
        {
            //Arrange

            //Act
            var result = LevenshteinProcessor.LevenshteinDistance(null, null);

            //Assert
            Assert.AreEqual(0, result);
        }

        [Test]
        public void LevenshteinDistance_StringsDiffer_ReturnGreaterThanZero()
        {
            //Arrange

            //Act
            var result = LevenshteinProcessor.LevenshteinDistance("Ninja", "Nye");

            //Assert
            Assert.Greater(result, 0);
        }

        [Test]
        public void LevenshteinDistance_StringsAreEqualExceptForCase_ReturnZero()
        {
            //Arrange

            //Act
            var result = LevenshteinProcessor.LevenshteinDistance("test", "TEST");

            //Assert
            Assert.AreEqual(0, result);
        }

        [Test]
        public void LevenshteinDistance_StringsDifferByTwoCharacters_ReturnTwo()
        {
            //Arrange

            //Act
            var result = LevenshteinProcessor.LevenshteinDistance("Barry", "Lorry");

            //Assert
            Assert.AreEqual(2, result);
        }

        [Test]
        public void LevenshteinDistance_StringsDifferByTwoCharactersAndCasing_ReturnTwo()
        {
            //Arrange

            //Act
            var result = LevenshteinProcessor.LevenshteinDistance("Barry", "LORRY");

            //Assert
            Assert.AreEqual(2, result);
        }

        [Test]
        public void LevenshteinDistance_StringsDifferByNonLinearChanges_ReturnTwo()
        {
            //Arrange

            //Act
            var result = LevenshteinProcessor.LevenshteinDistance("house", "use");

            //Assert
            Assert.AreEqual(2, result);
        }
    }
}
