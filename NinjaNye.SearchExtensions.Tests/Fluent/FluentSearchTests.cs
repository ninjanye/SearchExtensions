using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using NUnit.Framework;
using NinjaNye.SearchExtensions.Tests.SearchExtensionTests;

namespace NinjaNye.SearchExtensions.Tests.Fluent
{
    [TestFixture]
    public class FluentSearchTests
    {
        private List<TestData> testData = new List<TestData>();

        [SetUp]
        public void ClassSetup()
        {
            testData = new List<TestData>();
            this.BuildTestData();
        }

        private void BuildTestData()
        {
            this.testData.Add(new TestData { Name = "abcd", Description = "efgh", Number = 1 });
            this.testData.Add(new TestData { Name = "ijkl", Description = "mnop", Number = 2 });
            this.testData.Add(new TestData { Name = "qrst", Description = "uvwx", Number = 3 });
            this.testData.Add(new TestData { Name = "yzab", Description = "cdef", Number = 4 });
            this.testData.Add(new TestData { Name = "efgh", Description = "ijkl", Number = 5 });
            this.testData.Add(new TestData { Name = "UPPER", Description = "CASE", Number = 6 });
            this.testData.Add(new TestData { Name = "lower", Description = "case", Number = 7 });
            this.testData.Add(new TestData { Name = "tastiest", Description = "two occurences of st", Number = 8 });
        }

        [Test]
        public void Search_SearchWithoutActionHasNoAffectOnTheResults_ResultsAreUnchanged()
        {
            //Arrange
            
            //Act
            var result = testData.Search(x => x.Name);

            //Assert
            CollectionAssert.AreEquivalent(testData, result);
        }

        [Test]
        public void Search_FluentCallContaining_OnlyResultsContainingTermAreReturned()
        {
            //Arrange
            
            //Act
            var result = testData.Search(x => x.Name).Containing("abc");

            //Assert
            Assert.AreEqual(1, result.Count());
            Assert.IsTrue(result.All(x => x.Name.Contains("abc")));
        }

        [Test]
        public void Search_AfterCallingContainsChainStartsWith_OnlyResultsThatContainTextAndStartWithTextAreReturned()
        {
            //Arrange
            
            //Act
            var result = testData.Search(x => x.Name).Containing("b").StartsWith("a");

            //Assert
            Assert.AreEqual(1, result.Count());
            Assert.IsTrue(result.All(x => x.Name.Contains("abc") && x.Name.StartsWith("a")));
        }

        [Test]
        public void Search_AllowEqualsMethod_DefinedPropertyEqualsSearchResult()
        {
            //Arrange

            //Act
            var result = testData.Search(x => x.Name).IsEqual("abcd");

            //Assert
            Assert.AreEqual(1, result.Count());
            Assert.IsTrue(result.All(x => x.Name == "abcd"));
        }

        [Test]
        public void Search_AllowEndsWithAndContainsMethod_AllResultsEndWithSearchTermAndContainSearch()
        {
            //Arrange
            
            //Act
            var result = testData.Search(x => x.Name).EndsWith("st")
                                                     .Containing("qr");

            //Assert
            Assert.AreEqual(1, result.Count());
            Assert.IsTrue(result.All(x => x.Name.EndsWith("st") && x.Name.Contains("qr")));
        }

        [Test]
        public void SearchMultiple_ResultContainsAcrossTwoProperties_ResultContainsTermInEitherProperty()
        {
            //Arrange
            const string searchTerm = "cd";
            
            //Act
            var result = testData.Search(x => x.Name, x => x.Description).Containing(searchTerm);

            //Assert
            Assert.AreEqual(2, result.Count());
            Assert.IsTrue(result.All(x => x.Name.Contains(searchTerm) || x.Description.Contains(searchTerm)));
        }

        [Test]
        public void SearchMultiple_ResultStartsWithAcrossTwoProperties_ResultStartsWithTermInEitherProperty()
        {
            //Arrange
            const string searchTerm = "ef";
            
            //Act
            var result = testData.Search(x => x.Name, x => x.Description).StartsWith(searchTerm);

            //Assert
            Assert.AreEqual(2, result.Count());
            Assert.IsTrue(result.All(x => x.Name.StartsWith(searchTerm) || x.Description.StartsWith(searchTerm)));
        }

        [Test]
        public void SearchAll_NoPropertiesDefined_AllPropertiesAreSearched()
        {
            //Arrange
            
            //Act
            var result = this.testData.Search().Containing("cd");

            //Assert
            Assert.AreEqual(2, result.Count());
            Assert.IsTrue(result.All(x => x.Name.Contains("cd") || x.Description.Contains("cd")));
        }

        [Test]
        public void Search_ContainingMultipleTerms_SearchAgainstMultipleTerms()
        {
            //Arrange

            //Act
            var result = testData.Search(x => x.Name).Containing("ab", "jk");

            //Assert
            Assert.AreEqual(3, result.Count());
            Assert.IsTrue(result.All(x => x.Name.Contains("ab") || x.Name.Contains("jk")));
        }

        [Test]
        public void Search_StartsWithMultipleTerms_SearchAgainstMultipleTerms()
        {
            //Arrange

            //Act
            var result = testData.Search(x => x.Name).StartsWith("ab", "ef");

            //Assert
            Assert.AreEqual(2, result.Count());
            Assert.IsTrue(result.All(x => x.Name.StartsWith("ab") || x.Name.StartsWith("ef")));

        }

        [Test]
        public void Search_SearchManyPropertiesContainingManyTerms_AllResultsHaveASearchTermWithin()
        {
            //Arrange
            
            //Act
            var result = testData.Search(x => x.Name, x => x.Description).Containing("cd", "jk");

            //Assert
            Assert.AreEqual(4, result.Count());
            Assert.IsTrue(result.All(x => x.Name.Contains("cd") || x.Name.Contains("jk") 
                                       || x.Description.Contains("cd") || x.Description.Contains("jk")));
        }

        [Test]
        public void Search_SearchManyPropertiesStartingWithManyTerms_AllResultsHaveAPropertyStartingWithASpecifiedTerm()
        {
            //Arrange
            
            //Act
            var result = testData.Search(x => x.Name, x => x.Description).StartsWith("cd", "ef");

            //Assert
            Assert.AreEqual(3, result.Count());
            Assert.IsTrue(result.All(x => x.Name.StartsWith("cd") || x.Name.StartsWith("ef")
                                       || x.Description.StartsWith("cd") || x.Description.StartsWith("ef")));
        }

        [Test]
        public void Search_SearchManyPropertiesEndingWithManyTerms_AllResultsHaveAPropertyEndingWithASpecifiedTerm()
        {
            //Arrange
            
            //Act
            var result = testData.Search(x => x.Name, x => x.Description).EndsWith("kl", "ef");

            //Assert
            Assert.AreEqual(3, result.Count());
            Assert.IsTrue(result.All(x => x.Name.EndsWith("kl") || x.Name.EndsWith("ef")
                                       || x.Description.EndsWith("kl") || x.Description.EndsWith("ef")));
        }

        [Test]
        public void Search_SearchContainingWithOrdinalStringComparison_OnlyMatchingCaseIsReturned()
        {
            //Arrange
            
            //Act
            var result = testData.Search(x => x.Name).SetCulture(StringComparison.Ordinal).Containing("AB", "jk");

            //Assert
            Assert.AreEqual(1, result.Count());
            Assert.IsTrue(result.All(x => x.Name.Contains("jk")));
            Assert.IsFalse(result.Any(x => x.Name.Contains("AB")));
        }

        [Test]
        public void Search_SearchContainingWithOrdinalIgnoreCaseStringComparison_CaseIsIgnored()
        {
            //Arrange
            
            //Act
            var result = testData.Search(x => x.Name).SetCulture(StringComparison.OrdinalIgnoreCase).Containing("AB", "jk");

            //Assert
            Assert.AreEqual(3, result.Count());
            Assert.IsTrue(result.All(x => x.Name.Contains("jk") || x.Name.Contains("ab")));
        }

        [Test]
        public void Search_SearchStartsWithOrdinalStringComparison_OnlyMatchingCaseIsReturned()
        {
            //Arrange
            
            //Act
            var result = testData.Search(x => x.Description).SetCulture(StringComparison.Ordinal).StartsWith("C");

            //Assert
            Assert.AreEqual(1, result.Count());
            Assert.IsTrue(result.All(x => x.Description.StartsWith("C")));
        }

        [Test]
        public void Search_SearchStartsWithOrdinalIgnoreCaseStringComparison_CaseIsIgnored()
        {
            //Arrange
            
            //Act
            var result = testData.Search(x => x.Description).SetCulture(StringComparison.OrdinalIgnoreCase).StartsWith("C");

            //Assert
            Assert.AreEqual(3, result.Count());
            Assert.IsTrue(result.All(x => x.Description.StartsWith("c", StringComparison.OrdinalIgnoreCase)));
        }

        [Test]
        public void Search_SearchEndsWithOrdinalStringComparison_OnlyMatchingCaseIsReturned()
        {
            //Arrange
            
            //Act
            var result = testData.Search(x => x.Description).SetCulture(StringComparison.Ordinal).EndsWith("SE");

            //Assert
            Assert.AreEqual(1, result.Count());
            Assert.IsTrue(result.All(x => x.Description.EndsWith("SE", StringComparison.Ordinal)));
        }

        [Test]
        public void Search_SearchEndsWithOrdinalIgnoreCaseStringComparison_CaseIsIgnored()
        {
            //Arrange
            
            //Act
            var result = testData.Search(x => x.Description).SetCulture(StringComparison.OrdinalIgnoreCase).EndsWith("SE");

            //Assert
            Assert.AreEqual(2, result.Count());
            Assert.IsTrue(result.All(x => x.Description.EndsWith("se", StringComparison.OrdinalIgnoreCase)));
        }

        [Test]
        public void Search_SearchIsEqualOrdinalStringComparison_OnlyMatchingCaseIsReturned()
        {
            //Arrange
            
            //Act
            var result = testData.Search(x => x.Description).SetCulture(StringComparison.Ordinal).IsEqual("CASE");

            //Assert
            Assert.AreEqual(1, result.Count());
            Assert.IsTrue(result.All(x => x.Description.EndsWith("CASE", StringComparison.Ordinal)));
        }

        [Test]
        public void Search_SearchIsEqualOrdinalIgnoreCaseStringComparison_CaseIsIgnored()
        {
            //Arrange
            
            //Act
            var result = testData.Search(x => x.Description).SetCulture(StringComparison.OrdinalIgnoreCase).IsEqual("CASE");

            //Assert
            Assert.AreEqual(2, result.Count());
            Assert.IsTrue(result.All(x => x.Description.Equals("case", StringComparison.OrdinalIgnoreCase)));
        }

        [Test]
        public void Search_SearchManyTermsAreEqual_ResultsMatchAnyTerm()
        {
            //Arrange
            
            //Act
            var result = testData.Search(x => x.Name).IsEqual("abcd", "efgh");

            //Assert
            Assert.AreEqual(2, result.Count());
            Assert.IsTrue(result.All(x => x.Name == "abcd" || x.Name == "efgh"));
        }

        [Test]
        public void Search_SearchManyTermsAreEqualIgnoringCase_ResultsMatchAnyTermInAnyCase()
        {
            //Arrange
            
            //Act
            var result = testData.Search(x => x.Name)
                                 .SetCulture(StringComparison.OrdinalIgnoreCase)
                                 .IsEqual("ABCD", "EFGH");

            //Assert
            Assert.AreEqual(2, result.Count());
            Assert.IsTrue(result.All(x => x.Name == "abcd" || x.Name == "efgh"));
        }
    }
}
