using Xunit;

namespace NinjaNye.SearchExtensions.Tests.SoundexTests
{
    public class ToSoundexTests
    {
        [Fact]
        public void ToSoundex_EmptyStringProvided_EmptyStringReturned()
        {
            //Arrange
            
            //Act
            string result = Soundex.SoundexProcessor.ToSoundex("");

            //Assert
            Assert.Empty(result);
        }

        [Fact]
        public void ToSoundex_NonEmptyStringProvided_NonEmptyStringReturned()
        {
            //Arrange
            
            //Act
            string result = Soundex.SoundexProcessor.ToSoundex("test");

            //Assert
            Assert.NotEmpty(result);
        }

        [Fact]
        public void ToSoundex_NonEmptyStringProvided_ReturnedStringStartsWithSameUpperCaseLetter()
        {
            //Arrange
            
            //Act
            string result = Soundex.SoundexProcessor.ToSoundex("Test");

            //Assert
            Assert.Equal('T', result[0]);
        }

        [Fact]
        public void ToSoundex_NonEmptyStringProvided_ReturnedStringHasNoACharacters()
        {
            //Arrange
            
            //Act
            string result = Soundex.SoundexProcessor.ToSoundex("Bats");

            //Assert
            Assert.Equal("B320", result);
        }

        [Fact]
        public void ToSoundex_NonEmptyStringProvided_ReturnedStringHasNoUppercaseACharacters()
        {
            //Arrange
            
            //Act
            string result = Soundex.SoundexProcessor.ToSoundex("BATS");

            //Assert
            Assert.Equal("B320", result);
        }

        [Fact]
        public void ToSoundex_NonEmptyStringProvided_ReturnedStringHasNoECharacters()
        {
            //Arrange
            
            //Act
            string result = Soundex.SoundexProcessor.ToSoundex("Breeze");

            //Assert
            Assert.Equal("B620", result);
        }

        [Fact]
        public void ToSoundex_NonEmptyStringProvided_ReturnedStringHasNoUppercaseECharacters()
        {
            //Arrange
            
            //Act
            string result = Soundex.SoundexProcessor.ToSoundex("BREEZE");

            //Assert
            Assert.Equal("B620", result);
        }

        [Fact]
        public void ToSoundex_NonEmptyStringProvided_ReturnedStringHasNoICharacters()
        {
            //Arrange
            
            //Act
            string result = Soundex.SoundexProcessor.ToSoundex("Brick");

            //Assert
            Assert.Equal("B620", result);
        }

        [Fact]
        public void ToSoundex_NonEmptyStringProvided_ReturnedStringHasNoUppercaseICharacters()
        {
            //Arrange
            
            //Act
            string result = Soundex.SoundexProcessor.ToSoundex("BRICK");

            //Assert
            Assert.Equal("B620", result);
        }

        [Fact]
        public void ToSoundex_NonEmptyStringProvided_ReturnedStringHasNoUCharacters()
        {
            //Arrange
            
            //Act
            string result = Soundex.SoundexProcessor.ToSoundex("Hunt");

            //Assert
            Assert.Equal("H530", result);
        }

        [Fact]
        public void ToSoundex_NonEmptyStringProvided_ReturnedStringHasNoUppercaseUCharacters()
        {
            //Arrange
            
            //Act
            string result = Soundex.SoundexProcessor.ToSoundex("HUNT");

            //Assert
            Assert.Equal("H530", result);
        }

        [Fact]
        public void ToSoundex_NonEmptyStringProvided_ReturnedStringHasNoYCharacters()
        {
            //Arrange
            
            //Act
            string result = Soundex.SoundexProcessor.ToSoundex("Bayern");

            //Assert
            Assert.Equal("B650", result);
        }

        [Fact]
        public void ToSoundex_NonEmptyStringProvided_ReturnedStringHasNoUppercaseYCharacters()
        {
            //Arrange
            
            //Act
            string result = Soundex.SoundexProcessor.ToSoundex("BAYERN");

            //Assert
            Assert.Equal("B650", result);
        }

        [Fact]
        public void ToSoundex_NonEmptyStringProvided_ReturnedStringHasNoHCharacters()
        {
            //Arrange
            
            //Act
            string result = Soundex.SoundexProcessor.ToSoundex("Breathe");

            //Assert
            Assert.Equal("B630", result);
        }

        [Fact]
        public void ToSoundex_NonEmptyStringProvided_ReturnedStringHasNoUppercaseHCharacters()
        {
            //Arrange
            
            //Act
            string result = Soundex.SoundexProcessor.ToSoundex("BREATHE");

            //Assert
            Assert.Equal("B630", result);
        }

        [Fact]
        public void ToSoundex_NonEmptyStringProvided_ReturnedStringHasNoWCharacters()
        {
            //Arrange
            
            //Act
            string result = Soundex.SoundexProcessor.ToSoundex("Qwerty");

            //Assert
            Assert.Equal("Q630", result);
        }

        [Fact]
        public void ToSoundex_NonEmptyStringProvided_ReturnedStringHasNoUppercaseWCharacters()
        {
            //Arrange
            
            //Act
            string result = Soundex.SoundexProcessor.ToSoundex("QWERTY");

            //Assert
            Assert.Equal("Q630", result);
        }

        [Fact]
        public void ToSoundex_NonEmptyStringProvided_ReturnedStringHasNoOCharacters()
        {
            //Arrange
            
            //Act
            string result = Soundex.SoundexProcessor.ToSoundex("Broom");

            //Assert
            Assert.Equal("B650", result);
        }

        [Fact]
        public void ToSoundex_NonEmptyStringProvided_ReturnedStringHasNoUppercaseOCharacters()
        {
            //Arrange
            
            //Act
            string result = Soundex.SoundexProcessor.ToSoundex("BROOM");

            //Assert
            Assert.Equal("B650", result);
        }

        [Theory]
        [InlineData("apple", "A140")]
        [InlineData("elephant", "E415")]
        [InlineData("indigo", "I532")]
        [InlineData("orange", "O652")]
        [InlineData("umbrella", "U516")]
        public void ToSoundex_FirstLetterIsAVowell_FirstLetterIsNotRemoved(string value, string expected)
        {
            //Arrange
            
            //Act
            string result = Soundex.SoundexProcessor.ToSoundex(value).ToUpper();

            //Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ToSoundex_NumberReplacements_ReplaceBWith1()
        {
            //Arrange
            
            //Act
            string result = Soundex.SoundexProcessor.ToSoundex("ABCD");

            //Assert
            Assert.Equal("A123", result);
        }

        [Fact]
        public void ToSoundex_NumberReplacements_ReplaceFWith1()
        {
            //Arrange
            
            //Act
            string result = Soundex.SoundexProcessor.ToSoundex("DEFG");

            //Assert
            Assert.Equal("D120", result);
        }

        [Fact]
        public void ToSoundex_NumberReplacements_ReplacePWith1()
        {
            //Arrange
            
            //Act
            string result = Soundex.SoundexProcessor.ToSoundex("NOPQ");

            //Assert
            Assert.Equal("N120", result);
        }

        [Fact]
        public void ToSoundex_NumberReplacements_ReplaceVWith1()
        {
            //Arrange
            
            //Act
            string result = Soundex.SoundexProcessor.ToSoundex("TUVW");

            //Assert
            Assert.Equal("T100", result);
        }

        [Theory]
        [InlineData("BCDE", "B230")]
        [InlineData("FGHI", "F200")]
        [InlineData("PQRS", "P262")]
        [InlineData("VWXY", "V200")]
        public void ToSoundex_NumberReplacements_FirstCharacterIsNotReplaced(string value, string expected)
        {
            //Arrange
            
            //Act
            string result = Soundex.SoundexProcessor.ToSoundex(value);

            //Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ToSoundex_NumberReplacements_ReplaceCWith2()
        {
            //Arrange

            //Act
            string result = Soundex.SoundexProcessor.ToSoundex("ABCD");

            //Assert
            Assert.Equal("A123", result);
        }

        [Fact]
        public void ToSoundex_NumberReplacements_ReplaceGWith2()
        {
            //Arrange

            //Act
            string result = Soundex.SoundexProcessor.ToSoundex("EFGH");

            //Assert
            Assert.Equal("E120", result);
        }

        [Fact]
        public void ToSoundex_NumberReplacements_ReplaceJWith2()
        {
            //Arrange

            //Act
            string result = Soundex.SoundexProcessor.ToSoundex("HIJK");

            //Assert
            Assert.Equal("H200", result);
        }

        [Fact]
        public void ToSoundex_NumberReplacements_ReplaceKWith2()
        {
            //Arrange

            //Act
            string result = Soundex.SoundexProcessor.ToSoundex("HIJK");

            //Assert
            Assert.Equal("H200", result);
        }

        [Fact]
        public void ToSoundex_NumberReplacements_ReplaceQWith2()
        {
            //Arrange

            //Act
            string result = Soundex.SoundexProcessor.ToSoundex("PQRS");

            //Assert
            Assert.Equal("P262", result);
        }

        [Fact]
        public void ToSoundex_NumberReplacements_ReplaceSWith2()
        {
            //Arrange

            //Act
            string result = Soundex.SoundexProcessor.ToSoundex("PQRS");

            //Assert
            Assert.Equal("P262", result);
        }

        [Fact]
        public void ToSoundex_NumberReplacements_ReplaceXWith2()
        {
            //Arrange

            //Act
            string result = Soundex.SoundexProcessor.ToSoundex("VWXY");

            //Assert
            Assert.Equal("V200", result);
        }

        [Fact]
        public void ToSoundex_NumberReplacements_ReplacezWith2()
        {
            //Arrange

            //Act
            string result = Soundex.SoundexProcessor.ToSoundex("WXYZ");

            //Assert
            Assert.Equal("W220", result);
        }

        [Fact]
        public void ToSoundex_NumberReplacements_ReplaceDWith3()
        {
            //Arrange

            //Act
            string result = Soundex.SoundexProcessor.ToSoundex("CDEF");

            //Assert
            Assert.Equal("C310", result);
        }

        [Fact]
        public void ToSoundex_NumberReplacements_ReplaceTWith3()
        {
            //Arrange

            //Act
            string result = Soundex.SoundexProcessor.ToSoundex("RSTU");

            //Assert
            Assert.Equal("R230", result);
        }

        [Fact]
        public void ToSoundex_NumberReplacements_ReplaceLWith4()
        {
            //Arrange

            //Act
            string result = Soundex.SoundexProcessor.ToSoundex("MNLO");

            //Assert    
            Assert.Equal("M400", result);
        }

        [Fact]
        public void ToSoundex_NumberReplacements_ReplaceTWith5()
        {
            //Arrange

            //Act
            string result = Soundex.SoundexProcessor.ToSoundex("LMNO");

            //Assert
            Assert.Equal("L500", result);
        }

        [Fact]
        public void ToSoundex_NumberReplacements_ReplaceLWith5()
        {
            //Arrange

            //Act
            string result = Soundex.SoundexProcessor.ToSoundex("LMNO");

            //Assert
            Assert.Equal("L500", result);
        }

        [Fact]
        public void ToSoundex_NumberReplacements_ReplaceLWith6()
        {
            //Arrange

            //Act
            string result = Soundex.SoundexProcessor.ToSoundex("PQRS");

            //Assert
            Assert.Equal("P262", result);
        }

        [Fact]
        // If two or more letters with the same number are adjacent in the 
        // original name (before step 1), only retain the first letter
        public void ToSoundex_TwoMatchingLetterGroups_OnlyRetainOnce()
        {
            //Arrange

            //Act
            string result = Soundex.SoundexProcessor.ToSoundex("Flasker");

            //Assert
            Assert.Equal("F426", result);
        }

        [Fact]
        // If two or more letters with the same number are adjacent in the 
        // original name (before step 1), only retain the first letter
        public void ToSoundex_ThreeMatchingLetterGroups_OnlyRetainOnce()
        {
            //Arrange

            //Act
            string result = Soundex.SoundexProcessor.ToSoundex("Flasks");

            //Assert
            Assert.Equal("F420", result);
        }

        [Fact]
        // two letters with the same number separated by 'h' or 'w' are coded as a single number
        public void ToSoundex_MatchingLetterGroupSeperatedByH_OnlyRetainOnce()
        {
            //Arrange

            //Act
            string result = Soundex.SoundexProcessor.ToSoundex("Ashcraft");

            //Assert
            Assert.Equal("A261", result);
        }

        [Fact]
        // letters separated by a vowel are coded twice
        public void ToSoundex_MatchingLetterGroupSeperatedByVowell_RetainBoth()
        {
            //Arrange

            //Act
            string result = Soundex.SoundexProcessor.ToSoundex("Stadium");

            //Assert
            Assert.Equal("S335", result);
        }

        [Fact]
        // If two or more letters with the same number are adjacent in the 
        // original name (before step 1), only retain the first letter
        public void ToSoundex_TwoMatchingLetterGroupsAtStart_OnlyRetainOnce()
        {
            //Arrange

            //Act
            string result = Soundex.SoundexProcessor.ToSoundex("Pfister");

            //Assert
            Assert.Equal("P236", result);
        }

        [Fact]
        // If you have too few letters in your word that you can't assign 
        // three numbers, append with zeros until there are three numbers
        public void ToSoundex_TooFewNumbers_PadToThreeNumbers()
        {
            //Arrange

            //Act
            string result = Soundex.SoundexProcessor.ToSoundex("Rubin");

            //Assert
            Assert.Equal("R150", result);
        }

        [Fact]
        //  If you have more than 3 letters, just retain the first 3 numbers
        public void ToSoundex_TooManyNumbers_TrimToThreeNumbers()
        {
            //Arrange

            //Act
            string result = Soundex.SoundexProcessor.ToSoundex("Ashcraft");

            //Assert
            Assert.Equal("A261", result);
        }

        [Fact]
        //  If you have more than 3 letters, just retain the first 3 numbers
        public void ToSoundex_AnalyseRobertAndRupert_BothReturnTheSameCode()
        {
            //Arrange

            //Act
            string result1 = Soundex.SoundexProcessor.ToSoundex("Robert");
            string result2 = Soundex.SoundexProcessor.ToSoundex("Rupert");

            //Assert
            Assert.Equal("R163", result1);
            Assert.Equal(result1, result2);
        }

        [Fact]
        //  If you have more than 3 letters, just retain the first 3 numbers
        public void ToSoundex_AnalyseAshcraftAndAshcroft_BothReturnTheSameCode()
        {
            //Arrange

            //Act
            string result1 = Soundex.SoundexProcessor.ToSoundex("Ashcraft");
            string result2 = Soundex.SoundexProcessor.ToSoundex("Aschroft");

            //Assert
            Assert.Equal(result1, result2);
        }

        [Fact]
        //  If you have more than 3 letters, just retain the first 3 numbers
        public void ToSoundex_AnalyseTymczak_ReturnCorrectCode()
        {
            //Arrange

            //Act
            string result = Soundex.SoundexProcessor.ToSoundex("Tymczak");

            //Assert
            Assert.Equal("T522", result);
        }

        [Fact]
        public void ToSoundex_LowercaseWord_ReturnUppercaseCode()
        {
            //Arrange

            //Act
            string result = Soundex.SoundexProcessor.ToSoundex("ashcraft");

            //Assert
            Assert.Equal("A261", result);
        }
    }
}
