using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace NinjaNye.SearchExtensions.Tests.SearchExtensionTests.IEnumerableTests
{
    public class SearchChildrenChainingTests
    {
        private readonly ParentTestData _parent;
        private readonly List<ParentTestData> _testData;
        private readonly ParentTestData _otherParent;

        public SearchChildrenChainingTests()
        {
            var dataOne = new TestData {Name = "chris", Description = "child data", Number = 1, Age = 60};
            var dataTwo = new TestData {Name = "fred", Description = "child", Number = 20, Age = 30};
            var dataThree = new TestData {Name = "teddy", Description = "data", Number = 2, Age = 40};
            var dataFour = new TestData {Name = "josh", Description = "child data", Number = 20, Age = 50};
            _parent = new ParentTestData
            {
                Children = new List<TestData> {dataOne, dataTwo},
            };
            _otherParent = new ParentTestData
            {
                Children = new List<TestData> {dataThree, dataFour},
            };
            _testData = new List<ParentTestData> {_parent, _otherParent};
        }

        [Fact]
        public void SearchChildren_SearchStringAndInteger_ResultsMatchBothOccurrences()
        {
            //Arrange

            //Act
            var result = _testData.SearchChildren(x => x.Children)
                                            .With(c => c.Name)
                                            .Containing("ed")
                                            .With(c => c.Number)
                                            .EqualTo(20)
                                            .ToList();

            //Assert
            Assert.Single(result);
            Assert.Contains(_parent, result);
        }

        [Fact]
        public void SearchChildren_SearchStringAndString_ResultsMatchBothOccurrences()
        {
            //Arrange

            //Act
            var result = _testData.SearchChildren(x => x.Children)
                                            .With(c => c.Name)
                                            .Containing("ed")
                                            .With(c => c.Description)
                                            .Containing("child")
                                            .ToList();

            //Assert
            Assert.Single(result);
            Assert.Contains(_parent, result);
        }

        [Fact]
        public void SearchChildren_SearchIntegerAndString_ResultsMatchBothOccurrences()
        {
            //Arrange

            //Act
            var result = _testData.SearchChildren(x => x.Children)
                                            .With(c => c.Number)
                                            .EqualTo(20)
                                            .With(c => c.Name)
                                            .Containing("ed")
                                            .ToList();

            //Assert
            Assert.Single(result);
            Assert.Contains(_parent, result);
        }

        [Fact]
        public void SearchChildren_SearchIntegerAndInteger_ResultsMatchBothOccurrences()
        {
            //Arrange

            //Act
            var result = _testData.SearchChildren(x => x.Children)
                                            .With(c => c.Number)
                                            .EqualTo(20)
                                            .With(c => c.Age)
                                            .GreaterThan(40)
                                            .ToList();

            //Assert
            Assert.Single(result);
            Assert.Contains(_otherParent, result);
        }
        
    }
}