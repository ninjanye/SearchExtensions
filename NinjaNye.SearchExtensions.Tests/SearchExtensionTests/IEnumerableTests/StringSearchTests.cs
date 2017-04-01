using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace NinjaNye.SearchExtensions.Tests.SearchExtensionTests.IEnumerableTests
{
    public class StringSearchTests : IDisposable
    {
        private List<TestData> _testData = new List<TestData>();

        public StringSearchTests()
        {
            _testData = new List<TestData>();
            BuildTestData();
        }

        public void Dispose()
        {
            _testData.Clear();
        }

        private void BuildTestData()
        {
            _testData.Add(new TestData { Name = "abcd", Description = "efgh", Number = 1 });
            _testData.Add(new TestData { Name = "ijkl", Description = "mnop", Number = 2 });
            _testData.Add(new TestData { Name = "qrst", Description = "uvwx", Number = 3 });
            _testData.Add(new TestData { Name = "yzab", Description = "cdef", Number = 4 });
        }

        [Fact]
        public void Search_SearchTermNotSupplied_AllDataReturned()
        {
            //Arrange
            
            //Act
            var result = _testData.Search(x => x.Name);

            //Assert
            Assert.Equal(_testData, result);
        }

        [Fact]
        public void Search_SearchAParticularProperty_OnlyResultsWithAMatchAreReturned()
        {
            //Arrange
            const string searchTerm = "cd";
            
            //Act
            var result = _testData.Search(x => x.Name).Containing(searchTerm).ToList();

            //Assert
            Assert.True(result.All(x => x.Name.Contains(searchTerm)));
        }

        [Fact]
        public void Search_SearchAParticularPropertyWithMultipleTerms_OnlyResultsWithAMatchAreReturned()
        {
            //Arrange
            const string searchTerm1 = "cd";
            const string searchTerm2 = "jk";
            
            //Act
            var result = _testData.Search(x => x.Name).Containing(searchTerm1, searchTerm2).ToList();

            //Assert
            Assert.True(result.All(x => x.Name.Contains(searchTerm1) || x.Name.Contains(searchTerm2)));
        }

        [Fact]
        public void Search_SearchForATermWithMultipleProperties_OnlyResultsWithAMatchAreReturned()
        {
            //Arrange
            const string searchTerm = "cd";
            
            //Act
            var result = _testData.Search(x => x.Name, x => x.Description)
                                 .Containing(searchTerm)
                                 .ToList();

            //Assert
            Assert.True(result.All(x => x.Name.Contains(searchTerm) || x.Description.Contains(searchTerm)));
        }

        [Fact]
        public void Search_SearchForMultipleTermsWithMultipleProperties_OnlyResultsWithAMatchAreReturned()
        {
            //Arrange
            const string searchTerm1 = "cd";
            const string searchTerm2 = "uv";

            //Act
            var result = _testData.Search(x => x.Name, x => x.Description)
                                 .Containing(searchTerm1, searchTerm2)
                                 .ToList();

            //Assert
            Assert.True(result.All(x => x.Name.Contains(searchTerm1) 
                                       || x.Name.Contains(searchTerm2)
                                       || x.Description.Contains(searchTerm1) 
                                       || x.Description.Contains(searchTerm2)));
        }

        [Fact]
        public void Search_SearchForAnUppercaseTermIgnoringCase_StringComparisonIsRespected()
        {
            //Arrange
            const string searchTerm = "CD";

            //Act
            var result = _testData.Search(x => x.Name)
                                 .SetCulture(StringComparison.OrdinalIgnoreCase)
                                 .Containing(searchTerm)
                                 .ToList();

            //Assert
            Assert.True(result.All(x => x.Name.Contains(searchTerm.ToLower())));
        }

        [Fact]
        public void Search_SearchForAnUppercaseTermRespectingCase_OnlyMatchingResultsAreReturned()
        {
            //Arrange
            const string searchTerm = "CD";
            _testData.Add(new TestData { Name = searchTerm });

            //Act
            var result = _testData.Search(x => x.Name)
                                 .SetCulture(StringComparison.Ordinal)
                                 .Containing(searchTerm)
                                 .ToList();

            //Assert
            Assert.True(result.All(x => x.Name.Contains(searchTerm)));
        }

        [Fact]
        public void Search_SearchWithoutProperties_AllPropertiesSearched()
        {
            //Arrange
            const string searchTerm = "cd";
            
            //Act
            var result = _testData.Search().Containing(searchTerm);

            //Assert
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public void Search_SearchAllPropertiesIgnoreCase_MatchesMadeRegardlessOfCase()
        {
            //Arrange
            const string searchTerm = "CD";
            _testData.Add(new TestData { Name = searchTerm, Description = "test"});
            _testData.Add(new TestData { Name = "test", Description = searchTerm });
            
            //Act
            var result = _testData.Search()
                                 .SetCulture(StringComparison.OrdinalIgnoreCase)
                                 .Containing(searchTerm)
                                 .ToList();

            //Assert
            Assert.True(result.All(x => x.Name.IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) > -1
                                       || x.Description.IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) > -1 ));
        }

        [Fact]
        public void Search_PerformANDSearchAcrossTwoProperties_MatchesOnlyWhereBothAreTrue()
        {
            //Arrange
            const string searchTerm1 = "ab";
            const string searchTerm2 = "ef";
            
            //Act
            var result = _testData.Search(x => x.Name).Containing(searchTerm1)
                                 .Search(x => x.Description).Containing(searchTerm2);

            //Assert
            Assert.True(result.All(x => x.Name.Contains(searchTerm1) && x.Description.Contains(searchTerm2)));

        }
    }
}