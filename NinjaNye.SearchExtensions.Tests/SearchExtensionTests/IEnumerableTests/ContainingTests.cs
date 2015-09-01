using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting;
using NUnit.Framework;

namespace NinjaNye.SearchExtensions.Tests.SearchExtensionTests.IEnumerableTests
{
    [TestFixture]
    public class ContainingTests
    {
        private List<TestData> testData = new List<TestData>();
        private readonly TestData matchingItem1 = new TestData { Name = "searching this", Description = "chin" };
        private readonly TestData matchingItem2 = new TestData { Name = "searching this", Description = "sea" };
        private readonly TestData matchingItem3 = new TestData { Name = "look here", Description = "sea", Status = "chelsea"};
        private readonly TestData matchingItem4 = new TestData { Name = "in status", Description = "miss", Status = "status"};
        private readonly TestData unmatchingItem = new TestData { Name = "searching this", Description = "no match", Number = 1 };
        private readonly TestData nullItem = new TestData();

        [SetUp]
        public void ClassSetup()
        {
            this.testData = new List<TestData>();
            this.BuildTestData();
        }

        [TearDown]
        public void TearDown()
        {
            this.testData.Clear();
        }

        private void BuildTestData()
        {
            this.testData.Add(this.matchingItem1);
            this.testData.Add(this.unmatchingItem);
            this.testData.Add(this.matchingItem2);
            this.testData.Add(this.matchingItem3);
            this.testData.Add(this.nullItem);
            this.testData.Add(this.matchingItem4);
        }

        [Test]
        public void Containing_CompareAgainstAnotherProperty_DoesNotThrowAnException()
        {
            //Arrange
            
            //Act
            testData.Search(x => x.Name).Containing(x => x.Description);

            //Assert
            Assert.Pass("No exception thrown");
        }

        [Test]
        public void Containing_ComapareAgainstAnotherProperty_DoesNotReturnUnmatchedData()
        {
            //Arrange
            
            //Act
            var result = testData.Search(x => x.Name).Containing(x => x.Description);

            //Assert
            CollectionAssert.DoesNotContain(result, unmatchingItem);
        }

        [Test]
        public void Containing_ComapareAgainstAnotherProperty_ReturnsAllMatchedData()
        {
            //Arrange
            
            //Act
            var result = testData.Search(x => x.Name).Containing(x => x.Description).ToList();

            //Assert
            Assert.AreEqual(2, result.Count);
            CollectionAssert.Contains(result, matchingItem2);
        }

        [Test]
        public void Containing_SearchTwoProperties_ReturnsRecordWithMatchedDataInSecondProperty()
        {
            //Arrange
            
            //Act
            var result = testData.Search(x => x.Name, x => x.Status).Containing(x => x.Description).ToList();

            //Assert
            CollectionAssert.Contains(result, matchingItem3);
        }

        [Test]
        public void Containing_SearchAgainstMultipleProperties_ReturnMatchingItemOnSecondProperty()
        {
            //Arrange
            
            //Act
            var result = testData.Search(x => x.Name).Containing(x => x.Description, x => x.Status).ToList();

            //Assert
            CollectionAssert.Contains(result, matchingItem4);
        }
    }
}