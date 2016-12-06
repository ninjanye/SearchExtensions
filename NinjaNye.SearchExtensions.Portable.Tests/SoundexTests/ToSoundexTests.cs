using NUnit.Framework;

namespace NinjaNye.SearchExtensions.Portable.Tests.SoundexTests
{
    [TestFixture]
    public class ToSoundexTests
    {
        [Test]
        public void ToSoundex_EmptyStringProvided_EmptyStringReturned()
        {
            //Arrange
            
            //Act
            string result = Soundex.SoundexProcessor.ToSoundex("");

            //Assert
            Assert.IsEmpty(result);
        }

        [Test]
        public void ToSoundex_NonEmptyStringProvided_NonEmptyStringReturned()
        {
            //Arrange
            
            //Act
            string result = Soundex.SoundexProcessor.ToSoundex("test");

            //Assert
            Assert.IsNotEmpty(result);
        }

        [Test]
        public void ToSoundex_NonEmptyStringProvided_ReturnedStringStartsWithSameUpperCaseLetter()
        {
            //Arrange
            
            //Act
            string result = Soundex.SoundexProcessor.ToSoundex("Test");

            //Assert
            Assert.AreEqual('T', result[0]);
        }

        [Test]
        public void ToSoundex_NonEmptyStringProvided_ReturnedStringHasNoACharacters()
        {
            //Arrange
            
            //Act
            string result = Soundex.SoundexProcessor.ToSoundex("Bats");

            //Assert
            Assert.AreEqual("B320", result);
        }

        [Test]
        public void ToSoundex_NonEmptyStringProvided_ReturnedStringHasNoUppercaseACharacters()
        {
            //Arrange
            
            //Act
            string result = Soundex.SoundexProcessor.ToSoundex("BATS");

            //Assert
            Assert.AreEqual("B320", result);
        }

        [Test]
        public void ToSoundex_NonEmptyStringProvided_ReturnedStringHasNoECharacters()
        {
            //Arrange
            
            //Act
            string result = Soundex.SoundexProcessor.ToSoundex("Breeze");

            //Assert
            Assert.AreEqual("B620", result);
        }

        [Test]
        public void ToSoundex_NonEmptyStringProvided_ReturnedStringHasNoUppercaseECharacters()
        {
            //Arrange
            
            //Act
            string result = Soundex.SoundexProcessor.ToSoundex("BREEZE");

            //Assert
            Assert.AreEqual("B620", result);
        }

        [Test]
        public void ToSoundex_NonEmptyStringProvided_ReturnedStringHasNoICharacters()
        {
            //Arrange
            
            //Act
            string result = Soundex.SoundexProcessor.ToSoundex("Brick");

            //Assert
            Assert.AreEqual("B620", result);
        }

        [Test]
        public void ToSoundex_NonEmptyStringProvided_ReturnedStringHasNoUppercaseICharacters()
        {
            //Arrange
            
            //Act
            string result = Soundex.SoundexProcessor.ToSoundex("BRICK");

            //Assert
            Assert.AreEqual("B620", result);
        }

        [Test]
        public void ToSoundex_NonEmptyStringProvided_ReturnedStringHasNoUCharacters()
        {
            //Arrange
            
            //Act
            string result = Soundex.SoundexProcessor.ToSoundex("Hunt");

            //Assert
            Assert.AreEqual("H530", result);
        }

        [Test]
        public void ToSoundex_NonEmptyStringProvided_ReturnedStringHasNoUppercaseUCharacters()
        {
            //Arrange
            
            //Act
            string result = Soundex.SoundexProcessor.ToSoundex("HUNT");

            //Assert
            Assert.AreEqual("H530", result);
        }

        [Test]
        public void ToSoundex_NonEmptyStringProvided_ReturnedStringHasNoYCharacters()
        {
            //Arrange
            
            //Act
            string result = Soundex.SoundexProcessor.ToSoundex("Bayern");

            //Assert
            Assert.AreEqual("B650", result);
        }

        [Test]
        public void ToSoundex_NonEmptyStringProvided_ReturnedStringHasNoUppercaseYCharacters()
        {
            //Arrange
            
            //Act
            string result = Soundex.SoundexProcessor.ToSoundex("BAYERN");

            //Assert
            Assert.AreEqual("B650", result);
        }

        [Test]
        public void ToSoundex_NonEmptyStringProvided_ReturnedStringHasNoHCharacters()
        {
            //Arrange
            
            //Act
            string result = Soundex.SoundexProcessor.ToSoundex("Breathe");

            //Assert
            Assert.AreEqual("B630", result);
        }

        [Test]
        public void ToSoundex_NonEmptyStringProvided_ReturnedStringHasNoUppercaseHCharacters()
        {
            //Arrange
            
            //Act
            string result = Soundex.SoundexProcessor.ToSoundex("BREATHE");

            //Assert
            Assert.AreEqual("B630", result);
        }

        [Test]
        public void ToSoundex_NonEmptyStringProvided_ReturnedStringHasNoWCharacters()
        {
            //Arrange
            
            //Act
            string result = Soundex.SoundexProcessor.ToSoundex("Qwerty");

            //Assert
            Assert.AreEqual("Q630", result);
        }

        [Test]
        public void ToSoundex_NonEmptyStringProvided_ReturnedStringHasNoUppercaseWCharacters()
        {
            //Arrange
            
            //Act
            string result = Soundex.SoundexProcessor.ToSoundex("QWERTY");

            //Assert
            Assert.AreEqual("Q630", result);
        }

        [Test]
        public void ToSoundex_NonEmptyStringProvided_ReturnedStringHasNoOCharacters()
        {
            //Arrange
            
            //Act
            string result = Soundex.SoundexProcessor.ToSoundex("Broom");

            //Assert
            Assert.AreEqual("B650", result);
        }

        [Test]
        public void ToSoundex_NonEmptyStringProvided_ReturnedStringHasNoUppercaseOCharacters()
        {
            //Arrange
            
            //Act
            string result = Soundex.SoundexProcessor.ToSoundex("BROOM");

            //Assert
            Assert.AreEqual("B650", result);
        }

        [TestCase("apple", "A140")]
        [TestCase("elephant", "E415")]
        [TestCase("indigo", "I532")]
        [TestCase("orange", "O652")]
        [TestCase("umbrella", "U516")]
        public void ToSoundex_FirstLetterIsAVowell_FirstLetterIsNotRemoved(string value, string expected)
        {
            //Arrange
            
            //Act
            string result = Soundex.SoundexProcessor.ToSoundex(value).ToUpper();

            //Assert
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void ToSoundex_NumberReplacements_ReplaceBWith1()
        {
            //Arrange
            
            //Act
            string result = Soundex.SoundexProcessor.ToSoundex("ABCD");

            //Assert
            Assert.AreEqual("A123", result);
        }

        [Test]
        public void ToSoundex_NumberReplacements_ReplaceFWith1()
        {
            //Arrange
            
            //Act
            string result = Soundex.SoundexProcessor.ToSoundex("DEFG");

            //Assert
            Assert.AreEqual("D120", result);
        }

        [Test]
        public void ToSoundex_NumberReplacements_ReplacePWith1()
        {
            //Arrange
            
            //Act
            string result = Soundex.SoundexProcessor.ToSoundex("NOPQ");

            //Assert
            Assert.AreEqual("N120", result);
        }

        [Test]
        public void ToSoundex_NumberReplacements_ReplaceVWith1()
        {
            //Arrange
            
            //Act
            string result = Soundex.SoundexProcessor.ToSoundex("TUVW");

            //Assert
            Assert.AreEqual("T100", result);
        }

        [TestCase("BCDE", "B230")]
        [TestCase("FGHI", "F200")]
        [TestCase("PQRS", "P262")]
        [TestCase("VWXY", "V200")]
        public void ToSoundex_NumberReplacements_FirstCharacterIsNotReplaced(string value, string expected)
        {
            //Arrange
            
            //Act
            string result = Soundex.SoundexProcessor.ToSoundex(value);

            //Assert
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void ToSoundex_NumberReplacements_ReplaceCWith2()
        {
            //Arrange

            //Act
            string result = Soundex.SoundexProcessor.ToSoundex("ABCD");

            //Assert
            Assert.AreEqual("A123", result);
        }

        [Test]
        public void ToSoundex_NumberReplacements_ReplaceGWith2()
        {
            //Arrange

            //Act
            string result = Soundex.SoundexProcessor.ToSoundex("EFGH");

            //Assert
            Assert.AreEqual("E120", result);
        }

        [Test]
        public void ToSoundex_NumberReplacements_ReplaceJWith2()
        {
            //Arrange

            //Act
            string result = Soundex.SoundexProcessor.ToSoundex("HIJK");

            //Assert
            Assert.AreEqual("H200", result);
        }

        [Test]
        public void ToSoundex_NumberReplacements_ReplaceKWith2()
        {
            //Arrange

            //Act
            string result = Soundex.SoundexProcessor.ToSoundex("HIJK");

            //Assert
            Assert.AreEqual("H200", result);
        }

        [Test]
        public void ToSoundex_NumberReplacements_ReplaceQWith2()
        {
            //Arrange

            //Act
            string result = Soundex.SoundexProcessor.ToSoundex("PQRS");

            //Assert
            Assert.AreEqual("P262", result);
        }

        [Test]
        public void ToSoundex_NumberReplacements_ReplaceSWith2()
        {
            //Arrange

            //Act
            string result = Soundex.SoundexProcessor.ToSoundex("PQRS");

            //Assert
            Assert.AreEqual("P262", result);
        }

        [Test]
        public void ToSoundex_NumberReplacements_ReplaceXWith2()
        {
            //Arrange

            //Act
            string result = Soundex.SoundexProcessor.ToSoundex("VWXY");

            //Assert
            Assert.AreEqual("V200", result);
        }

        [Test]
        public void ToSoundex_NumberReplacements_ReplacezWith2()
        {
            //Arrange

            //Act
            string result = Soundex.SoundexProcessor.ToSoundex("WXYZ");

            //Assert
            Assert.AreEqual("W220", result);
        }

        [Test]
        public void ToSoundex_NumberReplacements_ReplaceDWith3()
        {
            //Arrange

            //Act
            string result = Soundex.SoundexProcessor.ToSoundex("CDEF");

            //Assert
            Assert.AreEqual("C310", result);
        }

        [Test]
        public void ToSoundex_NumberReplacements_ReplaceTWith3()
        {
            //Arrange

            //Act
            string result = Soundex.SoundexProcessor.ToSoundex("RSTU");

            //Assert
            Assert.AreEqual("R230", result);
        }

        [Test]
        public void ToSoundex_NumberReplacements_ReplaceLWith4()
        {
            //Arrange

            //Act
            string result = Soundex.SoundexProcessor.ToSoundex("MNLO");

            //Assert    
            Assert.AreEqual("M400", result);
        }

        [Test]
        public void ToSoundex_NumberReplacements_ReplaceTWith5()
        {
            //Arrange

            //Act
            string result = Soundex.SoundexProcessor.ToSoundex("LMNO");

            //Assert
            Assert.AreEqual("L500", result);
        }

        [Test]
        public void ToSoundex_NumberReplacements_ReplaceLWith5()
        {
            //Arrange

            //Act
            string result = Soundex.SoundexProcessor.ToSoundex("LMNO");

            //Assert
            Assert.AreEqual("L500", result);
        }

        [Test]
        public void ToSoundex_NumberReplacements_ReplaceLWith6()
        {
            //Arrange

            //Act
            string result = Soundex.SoundexProcessor.ToSoundex("PQRS");

            //Assert
            Assert.AreEqual("P262", result);
        }

        [Test]
        // If two or more letters with the same number are adjacent in the 
        // original name (before step 1), only retain the first letter
        public void ToSoundex_TwoMatchingLetterGroups_OnlyRetainOnce()
        {
            //Arrange

            //Act
            string result = Soundex.SoundexProcessor.ToSoundex("Flasker");

            //Assert
            Assert.AreEqual("F426", result);
        }

        [Test]
        // If two or more letters with the same number are adjacent in the 
        // original name (before step 1), only retain the first letter
        public void ToSoundex_ThreeMatchingLetterGroups_OnlyRetainOnce()
        {
            //Arrange

            //Act
            string result = Soundex.SoundexProcessor.ToSoundex("Flasks");

            //Assert
            Assert.AreEqual("F420", result);
        }

        [Test]
        // two letters with the same number separated by 'h' or 'w' are coded as a single number
        public void ToSoundex_MatchingLetterGroupSeperatedByH_OnlyRetainOnce()
        {
            //Arrange

            //Act
            string result = Soundex.SoundexProcessor.ToSoundex("Ashcraft");

            //Assert
            Assert.AreEqual("A261", result);
        }

        [Test]
        // letters separated by a vowel are coded twice
        public void ToSoundex_MatchingLetterGroupSeperatedByVowell_RetainBoth()
        {
            //Arrange

            //Act
            string result = Soundex.SoundexProcessor.ToSoundex("Stadium");

            //Assert
            Assert.AreEqual("S335", result);
        }

        [Test]
        // If two or more letters with the same number are adjacent in the 
        // original name (before step 1), only retain the first letter
        public void ToSoundex_TwoMatchingLetterGroupsAtStart_OnlyRetainOnce()
        {
            //Arrange

            //Act
            string result = Soundex.SoundexProcessor.ToSoundex("Pfister");

            //Assert
            Assert.AreEqual("P236", result);
        }

        [Test]
        // If you have too few letters in your word that you can't assign 
        // three numbers, append with zeros until there are three numbers
        public void ToSoundex_TooFewNumbers_PadToThreeNumbers()
        {
            //Arrange

            //Act
            string result = Soundex.SoundexProcessor.ToSoundex("Rubin");

            //Assert
            Assert.AreEqual("R150", result);
        }

        [Test]
        //  If you have more than 3 letters, just retain the first 3 numbers
        public void ToSoundex_TooManyNumbers_TrimToThreeNumbers()
        {
            //Arrange

            //Act
            string result = Soundex.SoundexProcessor.ToSoundex("Ashcraft");

            //Assert
            Assert.AreEqual("A261", result);
        }

        [Test]
        //  If you have more than 3 letters, just retain the first 3 numbers
        public void ToSoundex_AnalyseRobertAndRupert_BothReturnTheSameCode()
        {
            //Arrange

            //Act
            string result1 = Soundex.SoundexProcessor.ToSoundex("Robert");
            string result2 = Soundex.SoundexProcessor.ToSoundex("Rupert");

            //Assert
            Assert.AreEqual("R163", result1);
            Assert.AreEqual(result1, result2);
        }

        [Test]
        //  If you have more than 3 letters, just retain the first 3 numbers
        public void ToSoundex_AnalyseAshcraftAndAshcroft_BothReturnTheSameCode()
        {
            //Arrange

            //Act
            string result1 = Soundex.SoundexProcessor.ToSoundex("Ashcraft");
            string result2 = Soundex.SoundexProcessor.ToSoundex("Aschroft");

            //Assert
            Assert.AreEqual(result1, result2);
        }

        [Test]
        //  If you have more than 3 letters, just retain the first 3 numbers
        public void ToSoundex_AnalyseTymczak_ReturnCorrectCode()
        {
            //Arrange

            //Act
            string result = Soundex.SoundexProcessor.ToSoundex("Tymczak");

            //Assert
            Assert.AreEqual("T522", result);
        }

        [Test]
        public void ToSoundex_LowercaseWord_ReturnUppercaseCode()
        {
            //Arrange

            //Act
            string result = Soundex.SoundexProcessor.ToSoundex("ashcraft");

            //Assert
            Assert.AreEqual("A261", result);
        }
    }
}
