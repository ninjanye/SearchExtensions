using System;
using System.Linq;
using Xunit;

namespace NinjaNye.SearchExtensions.Tests.Integration.Fluent.StructSearchTests
{
    [Collection("Database tests")]
    public class LessThanTests
    {
        private readonly TestContext _context;

        public LessThanTests(DatabaseIntegrationTests @base)
        {
            _context = @base.Context;
        }

        [Fact]
        public void LessThan_CallWithValue_DoesNotThrowAnException()
        {
            //Arrange
            
            //Act

            //Assert
            try
            {
                _context.TestModels.Search(x => x.IntegerOne).LessThan(10);
            }
            catch (Exception)
            {
                Assert.False(true);
            }
        }

        [Fact]
        public void LessThan_CallWithValue_DoesNotReturnNull()
        {
            //Arrange
            
            //Act 
            var result = _context.TestModels.Search(x => x.IntegerOne).LessThan(50);

            //Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void LessThan_CallWithValue_AllRecordsHaveEqualPropertyValues()
        {
            //Arrange
            
            //Act
            var result = _context.TestModels.Search(x => x.IntegerOne).LessThan(10);

            //Assert
            Assert.NotEmpty(result);
            Assert.True(result.All(x => x.IntegerOne < 10));
        }

        [Fact]
        public void LessThan_SearchMultipleProperties_RecordsFromSecondPropertyMatchRequest()
        {
            //Arrange
            
            //Act
            var result = _context.TestModels.Search(x => x.IntegerOne, x => x.IntegerThree)
                                                .LessThan(130);

            //Assert
            Assert.True(result.All(x => x.IntegerOne < 130 || x.IntegerThree < 130));
        }
    }
}