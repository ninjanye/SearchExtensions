using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using NUnit.Framework;

namespace NinjaNye.SearchExtensions.Tests.SearchExtensionTests.IEnumerableTests
{
    [TestFixture]
    public class SearchEnumerableTests
    {
        private readonly List<TestData> testData = new List<TestData>();

        [TestFixtureSetUp]
        public void ClassSetup()
        {
            this.BuildTestData();
        }

        private void BuildTestData()
        {
            this.testData.Add(new TestData { Name = "abcd", Description = "efgh", Number = 1 });
            this.testData.Add(new TestData { Name = "ijkl", Description = "mnop", Number = 2 });
            this.testData.Add(new TestData { Name = "qrst", Description = "uvwx", Number = 3 });
            this.testData.Add(new TestData { Name = "yzab", Description = "cdef", Number = 4 });
        }

        [Test]
        public void Search_SearchTermNotSupplied_AllDataReturned()
        {
            //Arrange
            
            //Act
            var result = testData.Search((string)null, x => x.Name);

            //Assert
            Assert.AreEqual(testData, result);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Search_SearchPropertyNotSupplied_ThrowsArgumentNullException()
        {
            //Arrange
            
            //Act
            testData.Search("test", (Expression<Func<TestData, string>>)null);

            //Assert
            Assert.Fail("Expected an Argument Null expception to occur");
        }

        [Test]
        public void Search_SearchAParticularProperty_OnlyResultsWithAMatchAreReturned()
        {
            //Arrange
            const string searchTerm = "cd";
            
            //Act
            var result = testData.Search(searchTerm, x => x.Name).ToList();

            //Assert
            Assert.IsTrue(result.All(x => x.Name.Contains(searchTerm)));
        }

        [Test]
        public void Search_SearchAParticularPropertyWithMultipleTerms_OnlyResultsWithAMatchAreReturned()
        {
            //Arrange
            const string searchTerm1 = "cd";
            const string searchTerm2 = "jk";
            
            //Act
            var result = testData.Search(new[]{searchTerm1, searchTerm2}, x => x.Name).ToList();

            //Assert
            Assert.IsTrue(result.All(x => x.Name.Contains(searchTerm1) || x.Name.Contains(searchTerm2)));
        }

        [Test]
        public void Search_SearchForATermWithMultipleProperties_OnlyResultsWithAMatchAreReturned()
        {
            //Arrange
            const string searchTerm = "cd";
            
            //Act
            var result = testData.Search(searchTerm, x => x.Name, x => x.Description).ToList();

            //Assert
            Assert.IsTrue(result.All(x => x.Name.Contains(searchTerm) || x.Description.Contains(searchTerm)));
        }

        [Test]
        public void Search_SearchForMultipleTermsWithMultipleProperties_OnlyResultsWithAMatchAreReturned()
        {
            //Arrange
            const string searchTerm1 = "cd";
            const string searchTerm2 = "uv";

            //Act
            var result = testData.Search(new[] { searchTerm1, searchTerm2 }, x => x.Name, x => x.Description).ToList();

            //Assert
            Assert.IsTrue(result.All(x => x.Name.Contains(searchTerm1) 
                                       || x.Name.Contains(searchTerm2)
                                       || x.Description.Contains(searchTerm1) 
                                       || x.Description.Contains(searchTerm2)));
        }

        [Test]
        public void Search_SearchForAnUppercaseTermIgnoringCase_StringComparisonIsRespected()
        {
            //Arrange
            const string searchTerm = "CD";

            //Act
            var result = testData.Search(searchTerm, x => x.Name, StringComparison.InvariantCultureIgnoreCase).ToList();

            //Assert
            Assert.IsTrue(result.All(x => x.Name.Contains(searchTerm.ToLower())));
        }

        [Test]
        public void Search_SearchForAnUppercaseTermRespectingCase_OnlyMatchingResultsAreReturned()
        {
            //Arrange
            const string searchTerm = "CD";
            testData.Add(new TestData { Name = searchTerm });

            //Act
            var result = testData.Search(searchTerm, x => x.Name, StringComparison.Ordinal).ToList();

            //Assert
            Assert.IsTrue(result.All(x => x.Name.Contains(searchTerm)));
        }

        [Test]
        public void Search_SearchAllProperties_AllPropertiesSearched()
        {
            //Arrange
            
            //Act
            var result = testData.Search("cd");

            //Assert
            Assert.AreEqual(2, result.Count());
        }

        private class TestData
        {
            public string Name { get; set; }
            public string Description { get; set; }
            public int Number { get; set; }
        }

    }
}