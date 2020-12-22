using System;
using System.Linq;
using Xunit;

namespace NinjaNye.SearchExtensions.Tests.Integration.Fluent.StructSearchTests
{
    [Collection("Database tests")]
    public class LessThanOrEqualTests
    {
        private readonly TestContext _context;

        public LessThanOrEqualTests(DatabaseIntegrationTests @base)
        {
            _context = @base.Context;
        }

        [Fact]
        public void LessThanOrEqual_CallWithValue_DoesNotThrowAnException()
        {
            //Arrange
            
            //Act

            //Assert
            try
            {
                _context.TestModels.Search(x => x.IntegerOne).LessThanOrEqualTo(10);
            }
            catch (Exception)
            {
                Assert.False(true);
            }
        }

        [Fact]
        public void LessThanOrEqual_CallWithValue_DoesNotReturnNull()
        {
            //Arrange
            
            //Act 
            var result = _context.TestModels.Search(x => x.IntegerOne).LessThanOrEqualTo(50);

            //Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void LessThanOrEqual_CallWithValue_AllRecordsHaveLessThanOrEqualPropertyValues()
        {
            //Arrange
            
            //Act
            var result = _context.TestModels.Search(x => x.IntegerOne).LessThanOrEqualTo(3);

            //Assert
            Assert.NotEmpty(result);
            Assert.True(result.All(x => x.IntegerOne <= 3));
        }

        [Fact]
        public void LessThanOrEqual_SearchMultipleProperties_RecordsFromSecondPropertyMatchRequest()
        {
            //Arrange
            
            //Act
            var result = _context.TestModels.Search(x => x.IntegerOne, x => x.IntegerThree)
                                                .LessThanOrEqualTo(101);

            //Assert
            Assert.True(result.All(x => x.IntegerOne <= 101 || x.IntegerThree <= 101));
        }
    }
}