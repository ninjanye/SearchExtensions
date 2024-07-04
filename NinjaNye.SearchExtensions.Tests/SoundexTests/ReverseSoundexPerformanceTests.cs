using System.Diagnostics;
using System.Linq;
using Xunit;
using NinjaNye.SearchExtensions.Soundex;
using Xunit.Abstractions;

namespace NinjaNye.SearchExtensions.Tests.SoundexTests
{
    public class  ReverseSoundexPerformanceTests : BuildStringTestsBase
    {
        private readonly ITestOutputHelper _testOutputHelper;
        public ReverseSoundexPerformanceTests(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Fact(Skip = "Performance tests only to be run in Release mode")]
        public void ToReverseSoundex_OneMillionRecords_UnderOneSecond()
        {
            //Arrange
            var words = BuildWords(1000000);
            _testOutputHelper.WriteLine("Processing {0} words", words.Count);
            var stopwatch = new Stopwatch();
            _testOutputHelper.WriteLine("Begin soundex...");
            stopwatch.Start();

            //Act
            var result = words.Select(SoundexProcessor.ToReverseSoundex).ToList();
            stopwatch.Stop();
            _testOutputHelper.WriteLine("Time taken: {0}", stopwatch.Elapsed);
            _testOutputHelper.WriteLine("Results retrieved: {0}", result.Count());
            //Assert
            Assert.True(stopwatch.Elapsed.TotalMilliseconds < 1000);
        }

        [Fact(Skip = "Performance tests only to be run in Release mode")]
        public void ReverseSoundex_ReverseWordSoundexVsToReverseSoundex_ToReverseSoundexIsQuicker()
        {
            //Arrange
            var words = BuildWords(1000000);
            _testOutputHelper.WriteLine("Processing {0} words", words.Count);

            var stopwatch = new Stopwatch();
            _testOutputHelper.WriteLine("Begin reverse soundex search...");
            stopwatch.Start();
            var reverseWordResult = words.Select(w => w.Reverse().ToString().ToSoundex()).ToList();

            //Act

            stopwatch.Stop();
            var reverseSoundexTimeTaken = stopwatch.Elapsed;
            _testOutputHelper.WriteLine("Time taken: {0}", reverseSoundexTimeTaken);
            _testOutputHelper.WriteLine("Results retrieved: {0}", reverseWordResult.Count);

            _testOutputHelper.WriteLine("Begin reverse word ToSoundex search...");
            stopwatch.Restart();
            var reverseSoundexResult = words.Select(w => w.ToReverseSoundex()).ToList();
            stopwatch.Stop();
            var reverseWordTimeTaken = stopwatch.Elapsed;
            _testOutputHelper.WriteLine("Time taken: {0}", reverseWordTimeTaken);
            _testOutputHelper.WriteLine("Results retrieved: {0}", reverseSoundexResult.Count);

            //Assert
            Assert.True(reverseWordTimeTaken < reverseSoundexTimeTaken);
        }

    }
}