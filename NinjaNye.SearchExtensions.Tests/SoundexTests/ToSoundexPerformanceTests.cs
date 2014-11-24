using System;
using System.Diagnostics;
using System.Linq;
using NUnit.Framework;
using NinjaNye.SearchExtensions.Soundex;

namespace NinjaNye.SearchExtensions.Tests.SoundexTests
{
    [TestFixture]
    public class ToSoundexPerformanceTests : BuildStringTestsBase
    {
        [Test]
        public void ToSoundex_OneMillionRecords_UnderOneSecond()
        {
            //Arrange
            var words = BuildWords(1000000);
            Console.WriteLine("Processing {0} words", words.Count);
            var stopwatch = new Stopwatch();
            Console.WriteLine("Begin soundex...");
            stopwatch.Start();
             
            //Act
            var result = words.Select(SoundexProcessor.ToSoundex).ToList();
            stopwatch.Stop();
            Console.WriteLine("Time taken: {0}", stopwatch.Elapsed);
            Console.WriteLine("Results retrieved: {0}", result.Count()); 
            //Assert
            Assert.True(stopwatch.Elapsed.TotalMilliseconds < 1000);
        }

        [Test]
        public void SearchSoundex_OneMillionWordsComparedToOneWord_UnderOneSecond()
        {
            //Arrange
            var words = BuildWords(1000000);
            Console.WriteLine("Processing {0} words", words.Count);
            
            var stopwatch = new Stopwatch();
            Console.WriteLine("Begin soundex search...");
            stopwatch.Start();
             
            //Act
            var result = words.Search(x => x).Soundex("test").ToList();
            stopwatch.Stop();
            Console.WriteLine("Time taken: {0}", stopwatch.Elapsed);
            Console.WriteLine("Results retrieved: {0}", result.Count); 
            //Assert
            Assert.True(stopwatch.Elapsed.Milliseconds < 1000);
        }

        [Test]
        public void SearchSoundex_OneMillionWordsComparedToTwoWords_UnderOneSecond()
        {
            //Arrange
            var words = BuildWords(1000000);
            Console.WriteLine("Processing {0} words", words.Count);

            var stopwatch = new Stopwatch();
            Console.WriteLine("Begin soundex search...");
            stopwatch.Start();

            //Act
            var result = words.Search(x => x).Soundex("test", "bacon").ToList();
            stopwatch.Stop();
            Console.WriteLine("Time taken: {0}", stopwatch.Elapsed);
            Console.WriteLine("Results retrieved: {0}", result.Count);
            //Assert
            Assert.True(stopwatch.Elapsed.Milliseconds < 1000);
        }

        [Test]
        public void SearchSoundex_OneMillionWordsComparedToTenWords_UnderOneSecond()
        {
            //Arrange
            var words = BuildWords(1000000);
            Console.WriteLine("Processing {0} words", words.Count);

            var stopwatch = new Stopwatch();
            Console.WriteLine("Begin soundex search...");
            stopwatch.Start();

            //Act
            var result = words.Search(x => x).Soundex("historians", "often", "articulate", "great", "battles",
                                                      "elegantly", "without", "pause", "for", "thought").ToList();
            stopwatch.Stop();
            Console.WriteLine("Time taken: {0}", stopwatch.Elapsed);
            Console.WriteLine("Results retrieved: {0}", result.Count);
            //Assert
            Assert.True(stopwatch.Elapsed.Milliseconds < 1000);
        }
    }
}