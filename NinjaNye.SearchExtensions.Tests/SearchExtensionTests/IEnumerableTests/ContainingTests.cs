using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace NinjaNye.SearchExtensions.Tests.SearchExtensionTests.IEnumerableTests
{
    public class ContainingTests : IDisposable
    {
        private List<TestData> _testData = new List<TestData>();
        private readonly TestData _matchingItem1 = new TestData { Name = "searching this", Description = "chin" };
        private readonly TestData _matchingItem2 = new TestData { Name = "searching this", Description = "sea" };
        private readonly TestData _matchingItem3 = new TestData { Name = "look here", Description = "sea", Status = "chelsea"};
        private readonly TestData _matchingItem4 = new TestData { Name = "in status", Description = "miss", Status = "status"};
        private readonly TestData _unmatchingItem = new TestData { Name = "searching this", Description = "no match", Number = 1 };
        private readonly TestData _nullItem = new TestData();

        public ContainingTests()
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
            _testData.Add(_matchingItem1);
            _testData.Add(_unmatchingItem);
            _testData.Add(_matchingItem2);
            _testData.Add(_matchingItem3);
            _testData.Add(_nullItem);
            _testData.Add(_matchingItem4);
        }

        [Fact]
        public void Containing_CompareAgainstAnotherProperty_DoesNotThrowAnException()
        {
            //Arrange
            
            //Act
            _testData.Search(x => x.Name).Containing(x => x.Description);

            //Assert
            Assert.True(true, "No exception thrown");
        }

        [Fact]
        public void Containing_ComapareAgainstAnotherProperty_DoesNotReturnUnmatchedData()
        {
            //Arrange
            
            //Act
            var result = _testData.Search(x => x.Name).Containing(x => x.Description);

            //Assert
            Assert.DoesNotContain(_unmatchingItem, result);
        }

        [Fact]
        public void Containing_ComapareAgainstAnotherProperty_ReturnsAllMatchedData()
        {
            //Arrange
            
            //Act
            var result = _testData.Search(x => x.Name).Containing(x => x.Description).ToList();

            //Assert
            Assert.Equal(2, result.Count);
            Assert.Contains(_matchingItem2, result);
        }

        [Fact]
        public void Containing_SearchTwoProperties_ReturnsRecordWithMatchedDataInSecondProperty()
        {
            //Arrange
            
            //Act
            var result = _testData.Search(x => x.Name, x => x.Status).Containing(x => x.Description).ToList();

            //Assert
            Assert.Contains(_matchingItem3, result);
        }

        [Fact]
        public void Containing_SearchAgainstMultipleProperties_ReturnMatchingItemOnSecondProperty()
        {
            //Arrange
            
            //Act
            var result = _testData.Search(x => x.Name).Containing(x => x.Description, x => x.Status).ToList();

            //Assert
            Assert.Contains(_matchingItem4, result);
        }
    }
}