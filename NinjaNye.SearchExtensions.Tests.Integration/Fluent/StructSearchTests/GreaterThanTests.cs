using System;
using System.Linq;
using Xunit;

namespace NinjaNye.SearchExtensions.Tests.Integration.Fluent.StructSearchTests
{
    [Collection("Database tests")]
    public class GreaterThanTests
    {
        private readonly TestContext _context;

        public GreaterThanTests(DatabaseIntegrationTests @base)
        {
            _context = @base.Context;
        }

        [Fact]
        public void GreaterThan_CallWithValue_DoesNotThrowAnException()
        {
            //Arrange
            
            //Act

            //Assert
            try { _context.TestModels.Search(x => x.IntegerOne).GreaterThan(1); } catch (Exception) { Assert.False(true); }
        }

        [Fact]
        public void GreaterThan_CallWithValue_DoesNotReturnNull()
        {
            //Arrange
            
            //Act 
            var result = _context.TestModels.Search(x => x.IntegerOne).GreaterThan(50);

            //Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void GreaterThan_CallWithValue_AllRecordsHaveEqualPropertyValues()
        {
            //Arrange
            
            //Act
            var result = _context.TestModels.Search(x => x.IntegerOne).GreaterThan(10);

            //Assert
            Assert.NotEmpty(result);
            Assert.True(result.All(x => x.IntegerOne > 10));
        }

        [Fact]
        public void GreaterThan_SearchMultipleProperties_RecordsFromSecondPropertyMatchRequest()
        {
            //Arrange
            
            //Act
            var result = _context.TestModels.Search(x => x.IntegerOne, x => x.IntegerThree)
                                                .GreaterThan(3);

            //Assert
            Assert.True(result.All(x => x.IntegerOne > 3 || x.IntegerThree > 3));
        }
    }
}