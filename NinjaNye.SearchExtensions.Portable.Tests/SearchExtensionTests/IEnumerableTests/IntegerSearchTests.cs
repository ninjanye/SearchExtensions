using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace NinjaNye.SearchExtensions.Portable.Tests.SearchExtensionTests.IEnumerableTests
{
    [TestFixture]
    public class IntegerSearchTests
    {
        private List<TestData> _testData = new List<TestData>();

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
            _testData.Add(new TestData { Number = 1, Age = 5 });
            _testData.Add(new TestData { Number = 2, Age = 6 });
            _testData.Add(new TestData { Number = 3, Age = 7 });
            _testData.Add(new TestData { Number = 4, Age = 8 });
        }

        [Test]
        public void Search_SearchConditionNotSupplied_ReturnsAllData()
        {
            //Arrange
            
            //Act
            var result = _testData.Search(x => x.Number);

            //Assert
            CollectionAssert.AreEquivalent(_testData, result);
        }

        [Test]
        public void IsEqual_SearchOnePropertyForMatchingNumber_ReturnsMatchingData()
        {
            //Arrange
            
            //Act
            var result = _testData.Search(x => x.Number).EqualTo(2);

            ////Assert
            Assert.IsTrue(result.Any());
            Assert.IsTrue(result.All(x => x.Number == 2));
        }

        [Test]
        public void IsEqual_SearchTwoValues_ReturnsMatchingDataOnly()
        {
            //Arrange
            
            //Act
            var result = _testData.Search(x => x.Number).EqualTo(2, 4);

            ////Assert
            Assert.IsTrue(result.Any());
            Assert.IsTrue(result.All(x => x.Number == 2 || x.Number == 4));
        }

        [Test]
        public void IsEqual_SearchTwoProperties_ReturnsMatchingDataOnly()
        {
            //Arrange
            
            //Act
            var result = _testData.Search(x => x.Number, x => x.Age).EqualTo(5);

            ////Assert
            Assert.IsTrue(result.Any());
            Assert.IsTrue(result.All(x => x.Number == 5 || x.Age == 5));
        }

        [Test]
        public void GreaterThan_SearchOneProperty_ReturnsOnlyDataWherePropertyIsGreaterThanValue()
        {
            //Arrange

            //Act
            var result = _testData.Search(x => x.Number).GreaterThan(2);

            ////Assert
            Assert.IsTrue(result.Any());
            Assert.IsTrue(result.All(x => x.Number > 2));
        }

        [Test]
        public void GreaterThan_SearchTwoProperties_ReturnsOnlyDataWherePropertyIsGreaterThanValue()
        {
            //Arrange

            //Act
            var result = _testData.Search(x => x.Number, x => x.Age).GreaterThan(2);

            ////Assert
            Assert.IsTrue(result.Any());
            Assert.IsTrue(result.All(x => x.Number > 2 || x.Age > 2));
        }

        [Test]
        public void LessThan_SearchOneProperty_ReturnsOnlyDataWherePropertyIsLessThanValue()
        {
            //Arrange

            //Act
            var result = _testData.Search(x => x.Number).LessThan(2);

            ////Assert
            Assert.IsTrue(result.Any());
            Assert.IsTrue(result.All(x => x.Number < 2));
        }

        [Test]
        public void LessThan_SearchTwoProperties_ReturnsOnlyDataWhereEitherPropertyIsLessThanValue()
        {
            //Arrange

            //Act
            var result = _testData.Search(x => x.Number, x => x.Age).LessThan(2);

            ////Assert
            Assert.IsTrue(result.Any());
            Assert.IsTrue(result.All(x => x.Number < 2 || x.Age < 2));
        }

        [Test]
        public void LessThanOrEqual_SearchOneProperty_ReturnsOnlyDataWherePropertyIsLessThanOrEqualToValue()
        {
            //Arrange

            //Act
            var result = _testData.Search(x => x.Number).LessThanOrEqualTo(2);

            ////Assert
            Assert.IsTrue(result.Any());
            Assert.IsTrue(result.All(x => x.Number <= 2));
        }

        [Test]
        public void LessThanOrEqual_SearchTwoProperties_ReturnsOnlyDataWhereEitherPropertyIsLessThanOrEqualToValue()
        {
            //Arrange

            //Act
            var result = _testData.Search(x => x.Number, x => x.Age).LessThanOrEqualTo(2);

            ////Assert
            Assert.IsTrue(result.Any());
            Assert.IsTrue(result.All(x => x.Number <= 2 || x.Age <= 2));
        }

        [Test]
        public void GreaterThanOrEqual_SearchOneProperty_ReturnsOnlyDataWherePropertyIsGreaterThanOrEqualToValue()
        {
            //Arrange

            //Act
            var result = _testData.Search(x => x.Number).GreaterThanOrEqualTo(2);

            ////Assert
            Assert.IsTrue(result.Any());
            Assert.IsTrue(result.All(x => x.Number >= 2));
        }

        [Test]
        public void GreaterThanOrEqual_SearchTwoProperties_ReturnsOnlyDataWhereEitherPropertyIsGreaterThanOrEqualToValue()
        {
            //Arrange

            //Act
            var result = _testData.Search(x => x.Number, x => x.Age).GreaterThanOrEqualTo(2);

            ////Assert
            Assert.IsTrue(result.Any());
            Assert.IsTrue(result.All(x => x.Number >= 2 || x.Age >= 2));
        }

        [Test]
        public void GreaterThanOrLessThan_SearchOnePropertyBetweenTwoValues_OnlyRecordsBetweenValuesReturned()
        {
            //Arrange
            
            //Act
            var result = _testData.Search(x => x.Number)
                                      .GreaterThan(2)
                                      .LessThan(4);

            //Assert
            Assert.IsTrue(result.Any());
            Assert.IsTrue(result.All(x => x.Number > 2 && x.Number < 4));
        }

        [Test]
        public void Between_SearchTwoPropertiesBetweenTwoValues_OnlyRecordsBetweenValuesReturned()
        {
            //Arrange
            
            //Act
            var result = _testData.Search(x => x.Number, x => x.Age)
                                      .Between(2, 6);

            //Assert
            Assert.IsTrue(result.Any());
            Assert.IsTrue(result.All(x => (x.Number > 2 && x.Number < 6)
                                       || (x.Age > 2 && x.Age < 6)));
        }

    }
}