using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace NinjaNye.SearchExtensions.Tests.SearchExtensionTests.IEnumerableTests
{
    [TestFixture]
    public class IntegerSearchTests
    {
        private List<TestData> testData = new List<TestData>();

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
            this.testData.Add(new TestData { Number = 1, Age = 5 });
            this.testData.Add(new TestData { Number = 2, Age = 6 });
            this.testData.Add(new TestData { Number = 3, Age = 7 });
            this.testData.Add(new TestData { Number = 4, Age = 8 });
        }

        [Test]
        public void Search_SearchConditionNotSupplied_ReturnsAllData()
        {
            //Arrange
            
            //Act
            var result = this.testData.Search(x => x.Number);

            //Assert
            CollectionAssert.AreEquivalent(testData, result);
        }

        [Test]
        public void Search_SearchOnePropertyForMatchingNumber_ReturnsMatchingData()
        {
            //Arrange
            
            //Act
            var result = this.testData.Search(x => x.Number).IsEqual(2);

            ////Assert
            Assert.IsTrue(result.Any());
            Assert.IsTrue(result.All(x => x.Number == 2));
        }

        [Test]
        public void Search_SearchTwoValues_ReturnsMatchingDataOnly()
        {
            //Arrange
            
            //Act
            var result = this.testData.Search(x => x.Number).IsEqual(2, 4);

            ////Assert
            Assert.IsTrue(result.Any());
            Assert.IsTrue(result.All(x => x.Number == 2 || x.Number == 4));
        }

        [Test]
        public void Search_SearchTwoProperties_ReturnsMatchingDataOnly()
        {
            //Arrange
            
            //Act
            var result = this.testData.Search(x => x.Number, x => x.Age).IsEqual(5);

            ////Assert
            Assert.IsTrue(result.Any());
            Assert.IsTrue(result.All(x => x.Number == 5 || x.Age == 5));
        }        
    }
}