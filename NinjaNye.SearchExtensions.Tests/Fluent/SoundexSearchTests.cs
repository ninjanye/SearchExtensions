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
        private List<TestData> _testData = new List<TestData>();

        [SetUp]
        public void ClassSetup()
        {
            this._testData = new List<TestData>();
            this.BuildTestData();
        }

        private void BuildTestData()
        {
            this._testData.Add(new TestData {Name = "Robert", Description = "Sounds like Rupert", Number = 1});
            this._testData.Add(new TestData {Name = "Rupert", Description = "SOunds like Robert", Number = 2});
            this._testData.Add(new TestData {Name = "John", Description = "Sounds like Jon", Number = 3});
            this._testData.Add(new TestData {Name = "Jon", Description = "Sounds like John", Number = 4});
            this._testData.Add(new TestData {Name = "Matt", Description = "Sounds like Mitt", Number = 5});
            this._testData.Add(new TestData {Name = "Aschcraft", Description = "Sounds like Ashcroft", Number = 6});
        }

        [Test]
        public void SoundsLike_SearchSingleWord_ReturnsMatchingRecord()
        {
            //Arrange
            
            //Act
            var result = this._testData.Search(x => x.Name).Soundex("Robert");

            //Assert
            CollectionAssert.Contains(result, this._testData[0]);
        }

        [Test]
        public void SoundsLike_SearchSingleWord_DoesNotReturnsNonMatchingRecords()
        {
            //Arrange
            
            //Act
            var result = this._testData.Search(x => x.Name).Soundex("Robert");

            //Assert
            CollectionAssert.DoesNotContain(result, this._testData[5]);
        }

        [Test]
        public void SoundsLike_SearchMultipleWords_ReturnsAllMatchingRecords()
        {
            //Arrange
            var names = new[] {"Robert", "Mitt"};
            var soundexCodes = names.Select(w => w.ToSoundex());
            var expected = this._testData.Where(td => soundexCodes.Contains(td.Name.ToSoundex()));            
            
            //Act
            var result = this._testData.Search(x => x.Name).Soundex("Robert", "Mitt");

            //Assert
            CollectionAssert.AreEqual(expected, result);
        }
    }
}