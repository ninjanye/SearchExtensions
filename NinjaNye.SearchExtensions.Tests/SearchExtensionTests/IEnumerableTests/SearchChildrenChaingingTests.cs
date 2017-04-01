using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace NinjaNye.SearchExtensions.Tests.SearchExtensionTests.IEnumerableTests
{
    public class SearchChildrenChaingingTests
    {
        private ParentTestData _parent;
        private List<ParentTestData> _testData;
        private TestData _dataOne;
        private TestData _dataFour;
        private TestData _dataTwo;
        private TestData _dataThree;
        private ParentTestData _otherParent;

        public SearchChildrenChaingingTests()
        {
            _dataOne = new TestData {Name = "chris", Description = "child data", Number = 1, Age = 60};
            _dataTwo = new TestData {Name = "fred", Description = "child", Number = 20, Age = 30};
            _dataThree = new TestData {Name = "teddy", Description = "data", Number = 2, Age = 40};
            _dataFour = new TestData {Name = "josh", Description = "child data", Number = 20, Age = 50};
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

        [Fact]
        public void SearchChildren_SearchStringAndInteger_ResultsMatchBothOccurences()
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
            Assert.Equal(1, result.Count);
            Assert.Contains(_parent, result);
        }

        [Fact]
        public void SearchChildren_SearchStringAndString_ResultsMatchBothOccurences()
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
            Assert.Equal(1, result.Count);
            Assert.Contains(_parent, result);
        }

        [Fact]
        public void SearchChildren_SearchIntegerAndString_ResultsMatchBothOccurences()
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
            Assert.Equal(1, result.Count);
            Assert.Contains(_parent, result);
        }

        [Fact]
        public void SearchChildren_SearchIntegerAndInteger_ResultsMatchBothOccurences()
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
            Assert.Equal(1, result.Count);
            Assert.Contains(_otherParent, result);
        }
        
    }
}