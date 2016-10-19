using System;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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
                                            .ComparedTo("abce", "efgh");

            var firstResult = result.First();
            Assert.AreEqual(2, firstResult.Distances.Length);
            Assert.AreEqual(1, firstResult.Distances[0]);
            Assert.AreEqual(4, firstResult.Distances[1]);
        }

        [Test]
        public void LevenschteinDistanceOf_CompareAgainstTwoProperties_AllDistancesReturned()
        {
            var result = _context.TestModels.Search(x => x.Id).EqualTo(new Guid("2F75BE28-CEC8-46D8-852E-E6DAE5C8F0A3"))
                                            .LevenshteinDistanceOf(x => x.StringOne)
                                            .ComparedTo(x => x.StringTwo, x => x.StringThree);

            var firstResult = result.First();
            Assert.AreEqual(2, firstResult.Distances.Length);
            Assert.AreEqual(9, firstResult.Distances[0]);
            Assert.AreEqual(5, firstResult.Distances[1]);
        }

        [Test]
        public void LevenschteinDistanceOf_CompareAgainstTwoStrings_MinimumDistanceReturned()
        {
            var result = _context.TestModels.Search(x => x.StringOne).EqualTo("abcd")
                                            .LevenshteinDistanceOf(x => x.StringOne)
                                            .ComparedTo("abce", "efgh");

            var firstResult = result.First();
            Assert.AreEqual(2, firstResult.Distances.Length);
            Assert.AreEqual(1, firstResult.MinimumDistance);
        }

        [Test]
        public void LevenschteinDistanceOf_CompareAgainstTwoStrings_MaximumDistanceReturned()
        {
            var result = _context.TestModels.Search(x => x.StringOne).EqualTo("abcd")
                                            .LevenshteinDistanceOf(x => x.StringOne)
                                            .ComparedTo("abce", "efghsdfsdfadgv");

            var firstResult = result.First();
            Assert.AreEqual(2, firstResult.Distances.Length);
            Assert.AreEqual(13, firstResult.MaximumDistance);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
