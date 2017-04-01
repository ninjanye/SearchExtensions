using System.Linq;
using NinjaNye.SearchExtensions.Soundex;
using Xunit;

namespace NinjaNye.SearchExtensions.Tests.Integration.Fluent
{
    [Collection("Database tests")]
    public class FluentSoundexTests
    {
        private readonly TestContext _context;

        public FluentSoundexTests(DatabaseIntegrationTests @base)
        {
            _context = @base._context;
        }

        [Fact]
        public void SearchSoundex_SoundsLikeSingleWord_OnlyMatchingResultsReturned()
        {
            //Arrange
            var word = "prize";
            var soundex = word.ToSoundex();
            var expected = _context.TestModels.AsEnumerable().Where(x => x.StringOne.ToSoundex() == soundex);
            
            //Act
            var result = _context.TestModels.Search(x => x.StringOne).Soundex(word).ToList();

            //Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void SearchSoundex_SoundsLikeOneOfMultipleWords_OnlyMatchingResultsReturned()
        {
            //Arrange
            var words = new[] {"prize", "ashcraft"};
            var soundex = words.Select(w => w.ToSoundex());
            var expected = _context.TestModels.AsEnumerable().Where(x => soundex.Contains(x.StringOne.ToSoundex()));
            
            //Act
            var result = _context.TestModels.Search(x => x.StringOne).Soundex(words).ToList();

            //Assert
            Assert.Equal(expected, result);
        }
    }
}