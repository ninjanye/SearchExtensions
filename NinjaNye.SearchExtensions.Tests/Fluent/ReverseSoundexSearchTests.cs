using System.Collections.Generic;
using System.Linq;
using Xunit;
using NinjaNye.SearchExtensions.Soundex;
using NinjaNye.SearchExtensions.Tests.SearchExtensionTests;

namespace NinjaNye.SearchExtensions.Tests.Fluent
{
    
    public class ReverseSoundexSearchTests
    {
        private List<TestData> _testData = new List<TestData>();
        
        public ReverseSoundexSearchTests()
        {
            _testData = new List<TestData>();
            BuildTestData();
        }

        private void BuildTestData()
        {
            _testData.Add(new TestData { Name = "arrange", Description = "", Number = 1 });
            _testData.Add(new TestData { Name = "estrange", Description = "", Number = 2 });
            _testData.Add(new TestData { Name = "Taint", Description = "", Number = 3 });
            _testData.Add(new TestData { Name = "Paint", Description = "", Number = 4 });
        }

        [Fact]
        public void SoundsLike_SearchSingleWord_ReturnsMatchingRecord()
        {
            //Arrange

            //Act
            var result = _testData.Search(x => x.Name).ReverseSoundex("range");

            //Assert
            Assert.Contains(_testData[0], result);
        }

        [Fact]
        public void SoundsLike_SearchSingleWord_DoesNotReturnsNonMatchingRecords()
        {
            //Arrange

            //Act
            var result = _testData.Search(x => x.Name).ReverseSoundex("range");

            //Assert
            Assert.DoesNotContain(_testData[3], result);
        }

        [Fact]
        public void SoundsLike_SearchMultipleWords_ReturnsAllMatchingRecords()
        {
            //Arrange
            var names = new[] { "range", "point" };
            var soundexCodes = names.Select(w => w.ToReverseSoundex());
            var expected = _testData.Where(td => soundexCodes.Contains(td.Name.ToReverseSoundex()));

            //Act
            var result = _testData.Search(x => x.Name).ReverseSoundex(names);

            //Assert
            Assert.Equal(expected, result);
        }
    }
}