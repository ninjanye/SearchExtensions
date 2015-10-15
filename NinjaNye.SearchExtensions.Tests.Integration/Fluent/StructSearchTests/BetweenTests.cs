using System;
using System.Linq;
using NUnit.Framework;

namespace NinjaNye.SearchExtensions.Tests.Integration.Fluent.StructSearchTests
{
    [TestFixture]
    internal class BetweenTests : IDisposable
    {
        private readonly TestContext _context = new TestContext();

        [Test]
        public void Between_CallWithValue_DoesNotThrowAnException()
        {
            //Arrange
            
            //Act

            //Assert
            Assert.DoesNotThrow(() => this._context.TestModels.Search(x => x.IntegerOne).Between(1,3));
        }

        [Test]
        public void Between_CallWithValue_DoesNotReturnNull()
        {
            //Arrange
            
            //Act 
            var result = this._context.TestModels.Search(x => x.IntegerOne).Between(50, 150);

            //Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public void Between_CallWithValue_AllRecordsHaveEqualPropertyValues()
        {
            //Arrange
            
            //Act
            var result = this._context.TestModels.Search(x => x.IntegerOne).Between(10, 100);

            //Assert
            Assert.IsTrue(result.Any());
            Assert.IsTrue(result.All(x => x.IntegerOne > 10 && x.IntegerOne < 100));
        }

        [Test]
        public void Between_SearchMultipleProperties_RecordsFromSecondPropertyMatchRequest()
        {
            //Arrange
            
            //Act
            var result = this._context.TestModels.Search(x => x.IntegerOne, x => x.IntegerThree)
                                                .Between(3, 200);

            //Assert
            Assert.IsTrue(result.All(x => (x.IntegerOne > 3 && x.IntegerOne < 200) 
                                       || (x.IntegerThree > 3 && x.IntegerThree < 200)));
        }

        public void Dispose()
        {
            this._context.Dispose();
        }
    }
}