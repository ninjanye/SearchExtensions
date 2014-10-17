using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using NinjaNye.SearchExtensions.Soundex;
using NinjaNye.SearchExtensions.Tests.SearchExtensionTests;

namespace NinjaNye.SearchExtensions.Tests.Fluent
{
    [TestFixture]
    public class SoundexSearchTests
    {
        private List<TestData> testData = new List<TestData>();

        [SetUp]
        public void ClassSetup()
        {
            this.testData = new List<TestData>();
            this.BuildTestData();
        }

        private void BuildTestData()
        {
            this.testData.Add(new TestData {Name = "Robert", Description = "Sounds like Rupert", Number = 1});
            this.testData.Add(new TestData {Name = "Rupert", Description = "SOunds like Robert", Number = 2});
            this.testData.Add(new TestData {Name = "John", Description = "Sounds like Jon", Number = 3});
            this.testData.Add(new TestData {Name = "Jon", Description = "Sounds like John", Number = 4});
            this.testData.Add(new TestData {Name = "Matt", Description = "Sounds like Mitt", Number = 5});
            this.testData.Add(new TestData {Name = "Aschcraft", Description = "Sounds like Ashcroft", Number = 6});
        }

        [Test]
        public void SoundsLike_SearchSingleWord_ReturnsMatchingRecord()
        {
            //Arrange
            
            //Act
            var result = testData.SoundexSearch(x => x.Name).American("Robert");

            //Assert
            CollectionAssert.Contains(result, testData[0]);
        }

        [Test]
        public void SoundsLike_SearchSingleWord_DoesNotReturnsNonMatchingRecords()
        {
            //Arrange
            
            //Act
            var result = testData.SoundexSearch(x => x.Name).American("Robert");

            //Assert
            CollectionAssert.DoesNotContain(result, testData[5]);
        }

        [Test]
        public void SoundsLike_SearchMultipleWords_ReturnsAllMatchingRecords()
        {
            //Arrange
            var names = new[] {"Robert", "Mitt"};
            var soundexCodes = names.Select(w => w.ToSoundex());
            var expected = testData.Where(td => soundexCodes.Contains(td.Name.ToSoundex()));            
            
            //Act
            var result = testData.SoundexSearch(x => x.Name).American("Robert", "Mitt");

            //Assert
            CollectionAssert.AreEqual(expected, result);
        }
    }
}