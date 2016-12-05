using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace NinjaNye.SearchExtensions.Portable.Tests.SearchExtensionTests.IEnumerableTests
{
    [TestFixture]
    public class ContainingTests
    {
        private List<TestData> _testData = new List<TestData>();
        private readonly TestData _matchingItem1 = new TestData { Name = "searching this", Description = "chin" };
        private readonly TestData _matchingItem2 = new TestData { Name = "searching this", Description = "sea" };
        private readonly TestData _matchingItem3 = new TestData { Name = "look here", Description = "sea", Status = "chelsea"};
        private readonly TestData _matchingItem4 = new TestData { Name = "in status", Description = "miss", Status = "status"};
        private readonly TestData _unmatchingItem = new TestData { Name = "searching this", Description = "no match", Number = 1 };
        private readonly TestData _nullItem = new TestData();

        [SetUp]
        public void ClassSetup()
        {
            _testData = new List<TestData>();
            BuildTestData();
        }

        [TearDown]
        public void TearDown()
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

        [Test]
        public void Containing_CompareAgainstAnotherProperty_DoesNotThrowAnException()
        {
            //Arrange
            
            //Act
            _testData.Search(x => x.Name).Containing(x => x.Description);

            //Assert
            Assert.Pass("No exception thrown");
        }

        [Test]
        public void Containing_ComapareAgainstAnotherProperty_DoesNotReturnUnmatchedData()
        {
            //Arrange
            
            //Act
            var result = _testData.Search(x => x.Name).Containing(x => x.Description);

            //Assert
            CollectionAssert.DoesNotContain(result, _unmatchingItem);
        }

        [Test]
        public void Containing_ComapareAgainstAnotherProperty_ReturnsAllMatchedData()
        {
            //Arrange
            
            //Act
            var result = _testData.Search(x => x.Name).Containing(x => x.Description).ToList();

            //Assert
            Assert.AreEqual(2, result.Count);
            CollectionAssert.Contains(result, _matchingItem2);
        }

        [Test]
        public void Containing_SearchTwoProperties_ReturnsRecordWithMatchedDataInSecondProperty()
        {
            //Arrange
            
            //Act
            var result = _testData.Search(x => x.Name, x => x.Status).Containing(x => x.Description).ToList();

            //Assert
            CollectionAssert.Contains(result, _matchingItem3);
        }

        [Test]
        public void Containing_SearchAgainstMultipleProperties_ReturnMatchingItemOnSecondProperty()
        {
            //Arrange
            
            //Act
            var result = _testData.Search(x => x.Name).Containing(x => x.Description, x => x.Status).ToList();

            //Assert
            CollectionAssert.Contains(result, _matchingItem4);
        }
    }
}