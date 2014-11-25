using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace NinjaNye.SearchExtensions.Tests.SearchExtensionTests.IEnumerableTests
{
    [TestFixture]
    public class ContainingAllTests
    {
        private List<TestData> testData = new List<TestData>();

        [SetUp]
        public void ClassSetup()
        {
            testData = new List<TestData>();
            this.BuildTestData();
        }

        [TearDown]
        public void TearDown()
        {
            testData.Clear();
        }

        private void BuildTestData()
        {
            this.testData.Add(new TestData { Name = "abcd", Description = "efgh", Number = 1 });
            this.testData.Add(new TestData { Name = "a test", Description = "ijkl", Number = 2 });
            this.testData.Add(new TestData { Name = "search test", Description = "mnop", Number = 3 });
            this.testData.Add(new TestData { Name = "search", Description = "test three", Number = 4 });
        }

        [Test]
        public void ContainingAll_NoTermsSupplied_ReturnsAllData()
        {
            //Arrange
            
            //Act
            var result = testData.Search(x => x.Name).ContainingAll().ToList();

            //Assert
            Assert.AreEqual(testData, result);
        }

        [Test]
        public void ContainingAll_OneTermSupplied_ReturnsRecordNumber2()
        {
            //Arrange

            //Act
            var result = testData.Search(x => x.Name).ContainingAll("test").ToList();

            //Assert
            Assert.IsTrue(result.Any(r => r.Number == 2));
        }

        [Test]
        public void ContainingAll_TwoTermsSupplied_ReturnsRecordNumber3()
        {
            //Arrange

            //Act
            var result = testData.Search(x => x.Name).ContainingAll("test", "search").ToList();

            //Assert
            Assert.IsTrue(result.Any(r => r.Number == 3));
        }

        [Test]
        public void ContainingAll_TwoPropertiesAndTwoTermsSupplied_ReturnsRecordNumber3()
        {
            //Arrange

            //Act
            var result = testData.Search(x => x.Name, x => x.Description)
                                 .ContainingAll("test", "search", "three").ToList();

            //Assert
            Assert.AreEqual(1, result.Count());
            Assert.IsTrue(result.Any(r => r.Number == 4));
        }
    }
}
