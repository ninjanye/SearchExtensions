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
        #if DEBUG
        // Performance test will always fail in debug mode
        [Ignore]
    	#endif
        [Test]
        public void ToLevenshteinDistance_CompareOneMillionStringsOfLength7_ExecutesInLessThanOneSecond()
        {
            //Arrange
            var words = this.BuildWords(1000000, 7, 7);
            var randomWord = this.BuildRandomWord(7,7);
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
}