using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace NinjaNye.SearchExtensions.Tests.SearchExtensionTests.IEnumerableTests
{
    [TestFixture]
    public class SearchChildrenForStringTests
    {
        private ParentTestData _parent;
        private List<ParentTestData> _testData;
        private TestData _dataOne;
        private TestData _dataFour;
        private TestData _dataTwo;
        private TestData _dataThree;
        private ParentTestData _otherParent;

        [SetUp]
        public void SetUp()
        {
            _dataOne = new TestData {Name = "chris", Description = "child data", Number = 1, Age = 20};
            _dataTwo = new TestData {Name = "fred", Description = "nested positionly", Number = 6, Age = 30};
            _dataThree = new TestData {Name = "teddy", Description = "children description", Number = 2, Age = 40};
            _dataFour = new TestData {Name = "josh", Description = "nested data", Number = 20, Age = 50};
            _parent = new ParentTestData
            {
                Children = new List<TestData> {_dataOne, _dataTwo},
            };
            _otherParent = new ParentTestData
            {
                Children = new List<TestData> {_dataThree, _dataFour},
            };
            _testData = new List<ParentTestData> {_parent, _otherParent};
        }

        [Test]
        public void SearchChild_StringEquals_ReturnParentsWIthAnyChildThatMatches()
        {
            //Arrange
            
            //Act
            var result = _testData.SearchChildren(p => p.Children)
                                  .With(c => c.Name)
                                  .EqualTo("chris")
                                  .ToList();

            //Assert
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result, Contains.Item(_parent));
        }

        [Test]
        public void SearchChild_StringEqualsMany_ReturnParentsWIthAnyChildThatMatches()
        {
            //Arrange
            
            //Act
            var result = _testData.SearchChildren(p => p.Children)
                                  .With(c => c.Name)
                                  .EqualTo("chris", "teddy")
                                  .ToList();

            //Assert
            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result, Contains.Item(_parent));
            Assert.That(result, Contains.Item(_otherParent));
        }

        [Test]
        public void SearchChild_WithStringEqualToCaseInsensitive_ReturnParentsWithMatches()
        {
            //Arrange
            
            //Act
            var result = _testData.SearchChildren(p => p.Children)
                                  .With(c => c.Name)
                                  .SetCulture(StringComparison.OrdinalIgnoreCase)
                                  .EqualTo("CHRIS")
                                  .ToList();

            //Assert
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result, Contains.Item(_parent));
        }

        [Test]
        public void SearchChild_WithStringContaining_ReturnParentsWithMatches()
        {
            //Arrange
            
            //Act
            var result = _testData.SearchChildren(p => p.Children)
                                  .With(c => c.Name)
                                  .Containing("ed")
                                  .ToList();

            //Assert
            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result, Contains.Item(_parent));
            Assert.That(result, Contains.Item(_otherParent));
        }

        [Test]
        public void SearchChild_WithStringContaining_IgnoresEmptyStrings()
        {
            //Arrange
            
            //Act
            var result = _testData.SearchChildren(p => p.Children)
                                  .With(c => c.Name)
                                  .Containing("chris", "")
                                  .ToList();

            //Assert
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result, Contains.Item(_parent));
        }

        [Test]
        public void SearchChild_WithStringContainingNoValidSearchTerms_ReturnsAll()
        {
            //Arrange
            
            //Act
            var result = _testData.SearchChildren(p => p.Children)
                                  .With(c => c.Name)
                                  .Containing("")
                                  .ToList();

            //Assert
            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result, Contains.Item(_parent));
            Assert.That(result, Contains.Item(_otherParent));
        }

        [Test]
        public void SearchChild_WithStringContainingWholeWordSearch_ReturnsOnlyMatchesOfEntireWord()
        {
            //Arrange
            
            //Act
            var result = _testData.SearchChildren(p => p.Children)
                                  .With(c => c.Description)
                                  .Matching(SearchType.WholeWords)
                                  .Containing("child")
                                  .ToList();

            //Assert
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result, Contains.Item(_parent));
        }

        [Test]
        public void SearchChild_WithStringContainingAnyOccurenceSearch_ReturnsAllMatches()
        {
            //Arrange
            
            //Act
            var result = _testData.SearchChildren(p => p.Children)
                                  .With(c => c.Description)
                                  .Matching(SearchType.AnyOccurrence)
                                  .Containing("child")
                                  .ToList();

            //Assert
            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result, Contains.Item(_parent));
            Assert.That(result, Contains.Item(_otherParent));
        }

        [Test]
        public void SearchChild_WithStringContainingAllSuppliedWords_ReturnsAllMatches()
        {
            //Arrange
            
            //Act
            var result = _testData.SearchChildren(p => p.Children)
                                  .With(c => c.Description)
                                  .ContainingAll("child", "data")
                                  .ToList();

            //Assert
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result, Contains.Item(_parent));
        }

        [Test]
        public void SearchChild_StringStartsWith_ReturnsAllMatches()
        {
            //Arrange
            
            //Act
            var result = _testData.SearchChildren(p => p.Children)
                                  .With(c => c.Description)
                                  .StartsWith("children")
                                  .ToList();

            //Assert
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result, Contains.Item(_otherParent));
        }

        [Test]
        public void SearchChild_StringEndsWith_ReturnsAllMatches()
        {
            //Arrange
            
            //Act
            var result = _testData.SearchChildren(p => p.Children)
                                  .With(c => c.Description)
                                  .EndsWith("tion")
                                  .ToList();

            //Assert
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result, Contains.Item(_otherParent));
        }
    }
}