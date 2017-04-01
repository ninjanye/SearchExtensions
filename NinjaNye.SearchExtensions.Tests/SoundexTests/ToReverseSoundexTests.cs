using Xunit;

namespace NinjaNye.SearchExtensions.Tests.SoundexTests
{
    public class ToReverseSoundexTests
    {
        [Fact]
        public void ToReverseSoundex_EmptyStringProvided_EmptyStringReturned()
        {
            //Arrange
            
            //Act
            string result = Soundex.SoundexProcessor.ToReverseSoundex("");

            //Assert
            Assert.Empty(result);
        }

        [Fact]
        public void ToReverseSoundex_NonEmptyStringProvided_NonEmptyStringReturned()
        {
            //Arrange
            
            //Act
            string result = Soundex.SoundexProcessor.ToReverseSoundex("test");

            //Assert
            Assert.NotEmpty(result);
        }

        [Fact]
        public void ToReverseSoundex_NonEmptyStringProvided_ReturnedStringStartsWithLastLetterInUpperCase()
        {
            //Arrange
            
            //Act
            string result = Soundex.SoundexProcessor.ToReverseSoundex("Tester");

            //Assert
            Assert.Equal('R', result[0]);
        }

        [Theory]
        [InlineData("umberella", "A461")]
        [InlineData("apple", "E410")]
        [InlineData("perreli", "I461")]
        [InlineData("indigo", "O235")]
        [InlineData("zulu", "U420")]
        public void ToReverseSoundex_LastLetterIsAVowell_LastLetterIsNotRemoved(string value, string expected)
        {
            //Arrange
            
            //Act
            string result = Soundex.SoundexProcessor.ToReverseSoundex(value).ToUpper();

            //Assert
            Assert.Equal(expected, result);
        }
    }
}
