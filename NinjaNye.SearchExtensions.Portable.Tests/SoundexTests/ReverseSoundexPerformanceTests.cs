using System;
using System.Diagnostics;
using System.Linq;
using NUnit.Framework;
using NinjaNye.SearchExtensions.Portable.Soundex;

namespace NinjaNye.SearchExtensions.Portable.Tests.SoundexTests
{
#if DEBUG
    [Ignore("Performance tests only to be run in Release mode")]
#endif
    [TestFixture]
    public class  ReverseSoundexPerformanceTests : BuildStringTestsBase
    {
        [Test]
        public void ToReverseSoundex_OneMillionRecords_UnderOneSecond()
        {
            //Arrange
            var words = BuildWords(1000000);
            Console.WriteLine("Processing {0} words", words.Count);
            var stopwatch = new Stopwatch();
            Console.WriteLine("Begin soundex...");
            stopwatch.Start();

            //Act
            var result = words.Select(SoundexProcessor.ToReverseSoundex).ToList();
            stopwatch.Stop();
            Console.WriteLine("Time taken: {0}", stopwatch.Elapsed);
            Console.WriteLine("Results retrieved: {0}", result.Count());
            //Assert
            Assert.True(stopwatch.Elapsed.TotalMilliseconds < 1000);
        }

        [Test]
        public void ReverseSoundex_ReverseWordSoundexVsToReverseSoundex_ToReverseSoundexIsQuicker()
        {
            //Arrange
            var words = BuildWords(1000000);
            Console.WriteLine("Processing {0} words", words.Count);

            var stopwatch = new Stopwatch();
            Console.WriteLine("Begin reverse soundex search...");
            stopwatch.Start();
            var reverseWordResult = words.Select(w => w.Reverse().ToString().ToSoundex()).ToList();

            //Act

            stopwatch.Stop();
            var reverseSoundexTimeTaken = stopwatch.Elapsed;
            Console.WriteLine("Time taken: {0}", reverseSoundexTimeTaken);
            Console.WriteLine("Results retrieved: {0}", reverseWordResult.Count);

            Console.WriteLine("Begin reverse word ToSoundex search...");
            stopwatch.Restart();
            var reverseSoundexResult = words.Select(w => w.ToReverseSoundex()).ToList();
            stopwatch.Stop();
            var reverseWordTimeTaken = stopwatch.Elapsed;
            Console.WriteLine("Time taken: {0}", reverseWordTimeTaken);
            Console.WriteLine("Results retrieved: {0}", reverseSoundexResult.Count);

            //Assert
            Assert.True(reverseWordTimeTaken < reverseSoundexTimeTaken);
        }

    }
}