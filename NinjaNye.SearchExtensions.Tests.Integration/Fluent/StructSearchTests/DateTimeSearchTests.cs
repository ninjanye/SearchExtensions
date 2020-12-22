using System;
using System.Linq;
using Xunit;

namespace NinjaNye.SearchExtensions.Tests.Integration.Fluent.StructSearchTests
{
    [Collection("Database tests")]
    public class DateTimeSearchTests 
    {
        private readonly TestContext _context;

        public DateTimeSearchTests(DatabaseIntegrationTests @base)
        {
            _context = @base.Context;
        }

        [Fact]
        public void Search_SearchConditionNotSupplied_ReturnsAllData()
        {
            //Arrange
            
            //Act
            var result = _context.TestModels.Search(x => x.Start);

            //Assert
            Assert.Equal(_context.TestModels, result.ToList());
        }

        [Fact]
        public void IsEqual_SearchOnePropertyForMatchingNumber_ReturnsMatchingData()
        {
            //Arrange
            var expected = new DateTime(2010, 1, 1);
            
            //Act
            var result = _context.TestModels.Search(x => x.Start).EqualTo(expected);

            ////Assert
            Assert.NotEmpty(result);
            Assert.True(result.All(x => x.Start == expected));
        }

        [Fact]
        public void IsEqual_SearchTwoValues_ReturnsMatchingDataOnly()
        {
            //Arrange
            var date1 = new DateTime(2020, 1, 1);
            var date2 = new DateTime(2040, 1, 1);
            
            //Act
            var result = _context.TestModels.Search(x => x.End)
                                      .EqualTo(date1, date2);

            ////Assert
            Assert.NotEmpty(result);
            Assert.True(result.All(x => x.End == date1 || x.End == date2));
        }

        [Fact]
        public void IsEqual_SearchTwoProperties_ReturnsMatchingDataOnly()
        {
            //Arrange
            var expected = new DateTime(2030, 1, 1);
            
            //Act
            var result = _context.TestModels.Search(x => x.Start, x => x.End)
                                      .EqualTo(expected);

            ////Assert
            Assert.NotEmpty(result);
            Assert.True(result.All(x => x.Start == expected || x.End == expected));
        }

        [Fact]
        public void GreaterThan_SearchOneProperty_ReturnsOnlyDataWherePropertyIsGreaterThanValue()
        {
            //Arrange
            var expected = new DateTime(2020, 1, 1);

            //Act
            var result = _context.TestModels.Search(x => x.Start).GreaterThan(expected);

            ////Assert
            Assert.NotEmpty(result);
            Assert.True(result.All(x => x.Start > expected));
        }

        [Fact]
        public void GreaterThan_SearchTwoProperties_ReturnsOnlyDataWherePropertyIsGreaterThanValue()
        {
            //Arrange
            var expected = new DateTime(2020, 1, 1);

            //Act
            var result = _context.TestModels.Search(x => x.Start, x => x.End).GreaterThan(expected);

            ////Assert
            Assert.NotEmpty(result);
            Assert.True(result.All(x => x.Start > expected || x.End > expected));
        }

        [Fact]
        public void LessThan_SearchOneProperty_ReturnsOnlyDataWherePropertyIsLessThanValue()
        {
            //Arrange
            var expected = new DateTime(2030, 1, 1);

            //Act
            var result = _context.TestModels.Search(x => x.End).LessThan(expected);  

            ////Assert
            Assert.NotEmpty(result);
            Assert.True(result.All(x => x.End < expected));
        }

        [Fact]
        public void LessThan_SearchTwoProperties_ReturnsOnlyDataWhereEitherPropertyIsLessThanValue()
        {
            //Arrange
            var expected = new DateTime(2030, 1, 1);

            //Act
            var result = _context.TestModels.Search(x => x.Start, x => x.End).LessThan(expected);

            ////Assert
            Assert.NotEmpty(result);
            Assert.True(result.All(x => x.Start < expected || x.End < expected));
        }

        [Fact]
        public void LessThanOrEqual_SearchOneProperty_ReturnsOnlyDataWherePropertyIsLessThanOrEqualToValue()
        {
            //Arrange
            var expected = new DateTime(2030, 1, 1);

            //Act
            var result = _context.TestModels.Search(x => x.Start).LessThanOrEqualTo(expected);

            ////Assert
            Assert.NotEmpty(result);
            Assert.True(result.All(x => x.Start <= expected));
        }

        [Fact]
        public void LessThanOrEqual_SearchTwoProperties_ReturnsOnlyDataWhereEitherPropertyIsLessThanOrEqualToValue()
        {
            //Arrange
            var expected = new DateTime(2030, 1, 1);

            //Act
            var result = _context.TestModels.Search(x => x.Start, x => x.End)
                                      .LessThanOrEqualTo(expected);

            ////Assert
            Assert.NotEmpty(result);
            Assert.True(result.All(x => x.Start <= expected || x.End <= expected));
        }

        [Fact]
        public void GreaterThanOrEqual_SearchOneProperty_ReturnsOnlyDataWherePropertyIsGreaterThanOrEqualToValue()
        {
            //Arrange
            var expected = new DateTime(2030, 1, 1);

            //Act
            var result = _context.TestModels.Search(x => x.Start).GreaterThanOrEqualTo(expected);

            ////Assert
            Assert.NotEmpty(result);
            Assert.True(result.All(x => x.Start >= expected));
        }

        [Fact]
        public void GreaterThanOrEqual_SearchTwoProperties_ReturnsOnlyDataWhereEitherPropertyIsGreaterThanOrEqualToValue()
        {
            //Arrange
            var expected = new DateTime(2020, 1, 1);

            //Act
            var result = _context.TestModels.Search(x => x.Start, x => x.End)
                                      .GreaterThanOrEqualTo(expected);

            ////Assert
            Assert.NotEmpty(result);
            Assert.True(result.All(x => x.Start >= expected || x.End >= expected));
        }

        [Fact]
        public void GreaterThanOrLessThan_SearchOnePropertyBetweenTwoValues_OnlyRecordsBetweenValuesReturned()
        {
            //Arrange
            
            //Act
            var greaterThanDate = new DateTime(2020, 1, 1);
            var lessThanDate = new DateTime(2040, 1, 1);
            var result = _context.TestModels.Search(x => x.Start)
                                      .GreaterThan(greaterThanDate)
                                      .LessThan(lessThanDate);

            //Assert
            Assert.NotEmpty(result);
            Assert.True(result.All(x => x.Start > greaterThanDate && x.Start < lessThanDate));
        }

        [Fact]
        public void Between_SearchTwoPropertiesBetweenTwoValues_OnlyRecordsBetweenValuesReturned()
        {
            //Arrange
            var start = new DateTime(2020, 1, 1);
            var end = new DateTime(2040, 1, 1);
            
            //Act
            var result = _context.TestModels.Search(x => x.Start, x => x.End)
                                      .Between(start, end);

            //Assert
            Assert.NotEmpty(result);
            Assert.True(result.All(x => (x.Start > start && x.Start < end)
                                       || (x.End > start && x.End < end)));
        }
    }
}