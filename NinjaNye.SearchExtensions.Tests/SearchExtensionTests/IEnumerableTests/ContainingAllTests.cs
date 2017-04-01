using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace NinjaNye.SearchExtensions.Tests.SearchExtensionTests.IEnumerableTests
{
    
    public class ContainingAllTests
    {
        private List<TestData> _testData = new List<TestData>();

        public ContainingAllTests()
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
            _testData.Add(new TestData { Name = "abcd", Description = "efgh", Status = "ijkl", Number = 1 });
            _testData.Add(new TestData { Name = "a test", Description = "ijkl", Status = "mnop", Number = 2 });
            _testData.Add(new TestData { Name = "search test", Description = "mnop", Status = "qrst", Number = 3 });
            _testData.Add(new TestData { Name = "search", Description = "test three", Status = "search", Number = 4 });
            _testData.Add(new TestData { Name = "search test property match", Description = "test", Status = "match", Number = 5 });
        }

        [Fact]
        public void ContainingAll_OneTermSupplied_ReturnsRecordNumber2()
        {
            //Arrange

            //Act
            var result = _testData.Search(x => x.Name).ContainingAll("test").ToList();

            //Assert
            Assert.True(result.Any(r => r.Number == 2));
        }

        [Fact]
        public void ContainingAll_TwoTermsSupplied_ReturnsRecordNumber3()
        {
            //Arrange

            //Act
            var result = _testData.Search(x => x.Name).ContainingAll("test", "search").ToList();

            //Assert
            Assert.True(result.Any(r => r.Number == 3));
        }

        [Fact]
        public void ContainingAll_TwoPropertiesAndTwoTermsSupplied_ReturnsRecordNumber3()
        {
            //Arrange

            //Act
            var result = _testData.Search(x => x.Name, x => x.Description)
                                 .ContainingAll("test", "search", "three").ToList();

            //Assert
            Assert.Equal(1, result.Count());
            Assert.True(result.Any(r => r.Number == 4));
        }

        [Fact]
        public void ContainingAll_CompareAgainstOneProperty_DoesNotThrowAnException()
        {
            //Arrange
            
            //Act

            //Assert
            try {
                _testData.Search(x => x.Name).ContainingAll(x => x.Description);
            }
            catch(Exception)
            {
                Assert.False(true);
            }
        }

        [Fact]
        public void ContainingAll_CompareAgainstOneProperty_DoesNotReturnNull()
        {
            //Arrange
            
            //Act
            var result = _testData.Search(x => x.Name).ContainingAll(x => x.Description);

            //Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void ContainingAll_CompareAgainstOneProperty_ResultNameContainsDescription()
        {
            //Arrange
            
            //Act
            var result = _testData.Search(x => x.Name).ContainingAll(x => x.Description).ToList();

            //Assert
            Assert.True(result.Any());
            Assert.True(result.All(x => x.Name.Contains(x.Description)));
        }

        [Fact]
        public void ContainingAll_CompareAgainstTwoProperties_ResultNameContainsDescriptionAndStatus()
        {
            //Arrange
            
            //Act
            var result = _testData.Search(x => x.Name).ContainingAll(x => x.Description, x => x.Status).ToList();

            //Assert
            Assert.True(result.Any());
            Assert.True(result.All(x => x.Name.Contains(x.Description) && x.Name.Contains(x.Status)));
        }
    }
}
