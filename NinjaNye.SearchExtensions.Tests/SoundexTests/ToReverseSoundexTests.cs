using NUnit.Framework;

namespace NinjaNye.SearchExtensions.Tests.SoundexTests
{
    [TestFixture]
    public class ToReverseSoundexTests
    {
        [Test]
        public void ToReverseSoundex_EmptyStringProvided_EmptyStringReturned()
        {
            //Arrange
            
            //Act
            string result = Soundex.SoundexProcessor.ToReverseSoundex("");

            //Assert
            Assert.IsEmpty(result);
        }

        [Test]
        public void ToReverseSoundex_NonEmptyStringProvided_NonEmptyStringReturned()
        {
            //Arrange
            
            //Act
            string result = Soundex.SoundexProcessor.ToReverseSoundex("test");

            //Assert
            Assert.IsNotEmpty(result);
        }

        [Test]
        public void ToReverseSoundex_NonEmptyStringProvided_ReturnedStringStartsWithLastLetterInUpperCase()
        {
            //Arrange
            
            //Act
            string result = Soundex.SoundexProcessor.ToReverseSoundex("Tester");

            //Assert
            Assert.AreEqual('R', result[0]);
        }


        [TestCase("umberella", "A461")]
        [TestCase("apple", "E410")]
        [TestCase("perreli", "I461")]
        [TestCase("indigo", "O235")]
        [TestCase("zulu", "U420")]
        public void ToReverseSoundex_LastLetterIsAVowell_LastLetterIsNotRemoved(string value, string expected)
        {
            //Arrange
            
            //Act
            string result = Soundex.SoundexProcessor.ToReverseSoundex(value).ToUpper();

            //Assert
            Assert.AreEqual(expected, result);
        }
    }
}
