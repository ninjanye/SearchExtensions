using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace NinjaNye.SearchExtensions.Tests.SearchExtensionTests.IEnumerableTests
{
    public class SearchChildrenTests
    {
        private readonly ParentTestData _parent;
        private readonly List<ParentTestData> _testData;
        private readonly ParentTestData _otherParent;

        public SearchChildrenTests()
        {
            var dataOne = new TestData { Name = "chris", Description = "child data", Number = 1, Age = 20};
            var dataTwo = new TestData { Name = "fred", Description = "child data", Number = 6, Age = 30 };
            var dataThree = new TestData { Name = "teddy", Description = "child data", Number = 2, Age = 40 };
            var dataFour = new TestData { Name = "josh", Description = "child data", Number = 20, Age = 50 };
            _parent = new ParentTestData
            {
                Children = new List<TestData> {dataOne, dataTwo},
                OtherChildren = new List<TestData> { dataThree, dataFour }
            };
            _otherParent = new ParentTestData
            {
                Children = new List<TestData> {dataThree, dataFour},
                OtherChildren = new List<TestData> { dataOne, dataTwo }
            };
            _testData = new List<ParentTestData> { _parent, _otherParent };            
        }

        [Fact]
        public void SearchChild_SearchChildCollectionWithoutProperty_ReturnParent()
        {
            //Arrange

            //Act
            var result = _testData.SearchChildren(p => p.Children);

            //Assert
            Assert.Equal(_testData, result);
        }

        [Fact]
        public void SearchChild_SearchChildCollection_ReturnOnlyParentWhosChildNumberIsGreaterThanTen()
        {
            //Arrange
            
            //Act
            var result = _testData.SearchChildren(p => p.Children)
                                       .With(c => c.Number)
                                       .GreaterThan(10)
                                       .ToList();

            //Assert
            Assert.Single(result);
            Assert.True(result.All(p => p.Children.Any(c => c.Number > 10)));
        }

        [Fact]
        public void SearchChildren_SearchChildCollectionWithPropertyGreaterThan()
        {
            //Arrange
            
            //Act
            var result = _testData.SearchChildren(p => p.Children)
                                       .With(c => c.Number)
                                       .GreaterThan(4)
                                       .ToList();

            //Assert
            Assert.Equal(2, result.Count());
            Assert.Contains(_parent, result);
            Assert.Contains(_otherParent, result);
            Assert.True(result.All(p => p.Children.Any(c => c.Number > 4)));
        }

        [Fact]
        public void SearchChildren_SearchChildCollectionWithPropertyGreaterThanOrEqualTo()
        {
            //Arrange
            
            //Act
            var result = _testData.SearchChildren(p => p.Children)
                                       .With(c => c.Number)
                                       .GreaterThanOrEqualTo(6)
                                       .ToList();

            //Assert
            Assert.Equal(2, result.Count());
            Assert.Contains(_parent, result);
            Assert.Contains(_otherParent, result);
            Assert.True(result.All(p => p.Children.Any(c => c.Number >= 6)));
        }

        [Fact]
        public void SearchChildren_SearchChildCollectionWithPropertyLessThan()
        {
            //Arrange
            
            //Act
            var result = _testData.SearchChildren(p => p.Children)
                                       .With(c => c.Number)
                                       .LessThan(2)
                                       .ToList();

            //Assert
            Assert.Single(result);
            Assert.Contains(_parent, result);
        }

        [Fact]
        public void SearchChildren_SearchChildCollectionWithPropertyLessThanOrEqualTo()
        {
            //Arrange
            
            //Act
            var result = _testData.SearchChildren(p => p.Children)
                                       .With(c => c.Number)
                                       .LessThanOrEqualTo(2)
                                       .ToList();

            //Assert
            Assert.Equal(2, result.Count());
            Assert.Contains(_parent, result);
            Assert.Contains(_otherParent, result);
        }

        [Fact]
        public void SearchChildren_SearchChildCollectionWithPropertyLessThanAndGreaterThan()
        {
            //Arrange
            
            //Act
            var result = _testData.SearchChildren(p => p.Children)
                                       .With(c => c.Number)
                                       .LessThan(10)
                                       .GreaterThan(2)
                                       .ToList();

            //Assert
            Assert.Single(result);
            Assert.Contains(_parent, result);
        }

        [Fact]
        public void SearchChildren_SearchChildCollectionWithPropertyBetween()
        {
            //Arrange
            
            //Act
            var result = _testData.SearchChildren(p => p.Children)
                                       .With(c => c.Number)
                                       .Between(2, 10)
                                       .ToList();

            //Assert
            Assert.Single(result);
            Assert.Contains(_parent, result);
        }

        [Fact]
        public void SearchChildren_SearchChildCollectionWithPropertyEqualTo()
        {
            //Arrange
            
            //Act
            var result = _testData.SearchChildren(p => p.Children)
                                       .With(c => c.Number)
                                       .EqualTo(2)
                                       .ToList();

            //Assert
            Assert.Single(result);
            Assert.Contains(_otherParent, result);
        }

        [Fact]
        public void SearchChildren_SearchChildCollectionWithPropertyEqualToAnyOneOfMultiple()
        {
            //Arrange
            
            //Act
            var result = _testData.SearchChildren(p => p.Children)
                                       .With(c => c.Number)
                                       .EqualTo(2, 6)
                                       .ToList();

            //Assert
            Assert.Equal(2, result.Count());
            Assert.Contains(_parent, result);
            Assert.Contains(_otherParent, result);
        }

        [Fact]
        public void SearchChildren_SearchChildCollectionWithMultiplePropertiesEqualTo()
        {
            //Arrange
            
            //Act
            var result = _testData.SearchChildren(p => p.Children)
                                       .With(c => c.Number, c => c.Age)
                                       .EqualTo(20)
                                       .ToList();

            //Assert
            Assert.Equal(2, result.Count());
            Assert.Contains(_parent, result);
            Assert.Contains(_otherParent, result);
        }

        [Fact]
        public void SearchChildren_SearchMultipleChildCollectionsWithPropertyEqualTo()
        {
            //Arrange

            //Act
            var result = _testData.SearchChildren(p => p.Children, p => p.OtherChildren)
                                       .With(c => c.Number)
                                       .EqualTo(20)
                                       .ToList();

            //Assert
            Assert.Equal(2, result.Count());
            Assert.Contains(_parent, result);
            Assert.Contains(_otherParent, result);
        }

        [Fact]
        public void SearchChildren_SearchMultipleChildCollectionsWithStringPropertyEqualTo()
        {
            //Arrange

            //Act
            var result = _testData.SearchChildren(p => p.Children, p => p.OtherChildren)
                                       .With(c => c.Name)
                                       .EqualTo("chris")
                                       .ToList();

            //Assert
            Assert.Equal(2, result.Count());
            Assert.Contains(_parent, result);
            Assert.Contains(_otherParent, result);
        }
    }
}