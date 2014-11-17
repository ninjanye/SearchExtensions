using System;
using System.Diagnostics;
using System.Linq;
using NUnit.Framework;
using NinjaNye.SearchExtensions.Levenshtein;

namespace NinjaNye.SearchExtensions.Tests.LevenshteinTests
{
    [TestFixture]
    public class LevenshteinDistancePerformanceTests : BuildStringTestsBase
    {
        [Test]
        public void ToLevenshteinDistance_CompareOneMillionStringsOfLength7_ExecutesInLessThanOneSecond()
        {
            //Arrange
            var words = this.BuildWords(1000000, 7, 7);
            string randomWord = this.BuildRandomWord(7,7);
            var stopwatch = new Stopwatch();
            //Act
            stopwatch.Start();
            var result = words.Select(w => LevenshteinProcessor.LevenshteinDistance(w, randomWord)).ToList();
            stopwatch.Stop();

            //Assert
            Console.WriteLine("Elapsed Time: {0}", stopwatch.Elapsed);
            Console.WriteLine("Total matching words: {0}", result.Count(i => i == 0));
            Console.WriteLine("Total words with distance of 1: {0}", result.Count(i => i == 1));
            Console.WriteLine("Total words with distance of 2: {0}", result.Count(i => i == 2));
            Console.WriteLine("Total words with distance of 3: {0}", result.Count(i => i == 3));
            Assert.IsTrue(stopwatch.Elapsed.TotalMilliseconds <= 1000);
        }
    }

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
