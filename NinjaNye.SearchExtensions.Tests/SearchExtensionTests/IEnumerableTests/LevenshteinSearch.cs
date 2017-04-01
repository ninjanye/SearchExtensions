using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using NinjaNye.SearchExtensions.Levenshtein;

namespace NinjaNye.SearchExtensions.Tests.SearchExtensionTests.IEnumerableTests
{
    public class LevenshteinSearch : IDisposable
    {
        private List<TestData> _testData = new List<TestData>();
        
        public LevenshteinSearch()
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
            _testData.Add(new TestData {Name = "use", Description = "cdef"});
            _testData.Add(new TestData {Name = "house", Description = "mouse"});
            _testData.Add(new TestData {Name = "mouse", Description = "desc"});
            _testData.Add(new TestData {Name = "test", Description = "house"});
        }

        [Fact]
        public void Levenshtein_GetLevenshteinDistance_AllResultsReturned()
        {
            //Arrange
            
            //Act
            var result = _testData.LevenshteinDistanceOf(x => x.Name)
                                 .ComparedTo(string.Empty)
                                 .ToList();

            //Assert
            Assert.Equal(_testData.Count, result.Count);
        }

        [Fact]
        public void Levenshtein_GetLevenshteinDistance_ResultsOfTypeILevenshteinDistance()
        {
            //Arrange
            
            //Act
            var result = _testData.LevenshteinDistanceOf(x => x.Name)
                                 .ComparedTo(string.Empty)
                                 .ToList();

            //Assert
            Assert.IsAssignableFrom<IEnumerable<ILevenshteinDistance<TestData>>>(result);
        }

        [Fact]
        public void Levenshtein_GetLevenshteinDistanceAgainstEmptyString_DistanceOfFirstItemIsEqualToSourceLength()
        {
            //Arrange
            
            //Act
            var result = _testData.LevenshteinDistanceOf(x => x.Name)
                                 .ComparedTo(string.Empty)
                                 .Take(1)
                                 .ToList();

            //Assert
            Assert.Equal(result[0].Item.Name.Length, result[0].Distance);
        }

        [Fact]
        public void Levenshtein_GetLevenshteinDistanceAgainstEmptyString_DistanceOfSecondItemIsEqualToSourceLength()
        {
            //Arrange

            //Act
            var result = _testData.LevenshteinDistanceOf(x => x.Name)
                                 .ComparedTo(string.Empty)
                                 .Skip(1)
                                 .Take(1)
                                 .ToList();

            //Assert
            Assert.Equal(result[0].Item.Name.Length, result[0].Distance);
        }

        [Fact]
        public void Levenshtein_GetLevenshteinDistanceAgainstEmptyString_AllDistancesAreEqualToSourceLength()
        {
            //Arrange

            //Act
            var result = _testData.LevenshteinDistanceOf(x => x.Name)
                                 .ComparedTo(string.Empty)
                                 .ToList();

            //Assert
            Assert.True(result.All(x => x.Distance == x.Item.Name.Length));
        }

        [Fact]
        public void Levenshtein_GetLevenshteinDistanceAgainstDefinedString_DistanceIsLevenshteinDistance()
        {
            //Arrange
            const string compareTo = "choose";

            //Act
            var result = _testData.LevenshteinDistanceOf(x => x.Name)
                                 .ComparedTo(compareTo)                               
                                 .ToList();

            //Assert
            Assert.True(result.All(x => x.Distance == LevenshteinProcessor.LevenshteinDistance(x.Item.Name, compareTo)));
        }

        [Fact]
        public void Levenshtein_GetLevenshteinDistanceWithoutProperty_ThrowArgumentNullException()
        {
            //Arrange

            //Act

            //Assert
            Assert.Throws<ArgumentNullException>(() => _testData.LevenshteinDistanceOf(null));
        }

        [Fact]
        public void Levenshtein_GetLevenshteinDistanceCompareTo_IncompleteRequestException()
        {
            //Arrange

            //Act

            //Assert
            Assert.Throws<InvalidOperationException>(() => _testData.LevenshteinDistanceOf(x => x.Name).ToList());
        }

        [Fact]
        public void Levenshtein_GetLevenshteinDistanceAgainstDefinedProperty_DistanceIsLevenshteinDistance()
        {
            //Arrange

            //Act
            var result = _testData.LevenshteinDistanceOf(x => x.Name)
                                 .ComparedTo(x => x.Description)
                                 .ToList();

            //Assert
            Assert.True(result.All(x => x.Distance == LevenshteinProcessor.LevenshteinDistance(x.Item.Name, x.Item.Description)));
        }

        [Fact]
        public void Levenshtein_GetLevenshteinDistanceAgainstMultipleDefinedProperties_DistanceIsLevenshteinDistance()
        {
            //Arrange

            //Act
            var result = _testData.LevenshteinDistanceOf(x => x.Name)
                                 .ComparedTo(x => x.Description)
                                 .ToList();

            //Assert
            Assert.True(result.All(x => x.Distance == LevenshteinProcessor.LevenshteinDistance(x.Item.Name, x.Item.Description)));
        }
    }
}