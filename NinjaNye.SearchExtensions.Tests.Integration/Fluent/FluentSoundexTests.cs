using System;
using System.Linq;
using NUnit.Framework;
using NinjaNye.SearchExtensions.Soundex;

namespace NinjaNye.SearchExtensions.Tests.Integration.Fluent
{
    [TestFixture]
    public class FluentSoundexTests : IDisposable
    {
        private readonly TestContext _context = new TestContext();

        [Test]
        public void SearchSoundex_SoundsLikeSingleWord_OnlyMatchingResultsReturned()
        {
            //Arrange
            var word = "prize";
            var soundex = word.ToSoundex();
            var expected = this._context.TestModels.AsEnumerable().Where(x => x.StringOne.ToSoundex() == soundex);
            
            //Act
            var result = this._context.TestModels.Search(x => x.StringOne).Soundex(word).ToList();

            //Assert
            CollectionAssert.AreEqual(expected, result);
        }

        [Test]
        public void SearchSoundex_SoundsLikeOneOfMultipleWords_OnlyMatchingResultsReturned()
        {
            //Arrange
            var words = new[] {"prize", "ashcraft"};
            var soundex = words.Select(w => w.ToSoundex());
            var expected = this._context.TestModels.AsEnumerable().Where(x => soundex.Contains(x.StringOne.ToSoundex()));
            
            //Act
            var result = this._context.TestModels.Search(x => x.StringOne).Soundex(words).ToList();

            //Assert
            CollectionAssert.AreEqual(expected, result);
        }

        public void Dispose()
        {
            this._context.Dispose();
        }
    }
}