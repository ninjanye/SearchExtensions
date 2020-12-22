using System;
using System.Linq;
using NinjaNye.SearchExtensions.Levenshtein;
using Xunit;

namespace NinjaNye.SearchExtensions.Tests.Integration.Fluent
{
    [Collection("Database tests")]
    public class FluentLevenshteinTests
    {
        private readonly TestContext _context;

        public FluentLevenshteinTests(DatabaseIntegrationTests @base)
        {
            _context = @base.Context;
        }

        [Fact]
        public void LevenshteinDistanceOf_GetDistanceToString_ReturnAllRecords()
        {
            //Arrange
            int totalRecords = _context.TestModels.Count();

            //Act
            var result = _context.TestModels.LevenshteinDistanceOf(x => x.StringOne).ComparedTo("test");

            //Assert
            Assert.Equal(totalRecords, result.Count());
        }

        [Fact]
        public void LevenshteinDistanceOf_GetDistanceToString_DistancesAreCorrect()
        {
            //Arrange

            //Act
            var result = _context.TestModels.LevenshteinDistanceOf(x => x.StringOne).ComparedTo("test");

            //Assert
            Assert.True(result.All(x => x.Distance == LevenshteinProcessor.LevenshteinDistance(x.Item.StringOne, "test")));
        }

        [Fact]
        public void LevenshteinDistanceOf_GetDistanceToProperty_ReturnAllRecords()
        {
            //Arrange
            int totalRecords = _context.TestModels.Count();

            //Act
            var result = _context.TestModels.LevenshteinDistanceOf(x => x.StringOne).ComparedTo(x => x.StringTwo);

            //Assert
            Assert.Equal(totalRecords, result.Count());
        }

        [Fact]
        public void LevenshteinDistanceOf_GetDistanceToProperty_DistancesAreCorrect()
        {
            //Arrange

            //Act
            var result = _context.TestModels.LevenshteinDistanceOf(x => x.StringOne).ComparedTo(x => x.StringTwo);

            //Assert
            Assert.True(result.All(x => x.Distance == LevenshteinProcessor.LevenshteinDistance(x.Item.StringOne, x.Item.StringTwo)));
        }

        [Fact]
        public void LevenschteinDistanceOf_CompareAgainstTwoStrings_AllDistancesReturned()
        {
            var result = _context.TestModels.Search(x => x.StringOne).EqualTo("abcd")
                                            .LevenshteinDistanceOf(x => x.StringOne)
                                            .ComparedTo("abce", "efgh");

            var firstResult = result.First();
            Assert.Equal(2, firstResult.Distances.Length);
            Assert.Equal(1, firstResult.Distances[0]);
            Assert.Equal(4, firstResult.Distances[1]);
        }

        [Fact]
        public void LevenschteinDistanceOf_CompareAgainstTwoProperties_AllDistancesReturned()
        {
            var result = _context.TestModels.Search(x => x.Id).EqualTo(new Guid("2F75BE28-CEC8-46D8-852E-E6DAE5C8F0A3"))
                                            .LevenshteinDistanceOf(x => x.StringOne)
                                            .ComparedTo(x => x.StringTwo, x => x.StringThree);

            var firstResult = result.First();
            Assert.Equal(2, firstResult.Distances.Length);
            Assert.Equal(9, firstResult.Distances[0]);
            Assert.Equal(5, firstResult.Distances[1]);
        }

        [Fact]
        public void LevenschteinDistanceOf_CompareAgainstTwoStrings_MinimumDistanceReturned()
        {
            var result = _context.TestModels.Search(x => x.StringOne).EqualTo("abcd")
                                            .LevenshteinDistanceOf(x => x.StringOne)
                                            .ComparedTo("abce", "efgh");

            var firstResult = result.First();
            Assert.Equal(2, firstResult.Distances.Length);
            Assert.Equal(1, firstResult.MinimumDistance);
        }

        [Fact]
        public void LevenschteinDistanceOf_CompareAgainstTwoStrings_MaximumDistanceReturned()
        {
            var result = _context.TestModels.Search(x => x.StringOne).EqualTo("abcd")
                                            .LevenshteinDistanceOf(x => x.StringOne)
                                            .ComparedTo("abce", "efghsdfsdfadgv");

            var firstResult = result.First();
            Assert.Equal(2, firstResult.Distances.Length);
            Assert.Equal(13, firstResult.MaximumDistance);
        }

        [Fact]
        public void LevenschteinDistanceOf_CompareTwoPropertiesToString()
        {
            var result = _context.TestModels.Search(x => x.StringOne).EqualTo("abcd")
                                            .LevenshteinDistanceOf(x => x.StringOne, x => x.StringTwo)
                                            .ComparedTo("abce");

            var firstResult = result.First();
            Assert.Equal(2, firstResult.Distances.Length);
            Assert.Equal(1, firstResult.Distances[0]);
            Assert.Equal(4, firstResult.Distances[1]);
        }

        [Fact]
        public void LevenschteinDistanceOf_CompareTwoPropertiesToProperty()
        {
            var result = _context.TestModels.Search(x => x.Id).EqualTo(new Guid("2F75BE28-CEC8-46D8-852E-E6DAE5C8F0A3"))
                                            .LevenshteinDistanceOf(x => x.StringOne, x => x.StringTwo)
                                            .ComparedTo(x => x.StringThree);

            var firstResult = result.First();
            Assert.Equal(2, firstResult.Distances.Length);
            Assert.Equal(5, firstResult.Distances[0]);
            Assert.Equal(9, firstResult.Distances[1]);
        }

        [Fact]
        public void LevenschteinDistanceOf_CompareTwoPropertiesToTwoStrings()
        {
            var result = _context.TestModels.Search(x => x.StringOne).EqualTo("abcd")
                                            .LevenshteinDistanceOf(x => x.StringOne, x => x.StringTwo)
                                            .ComparedTo("abce", "absdfb");

            var firstResult = result.First();
            Assert.Equal(4, firstResult.Distances.Length);
            Assert.Equal(1, firstResult.Distances[0]);
            Assert.Equal(4, firstResult.Distances[1]);
            Assert.Equal(3, firstResult.Distances[2]);
            Assert.Equal(6, firstResult.Distances[3]);
        }

        [Fact]
        public void LevenschteinDistanceOf_CompareTwoPropertiesToProperties()
        {
            var result = _context.TestModels.Search(x => x.Id).EqualTo(new Guid("2F75BE28-CEC8-46D8-852E-E6DAE5C8F0A3"))
                                            .LevenshteinDistanceOf(x => x.StringOne, x => x.StringTwo)
                                            .ComparedTo(x => x.StringThree, x => x.Id.ToString());

            var firstResult = result.First();
            Assert.Equal(4, firstResult.Distances.Length);
            Assert.Equal(5, firstResult.Distances[0]);
            Assert.Equal(9, firstResult.Distances[1]);
            Assert.Equal(33, firstResult.Distances[2]);
            Assert.Equal(34, firstResult.Distances[3]);
        }
    }
}
