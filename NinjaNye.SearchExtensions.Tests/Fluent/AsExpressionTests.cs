using System.Collections.Generic;
using System.Linq;
using NinjaNye.SearchExtensions.Tests.SearchExtensionTests;
using Xunit;

namespace NinjaNye.SearchExtensions.Tests.Fluent
{
    public class AsExpressionTests
    {
        private readonly List<TestData> _testData;
        private readonly List<ParentTestData> _parentData;

        public AsExpressionTests()
        {
            _testData = new List<TestData>();
            _parentData = new List<ParentTestData>();
            BuildTestData();
        }

        private void BuildTestData()
        {
            _testData.Add(new TestData { Name = "aaa" });
            _testData.Add(new TestData { Name = "bbb" });
            _testData.Add(new TestData { Name = "ccc" });
            _testData.Add(new TestData { Name = "ddd" });

            var parent1 = new ParentTestData{ Children = new List<TestData>()};
            var parent2 = new ParentTestData { Children = new List<TestData>()};
            parent1.Children.Add(new TestData { Name = "aaa" });
            parent1.Children.Add(new TestData { Name = "bbb" });
            parent2.Children.Add(new TestData { Name = "ccc" });
            parent2.Children.Add(new TestData { Name = "ddd" });
            _parentData.Add(parent1);
            _parentData.Add(parent2);
        }

        [Fact]
        public void AsExpression_DoesNotReturnNull()
        {
            //Arrange

            //Act
            var result = _testData.Search(x => x.Name).Containing("a").AsExpression();

            //Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void AsExpression_PerformedWithoutArgs_ReturnsAllData()
        {
            //Arrange

            //Act
            var expr = _testData.Search(x => x.Name).AsExpression().Compile();
            var result = _testData.Where(expr).ToList();

            //Assert
            Assert.Equal(_testData, result);
        }

        [Fact]
        public void AsExpression_PerformedWithoutArgsOnChildSearch_ReturnsAllData()
        {
            //Arrange

            //Act
            var expr = _parentData.SearchChildren(x => x.Children).With(x => x.Name).AsExpression().Compile();
            var result = _parentData.Where(expr).ToList();

            //Assert
            Assert.Equal(_parentData, result);
        }

        [Fact]
        public void AsExpression_ReturnsCorrectResultForIQueryableSource()
        {
            //Arrange
            var expected = _testData.Search(x => x.Name).Containing("a").ToList();

            //Act
            var expression = _testData.AsQueryable().Search(x => x.Name).Containing("a").AsExpression();
            var result = _testData.AsQueryable().Where(expression).ToList();

            //Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void AsFunc_ReturnsCorrectResult()
        {
            //Arrange
            var expected = _testData.Search(x => x.Name).Containing("a").ToList();

            //Act
            var expression = _testData.Search(x => x.Name).Containing("a").AsExpression().Compile();
            var result = _testData.Where(expression).ToList();

            //Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void AsFunc_ReturnsCorrectResultForNestedSearch()
        {
            //Arrange
            var expected = _parentData.SearchChildren(x => x.Children)
                .With(x => x.Name).Containing("a")
                .ToList();

            //Act
            var expression = _parentData.SearchChildren(x => x.Children)
                .With(x => x.Name).Containing("a").AsExpression().Compile();
            var result = _parentData.Where(expression).ToList();

            //Assert
            Assert.Equal(expected, result);
        }
    }
}