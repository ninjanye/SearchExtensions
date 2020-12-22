using System;
using System.Linq;
using Xunit;

namespace NinjaNye.SearchExtensions.Tests.Integration.Fluent.StructSearchTests
{
    [Collection("Database tests")]
    public class GreaterThanOrEqualTests
    {
        private readonly TestContext _context;

        public GreaterThanOrEqualTests(DatabaseIntegrationTests @base)
        {
            _context = @base.Context;
        }

        [Fact]
        public void GreaterThanOrEqual_CallWithValue_DoesNotThrowAnException()
        {
            //Arrange
            
            //Act

            //Assert
            try { _context.TestModels.Search(x => x.IntegerOne).GreaterThanOrEqualTo(10); } catch (Exception) { Assert.False(true); }
        }

        [Fact]
        public void GreaterThanOrEqual_CallWithValue_DoesNotReturnNull()
        {
            //Arrange
            
            //Act 
            var result = _context.TestModels.Search(x => x.IntegerOne).GreaterThanOrEqualTo(50);

            //Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void GreaterThanOrEqual_CallWithValue_AllRecordsHaveGreaterThanOrEqualPropertyValues()
        {
            //Arrange
            
            //Act
            var result = _context.TestModels.Search(x => x.IntegerOne).GreaterThanOrEqualTo(3);

            //Assert
            Assert.NotEmpty(result);
            Assert.True(result.All(x => x.IntegerOne >= 3));
        }

        [Fact]
        public void GreaterThanOrEqual_SearchMultipleProperties_RecordsFromSecondPropertyMatchRequest()
        {
            //Arrange
            
            //Act
            var result = _context.TestModels.Search(x => x.IntegerOne, x => x.IntegerThree)
                                                .GreaterThanOrEqualTo(101);

            //Assert
            Assert.True(result.All(x => x.IntegerOne >= 101 || x.IntegerThree >= 101));
        }
    }
}