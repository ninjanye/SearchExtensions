using System;
using System.Linq;
using NinjaNye.SearchExtensions.Levenshtein;
using NUnit.Framework;

namespace NinjaNye.SearchExtensions.Tests.Integration.Fluent
{
    [TestFixture]
    public class FluentLevenshteinTests : IDisposable
    {
        private readonly TestContext _context = new TestContext();

        [Test]
        public void LevenshteinDistanceOf_GetDistanceToString_ReturnAllRecords()
        {
            //Arrange
            int totalRecords = _context.TestModels.Count();

            //Act
            var result = _context.TestModels.LevenshteinDistanceOf(x => x.StringOne).ComparedTo("test");

            //Assert
            Assert.AreEqual(totalRecords, result.Count());
        }

        [Test]
        public void LevenshteinDistanceOf_GetDistanceToString_DistancesAreCorrect()
        {
            //Arrange

            //Act
            var result = _context.TestModels.LevenshteinDistanceOf(x => x.StringOne).ComparedTo("test");

            //Assert
            Assert.IsTrue(result.All(x => x.Distance == LevenshteinProcessor.LevenshteinDistance(x.Item.StringOne, "test")));
        }

        [Test]
        public void LevenshteinDistanceOf_GetDistanceToProperty_ReturnAllRecords()
        {
            //Arrange
            int totalRecords = _context.TestModels.Count();

            //Act
            var result = _context.TestModels.LevenshteinDistanceOf(x => x.StringOne).ComparedTo(x => x.StringTwo);

            //Assert
            Assert.AreEqual(totalRecords, result.Count());
        }

        [Test]
        public void LevenshteinDistanceOf_GetDistanceToProperty_DistancesAreCorrect()
        {
            //Arrange

            //Act
            var result = _context.TestModels.LevenshteinDistanceOf(x => x.StringOne).ComparedTo(x => x.StringTwo);

            //Assert
            Assert.IsTrue(result.All(x => x.Distance == LevenshteinProcessor.LevenshteinDistance(x.Item.StringOne, x.Item.StringTwo)));
        }

        [Test]
        public void LevenschteinDistanceOf_CompareAgainstTwoStrings_AllDistancesReturned()
        {
            var result = _context.TestModels.Search(x => x.StringOne).EqualTo("abcd")
                                            .LevenshteinDistanceOf(x => x.StringOne)
                                            .ComparedTo("abdc");

            Assert.AreEqual(1, result.First().Distances.Length);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
