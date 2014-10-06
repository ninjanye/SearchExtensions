using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using NUnit.Framework;

namespace NinjaNye.SearchExtensions.Tests.SearchExtensionTests.IEnumerableTests
{
    [TestFixture]
    public class StringSearch
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
            this.testData.Add(new TestData { Name = "ijkl", Description = "mnop", Number = 2 });
            this.testData.Add(new TestData { Name = "qrst", Description = "uvwx", Number = 3 });
            this.testData.Add(new TestData { Name = "yzab", Description = "cdef", Number = 4 });
        }

        [Test]
        public void Search_SearchTermNotSupplied_AllDataReturned()
        {
            //Arrange
            
            //Act
            var result = testData.Search(x => x.Name);

            //Assert
            Assert.AreEqual(testData, result);
        }

        [Test]
        public void Search_SearchAParticularProperty_OnlyResultsWithAMatchAreReturned()
        {
            //Arrange
            const string searchTerm = "cd";
            
            //Act
            var result = testData.Search(x => x.Name).Containing(searchTerm).ToList();

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
            var result = testData.Search(x => x.Name).Containing(searchTerm1, searchTerm2).ToList();

            //Assert
            Assert.IsTrue(result.All(x => x.Name.Contains(searchTerm1) || x.Name.Contains(searchTerm2)));
        }

        [Test]
        public void Search_SearchForATermWithMultipleProperties_OnlyResultsWithAMatchAreReturned()
        {
            //Arrange
            const string searchTerm = "cd";
            
            //Act
            var result = testData.Search(x => x.Name, x => x.Description)
                                 .Containing(searchTerm)
                                 .ToList();

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
            var result = testData.Search(x => x.Name, x => x.Description)
                                 .Containing(searchTerm1, searchTerm2)
                                 .ToList();

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
            var result = testData.Search(x => x.Name)
                                 .SetCulture(StringComparison.InvariantCultureIgnoreCase)
                                 .Containing(searchTerm)
                                 .ToList();

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
            var result = testData.Search(x => x.Name)
                                 .SetCulture(StringComparison.Ordinal)
                                 .Containing(searchTerm)
                                 .ToList();

            //Assert
            Assert.IsTrue(result.All(x => x.Name.Contains(searchTerm)));
        }

        [Test]
        public void Search_SearchWithoutProperties_AllPropertiesSearched()
        {
            //Arrange
            const string searchTerm = "cd";
            
            //Act
            var result = this.testData.Search().Containing(searchTerm);

            //Assert
            Assert.AreEqual(2, result.Count());
        }

        [Test]
        public void Search_SearchAllPropertiesIgnoreCase_MatchesMadeRegardlessOfCase()
        {
            //Arrange
            const string searchTerm = "CD";
            testData.Add(new TestData { Name = searchTerm, Description = "test"});
            testData.Add(new TestData { Name = "test", Description = searchTerm });
            
            //Act
            var result = testData.Search()
                                 .SetCulture(StringComparison.OrdinalIgnoreCase)
                                 .Containing(searchTerm)
                                 .ToList();

            //Assert
            Assert.IsTrue(result.All(x => x.Name.IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) > -1
                                       || x.Description.IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) > -1 ));
        }

        [Test]
        public void Search_PerformANDSearchAcrossTwoProperties_MatchesOnlyWhereBothAreTrue()
        {
            //Arrange
            const string searchTerm1 = "ab";
            const string searchTerm2 = "ef";
            
            //Act
            var result = testData.Search(x => x.Name).Containing(searchTerm1)
                                 .Search(x => x.Description).Containing(searchTerm2);

            //Assert
            Assert.IsTrue(result.All(x => x.Name.Contains(searchTerm1) && x.Description.Contains(searchTerm2)));

        }
    }
}