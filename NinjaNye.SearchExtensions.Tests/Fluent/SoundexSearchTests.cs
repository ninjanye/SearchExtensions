using System.Collections.Generic;
using System.Linq;
using Xunit;
using NinjaNye.SearchExtensions.Soundex;
using NinjaNye.SearchExtensions.Tests.SearchExtensionTests;

namespace NinjaNye.SearchExtensions.Tests.Fluent
{
    
    public class SoundexSearchTests
    {
        private List<TestData> _testData = new List<TestData>();
        
        public SoundexSearchTests()
        {
            _testData = new List<TestData>();
            BuildTestData();
        }

        private void BuildTestData()
        {
            _testData.Add(new TestData {Name = "Robert", Description = "Sounds like Rupert", Number = 1});
            _testData.Add(new TestData {Name = "Rupert", Description = "SOunds like Robert", Number = 2});
            _testData.Add(new TestData {Name = "John", Description = "Sounds like Jon", Number = 3});
            _testData.Add(new TestData {Name = "Jon", Description = "Sounds like John", Number = 4});
            _testData.Add(new TestData {Name = "Matt", Description = "Sounds like Mitt", Number = 5});
            _testData.Add(new TestData {Name = "Aschcraft", Description = "Sounds like Ashcroft", Number = 6});
        }

        [Fact]
        public void SoundsLike_SearchSingleWord_ReturnsMatchingRecord()
        {
            //Arrange
            
            //Act
            var result = _testData.SoundexOf(x => x.Name).Matching("Robert");

            //Assert
            Assert.Contains(_testData[0], result);
        }

        [Fact]
        public void SoundsLike_SearchSingleWord_DoesNotReturnsNonMatchingRecords()
        {
            //Arrange

            //Act
            var result = _testData.SoundexOf(x => x.Name).Matching("Robert");

            //Assert
            Assert.DoesNotContain(_testData[5], result);
        }

        [Fact]
        public void SoundsLike_SearchMultipleWords_ReturnsAllMatchingRecords()
        {
            //Arrange
            var names = new[] {"Robert", "Mitt"};
            var soundexCodes = names.Select(w => w.ToSoundex());
            var expected = _testData.Where(td => soundexCodes.Contains(td.Name.ToSoundex()));

            //Act
            var result = _testData.SoundexOf(x => x.Name).Matching("Robert", "Mitt");

            //Assert
            Assert.Equal(expected, result);
        }
    }
}