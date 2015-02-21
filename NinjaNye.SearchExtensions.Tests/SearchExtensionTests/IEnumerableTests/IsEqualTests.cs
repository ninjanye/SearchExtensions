using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace NinjaNye.SearchExtensions.Tests.SearchExtensionTests.IEnumerableTests
{
    [TestFixture]
    public class IsEqualTests
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
            this.testData.Add(new TestData {Name = "abcd", Description = "efgh", Status = "status"});
            this.testData.Add(new TestData {Name = "match", Description = "match", Status = "status"});
            this.testData.Add(new TestData {Name = "match", Description = "status", Status = "status"});
            this.testData.Add(new TestData {Name = "status", Description = "test", Status = "status"});
            this.testData.Add(new TestData {Name = "TEst", Description = "teST", Status = "case"});
        }

        [Test]
        public void IsEqual_CallWithProperty_DoesNotThrowAnException()
        {
            //Arrange
            
            //Act

            //Assert
            Assert.DoesNotThrow(() => this.testData.Search(x => x.Name).EqualTo(x => x.Description));
        }

        [Test]
        public void IsEqual_CallWithProperty_DoesNotReturnNull()
        {
            //Arrange
            
            //Act
            var result = testData.Search(x => x.Name).EqualTo(x => x.Description);

            //Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public void IsEqual_CallWithProperty_OnlyMatchingDataReturned()
        {
            //Arrange
            
            //Act
            var result = testData.Search(x => x.Name).EqualTo(x => x.Description);

            //Assert
            Assert.IsTrue(result.Any());
            Assert.IsTrue(result.All(x => x.Name == x.Description));
        }

        [Test]
        public void IsEqual_CompareTwoProperties_RecordsForSecondPropertyMatchReturned()
        {
            //Arrange
            
            //Act
            var result = testData.Search(x => x.Name, x => x.Description).EqualTo(x => x.Status);

            //Assert
            Assert.IsTrue(result.Any(x => x.Description == x.Status));
        }

        [Test]
        public void IsEqual_CompareAgainstTwoProperties_RecordsForSecondPropertyMatchReturned()
        {
            //Arrange
            
            //Act
            var result = testData.Search(x => x.Name).EqualTo(x => x.Description, x => x.Status);

            //Assert
            Assert.IsTrue(result.Any(x => x.Name == x.Status));
        }

        [Test]
        public void IsEqual_SetCultureToIgnoreCase_MatchedRecordsOfDifferentCaseReturned()
        {
            //Arrange
            
            //Act
            var result = testData.Search(x => x.Name).SetCulture(StringComparison.OrdinalIgnoreCase)
                                                     .EqualTo(x => x.Description);

            //Assert
            Assert.IsTrue(result.Any(x => x.Name == "TEst" && x.Description == "teST"));
        }
    }
}