using System.Diagnostics;
using System.Linq;
using NinjaNye.SearchExtensions.Levenshtein;
using Xunit;
using Xunit.Abstractions;

namespace NinjaNye.SearchExtensions.Tests.LevenshteinTests
{
    public class LevenshteinDistancePerformanceTests : BuildStringTestsBase
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public LevenshteinDistancePerformanceTests(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Theory(Skip = "Performance tests will likely fail in debug mode. Run in release mode")]
        [InlineData(6)]
        [InlineData(7)]
        public void ToLevenshteinDistance_CompareOneMillionStringsOfLengthX_ExecutesInLessThanOneSecond(int length)
        {
            //Arrange
            var words = BuildWords(1000000, length, length);
            var randomWord = BuildRandomWord(length,length);
            var stopwatch = new Stopwatch();
            //Act
            stopwatch.Start();
            var result = words.Select(w => LevenshteinProcessor.LevenshteinDistance(w, randomWord)).ToList();
            stopwatch.Stop();

            //Assert
            _testOutputHelper.WriteLine("Elapsed Time: {0}", stopwatch.Elapsed);
            _testOutputHelper.WriteLine("Total matching words: {0}", result.Count(i => i == 0));
            _testOutputHelper.WriteLine("Total words with distance of 1: {0}", result.Count(i => i == 1));
            _testOutputHelper.WriteLine("Total words with distance of 2: {0}", result.Count(i => i == 2));
            _testOutputHelper.WriteLine("Total words with distance of 3: {0}", result.Count(i => i == 3));
            Assert.True(stopwatch.Elapsed.TotalMilliseconds < 1000);
        }

        [Fact(Skip = "Performance tests will likely fail in debug mode. Run in release mode")]
        public void PerformLevenshteinDistanceUsingExpressionTreeBuilder()
        {
            //Setup 1 million comparisons
            var words = BuildWords(100000, 6, 6);
            var wordsToCompareTo = BuildWords(10, 6, 6).ToArray();
            var stopwatch = new Stopwatch();

            //Act
            stopwatch.Start();
            var result = words.LevenshteinDistanceOf(x => x).ComparedTo(wordsToCompareTo).ToList();
            stopwatch.Stop();

            //Assert
            _testOutputHelper.WriteLine("Elapsed Time: {0}", stopwatch.Elapsed);
            _testOutputHelper.WriteLine("Total matching words: {0}", result.Count(i => i.MinimumDistance == 0));
            _testOutputHelper.WriteLine("Total words with distance of 1: {0}", result.Count(i => i.MinimumDistance == 1));
            _testOutputHelper.WriteLine("Total words with distance of 2: {0}", result.Count(i => i.MinimumDistance == 2));
            _testOutputHelper.WriteLine("Total words with distance of 3: {0}", result.Count(i => i.MinimumDistance == 3));
            Assert.True(stopwatch.Elapsed.TotalMilliseconds <1000);
        }
    }
}