using System;
using System.Linq;
using NUnit.Framework;

namespace NinjaNye.SearchExtensions.Tests.Integration.Fluent.StructSearchTests
{
    [TestFixture]
    internal class GreaterThanOrEqualTests : IDisposable
    {
        private readonly TestContext _context = new TestContext();

        [Test]
        public void GreaterThanOrEqual_CallWithValue_DoesNotThrowAnException()
        {
            //Arrange
            
            //Act

            //Assert
            Assert.DoesNotThrow(() => _context.TestModels.Search(x => x.IntegerOne).GreaterThanOrEqualTo(10));
        }

        [Test]
        public void GreaterThanOrEqual_CallWithValue_DoesNotReturnNull()
        {
            //Arrange
            
            //Act 
            var result = _context.TestModels.Search(x => x.IntegerOne).GreaterThanOrEqualTo(50);

            //Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public void GreaterThanOrEqual_CallWithValue_AllRecordsHaveGreaterThanOrEqualPropertyValues()
        {
            //Arrange
            
            //Act
            var result = _context.TestModels.Search(x => x.IntegerOne).GreaterThanOrEqualTo(3);

            //Assert
            Assert.IsTrue(result.Any());
            Assert.IsTrue(result.All(x => x.IntegerOne >= 3));
        }

        [Test]
        public void GreaterThanOrEqual_SearchMultipleProperties_RecordsFromSecondPropertyMatchRequest()
        {
            //Arrange
            
            //Act
            var result = _context.TestModels.Search(x => x.IntegerOne, x => x.IntegerThree)
                                                .GreaterThanOrEqualTo(101);

            //Assert
            Assert.IsTrue(result.All(x => x.IntegerOne >= 101 || x.IntegerThree >= 101));
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}