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
            this.testData.Add(new TestData { Name = "abcd", Description = "efgh", Status = "ijkl", Number = 1 });
            this.testData.Add(new TestData { Name = "a test", Description = "ijkl", Status = "mnop", Number = 2 });
            this.testData.Add(new TestData { Name = "search test", Description = "mnop", Status = "qrst", Number = 3 });
            this.testData.Add(new TestData { Name = "search", Description = "test three", Status = "search", Number = 4 });
            this.testData.Add(new TestData { Name = "search test property match", Description = "test", Status = "match", Number = 5 });
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

        [Test]
        public void ContainingAll_CompareAgainstOneProperty_DoesNotThrowAnException()
        {
            //Arrange
            
            //Act

            //Assert
            Assert.DoesNotThrow(() => testData.Search(x => x.Name).ContainingAll(x => x.Description));
        }

        [Test]
        public void ContainingAll_CompareAgainstOneProperty_DoesNotReturnNull()
        {
            //Arrange
            
            //Act
            var result = testData.Search(x => x.Name).ContainingAll(x => x.Description);

            //Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public void ContainingAll_CompareAgainstOneProperty_ResultNameContainsDescription()
        {
            //Arrange
            
            //Act
            var result = testData.Search(x => x.Name).ContainingAll(x => x.Description).ToList();

            //Assert
            Assert.IsTrue(result.Any());
            Assert.IsTrue(result.All(x => x.Name.Contains(x.Description)));
        }

        [Test]
        public void ContainingAll_CompareAgainstTwoProperties_ResultNameContainsDescriptionAndStatus()
        {
            //Arrange
            
            //Act
            var result = testData.Search(x => x.Name).ContainingAll(x => x.Description, x => x.Status).ToList();

            //Assert
            Assert.IsTrue(result.Any());
            Assert.IsTrue(result.All(x => x.Name.Contains(x.Description) && x.Name.Contains(x.Status)));
        }
    }
}
