﻿using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using NinjaNye.SearchExtensions.Portable.Levenshtein;

namespace NinjaNye.SearchExtensions.Portable.Tests.SearchExtensionTests.IEnumerableTests
{
    [TestFixture]
    public class LevenshteinSearch
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
            _testData.Add(new TestData {Name = "use", Description = "cdef"});
            _testData.Add(new TestData {Name = "house", Description = "mouse"});
            _testData.Add(new TestData {Name = "mouse", Description = "desc"});
            _testData.Add(new TestData {Name = "test", Description = "house"});
        }

        [Test]
        public void Levenshtein_GetLevenshteinDistance_AllResultsReturned()
        {
            //Arrange
            
            //Act
            var result = _testData.LevenshteinDistanceOf(x => x.Name)
                                 .ComparedTo(string.Empty)
                                 .ToList();

            //Assert
            Assert.AreEqual(_testData.Count, result.Count);
        }

        [Test]
        public void Levenshtein_GetLevenshteinDistance_ResultsOfTypeILevenshteinDistance()
        {
            //Arrange
            
            //Act
            var result = _testData.LevenshteinDistanceOf(x => x.Name)
                                 .ComparedTo(string.Empty)
                                 .ToList();

            //Assert
            Assert.IsInstanceOf<IEnumerable<ILevenshteinDistance<TestData>>>(result);
        }

        [Test]
        public void Levenshtein_GetLevenshteinDistanceAgainstEmptyString_DistanceOfFirstItemIsEqualToSourceLength()
        {
            //Arrange
            
            //Act
            var result = _testData.LevenshteinDistanceOf(x => x.Name)
                                 .ComparedTo(string.Empty)
                                 .Take(1)
                                 .ToList();

            //Assert
            Assert.AreEqual(result[0].Item.Name.Length, result[0].Distance);
        }

        [Test]
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
            Assert.AreEqual(result[0].Item.Name.Length, result[0].Distance);
        }

        [Test]
        public void Levenshtein_GetLevenshteinDistanceAgainstEmptyString_AllDistancesAreEqualToSourceLength()
        {
            //Arrange

            //Act
            var result = _testData.LevenshteinDistanceOf(x => x.Name)
                                 .ComparedTo(string.Empty)
                                 .ToList();

            //Assert
            Assert.IsTrue(result.All(x => x.Distance == x.Item.Name.Length));
        }

        [Test]
        public void Levenshtein_GetLevenshteinDistanceAgainstDefinedString_DistanceIsLevenshteinDistance()
        {
            //Arrange
            const string compareTo = "choose";

            //Act
            var result = _testData.LevenshteinDistanceOf(x => x.Name)
                                 .ComparedTo(compareTo)                               
                                 .ToList();

            //Assert
            Assert.IsTrue(result.All(x => x.Distance == LevenshteinProcessor.LevenshteinDistance(x.Item.Name, compareTo)));
        }

        [Test]
        public void Levenshtein_GetLevenshteinDistanceWithoutProperty_ThrowArgumentNullException()
        {
            //Arrange

            //Act

            //Assert
            Assert.Throws<ArgumentNullException>(() => _testData.LevenshteinDistanceOf(null));
        }

        [Test]
        public void Levenshtein_GetLevenshteinDistanceCompareTo_IncompleteRequestException()
        {
            //Arrange

            //Act

            //Assert
            Assert.Throws<InvalidOperationException>(() => _testData.LevenshteinDistanceOf(x => x.Name).ToList());
        }

        [Test]
        public void Levenshtein_GetLevenshteinDistanceAgainstDefinedProperty_DistanceIsLevenshteinDistance()
        {
            //Arrange

            //Act
            var result = _testData.LevenshteinDistanceOf(x => x.Name)
                                 .ComparedTo(x => x.Description)
                                 .ToList();

            //Assert
            Assert.IsTrue(result.All(x => x.Distance == LevenshteinProcessor.LevenshteinDistance(x.Item.Name, x.Item.Description)));
        }

        [Test]
        public void Levenshtein_GetLevenshteinDistanceAgainstMultipleDefinedProperties_DistanceIsLevenshteinDistance()
        {
            //Arrange

            //Act
            var result = _testData.LevenshteinDistanceOf(x => x.Name)
                                 .ComparedTo(x => x.Description)
                                 .ToList();

            //Assert
            Assert.IsTrue(result.All(x => x.Distance == LevenshteinProcessor.LevenshteinDistance(x.Item.Name, x.Item.Description)));
        }
    }
}