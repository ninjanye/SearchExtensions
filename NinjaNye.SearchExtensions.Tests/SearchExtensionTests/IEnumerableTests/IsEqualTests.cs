using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace NinjaNye.SearchExtensions.Tests.SearchExtensionTests.IEnumerableTests
{
    public class IsEqualTests : IDisposable
    {
        private List<TestData> _testData = new List<TestData>();

        public IsEqualTests()
        {
            _testData = new List<TestData>();
            BuildTestData();
        }

        public void Dispose()
        {
            _testData.Clear();
        }

        private void BuildTestData()
        {
            _testData.Add(new TestData {Name = "abcd", Description = "efgh", Status = "status"});
            _testData.Add(new TestData {Name = "match", Description = "match", Status = "status"});
            _testData.Add(new TestData {Name = "match", Description = "status", Status = "status"});
            _testData.Add(new TestData {Name = "status", Description = "test", Status = "status"});
            _testData.Add(new TestData {Name = "TEst", Description = "teST", Status = "case"});
        }

        [Fact]
        public void IsEqual_CallWithProperty_DoesNotThrowAnException()
        {
            //Arrange
            
            //Act

            //Assert
            try { _testData.Search(x => x.Name).EqualTo(x => x.Description); } catch (Exception) { Assert.False(true); }
        }

        [Fact]
        public void IsEqual_CallWithProperty_DoesNotReturnNull()
        {
            //Arrange
            
            //Act
            var result = _testData.Search(x => x.Name).EqualTo(x => x.Description);

            //Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void IsEqual_CallWithProperty_OnlyMatchingDataReturned()
        {
            //Arrange
            
            //Act
            var result = _testData.Search(x => x.Name).EqualTo(x => x.Description);

            //Assert
            Assert.True(result.Any());
            Assert.True(result.All(x => x.Name == x.Description));
        }

        [Fact]
        public void IsEqual_CompareTwoProperties_RecordsForSecondPropertyMatchReturned()
        {
            //Arrange
            
            //Act
            var result = _testData.Search(x => x.Name, x => x.Description).EqualTo(x => x.Status);

            //Assert
            Assert.True(result.Any(x => x.Description == x.Status));
        }

        [Fact]
        public void IsEqual_CompareAgainstTwoProperties_RecordsForSecondPropertyMatchReturned()
        {
            //Arrange
            
            //Act
            var result = _testData.Search(x => x.Name).EqualTo(x => x.Description, x => x.Status);

            //Assert
            Assert.True(result.Any(x => x.Name == x.Status));
        }

        [Fact]
        public void IsEqual_SetCultureToIgnoreCase_MatchedRecordsOfDifferentCaseReturned()
        {
            //Arrange
            
            //Act
            var result = _testData.Search(x => x.Name).SetCulture(StringComparison.OrdinalIgnoreCase)
                                                     .EqualTo(x => x.Description);

            //Assert
            Assert.True(result.Any(x => x.Name == "TEst" && x.Description == "teST"));
        }
    }
}