using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using NinjaNye.SearchExtensions.Fluent;
using NinjaNye.SearchExtensions.Tests.SearchExtensionTests;

namespace NinjaNye.SearchExtensions.Tests.Fluent
{
    [TestFixture]
    public class FluentSearchTests
    {
        private List<TestData> testData = new List<TestData>();

        [SetUp]
        public void ClassSetup()
        {
            testData = new List<TestData>();
            this.BuildTestData();
        }

        private void BuildTestData()
        {
            this.testData.Add(new TestData { Name = "abcd", Description = "efgh", Number = 1 });
            this.testData.Add(new TestData { Name = "ijkl", Description = "mnop", Number = 2 });
            this.testData.Add(new TestData { Name = "qrst", Description = "uvwx", Number = 3 });
            this.testData.Add(new TestData { Name = "yzab", Description = "cdef", Number = 4 });
            this.testData.Add(new TestData { Name = "efgh", Description = "ijkl", Number = 5 });
        }

        [Test]
        public void Search_SearchWithoutActionHasNoAffectOnTheResults_ResultsAreUnchanged()
        {
            //Arrange
            
            //Act
            var result = testData.Search(x => x.Name);

            //Assert
            CollectionAssert.AreEquivalent(testData, result);
        }

        [Test]
        public void Search_FluentCallContaining_OnlyResultsContainingTermAreReturned()
        {
            //Arrange
            
            //Act
            var result = testData.Search(x => x.Name).Containing("abc");

            //Assert
            Assert.AreEqual(1, result.Count());
            Assert.IsTrue(result.All(x => x.Name.Contains("abc")));
        }

        [Test]
        public void Search_AfterCallingContainsCantChainStartsWith_OnlyResultsThatContainTextAndStartWithTextAreReturned()
        {
            //Arrange
            
            //Act
            var result = testData.Search(x => x.Name).Containing("b").StartsWith("a");

            //Assert
            Assert.AreEqual(1, result.Count());
            Assert.IsTrue(result.All(x => x.Name.Contains("abc") && x.Name.StartsWith("a")));
        }

        [Test]
        public void Search_AllowEqualsMethod_DefinedPropertyEqualsSearchResult()
        {
            //Arrange

            //Act
            var result = testData.Search(x => x.Name).IsEqual("abcd");

            //Assert
            Assert.AreEqual(1, result.Count());
            Assert.IsTrue(result.All(x => x.Name == "abcd"));
        }

        [Test]
        public void Search_AllowEndsWithMethod_AllResultsEndWithSearchTerm()
        {
            //Arrange
            
            //Act
            var result = testData.Search(x => x.Name).EndsWith("st");

            //Assert
            Assert.AreEqual(1, result.Count());
            Assert.IsTrue(result.All(x => x.Name.EndsWith("st")));
        }

        [Test]
        public void Search_AllowEndsWithAndContainsMethod_AllResultsEndWithSearchTermAndContainSearch()
        {
            //Arrange
            
            //Act
            var result = testData.Search(x => x.Name).EndsWith("st")
                                                     .Containing("qr");

            //Assert
            Assert.AreEqual(1, result.Count());
            Assert.IsTrue(result.All(x => x.Name.EndsWith("st") && x.Name.Contains("qr")));
        }

        [Test]
        public void SearchMultiple_ResultContainsAcrossTwoProperties_ResultContainsTermInEitherProperty()
        {
            //Arrange
            const string searchTerm = "cd";
            
            //Act
            var result = testData.Search(x => x.Name, x => x.Description).Containing(searchTerm);

            //Assert
            Assert.AreEqual(2, result.Count());
            Assert.IsTrue(result.All(x => x.Name.Contains(searchTerm) || x.Description.Contains(searchTerm)));
        }

        [Test]
        public void SearchMultiple_ResultEndsWithAcrossTwoProperties_ResultEndsWithTermInEitherProperty()
        {
            //Arrange
            const string searchTerm = "gh";
            
            //Act
            var result = testData.Search(x => x.Name, x => x.Description).EndsWith(searchTerm);

            //Assert
            Assert.AreEqual(2, result.Count());
            Assert.IsTrue(result.All(x => x.Name.EndsWith(searchTerm) || x.Description.EndsWith(searchTerm)));
        }

        [Test]
        public void SearchMultiple_ResultStartsWithAcrossTwoProperties_ResultStartsWithTermInEitherProperty()
        {
            //Arrange
            const string searchTerm = "ef";
            
            //Act
            var result = testData.Search(x => x.Name, x => x.Description).StartsWith(searchTerm);

            //Assert
            Assert.AreEqual(2, result.Count());
            Assert.IsTrue(result.All(x => x.Name.StartsWith(searchTerm) || x.Description.StartsWith(searchTerm)));
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SearchMultiple_PassNull_ExceptionThrown()
        {
            //Arrange
            
            //Act
            this.testData.Search(null as Expression<Func<TestData, string>>[]).Containing("cd");

            //Assert
            Assert.Fail("ArgumentNullException should be thrown");
        }

        [Test]
        public void SearchAll_NoPropertiesDefined_AllPropertiesAreSearched()
        {
            //Arrange
            
            //Act
            var result = this.testData.SearchStrings().Containing("cd");

            //Assert
            Assert.AreEqual(2, result.Count());
            Assert.IsTrue(result.All(x => x.Name.Contains("cd") || x.Description.Contains("cd")));
        }
    }
}
