using System;
using System.Linq.Expressions;
using NinjaNye.SearchExtensions.Parsing;
using Xunit;

namespace NinjaNye.SearchExtensions.Tests.SearchExtensionTests.Parsing
{
    public class ExpressionParserTests
    {
        [Fact]
        public void Parse_ReturnsCorrectResult()
        {
            const string expressionString = "Child.Message";

            var expression = ExpressionParser.Parse<TestObject, string>(expressionString);

            var parameterExpression = Assert.Single(expression.Parameters);
            Assert.NotNull(parameterExpression);
            Assert.Equal(typeof(TestObject), parameterExpression.Type);

            var memberExpression = expression.Body as MemberExpression;
            Assert.NotNull(memberExpression);
            Assert.Equal("Message", memberExpression.Member.Name);
            
            memberExpression = memberExpression.Expression as MemberExpression;
            Assert.NotNull(memberExpression);
            Assert.Equal("Child", memberExpression.Member.Name);

            Assert.Equal(parameterExpression, memberExpression.Expression);
        }

        [Fact]
        public void ParseProperties_ReturnsCorrectResult()
        {
            const string expression = "Property1.Property2";

            string[] properties = ExpressionParser.ParseProperties(expression);

            Assert.Equal(2, properties.Length);
            Assert.Equal("Property1", properties[0]);
            Assert.Equal("Property2", properties[1]);
        }

        [Fact]
        public void ParseProperties_EmptyExpression_ThrowsException()
        {
            const string expression = null;

            Action act = () => ExpressionParser.ParseProperties(expression);

            Assert.Throws<ArgumentException>(act);
        }

        [Fact]
        public void ParseProperties_InvalidExpression_ThrowsException()
        {
            const string expression = "Test\\Child";

            Action act = () => ExpressionParser.ParseProperties(expression);

            Assert.Throws<ArgumentException>(act);
        }
        
        [Fact]
        public void CreateLambdaExpression_NullPropertyCollection_ThrowsException()
        {
            const string[] properties = null;

            Action act = () => ExpressionParser.CreateLambdaExpression<TestObject, string>(properties);

            Assert.Throws<ArgumentNullException>(act);
        }
        
        [Fact]
        public void CreateLambdaExpression_EmptyPropertyCollection_ThrowsException()
        {
            string[] properties = new string[0];

            Action act = () => ExpressionParser.CreateLambdaExpression<TestObject, string>(properties);

            Assert.Throws<ArgumentException>(act);
        }
        
        [Fact]
        public void CreateLambdaExpression_WrongReturnType_ThrowsException()
        {
            string[] properties = new[] { "Child" };

            Action act = () => ExpressionParser.CreateLambdaExpression<TestObject, string>(properties);

            Assert.Throws<TypeMismatchException>(act);
        }
        
        [Fact]
        public void CreateLambdaExpression_NonExistingProperty_ThrowsException()
        {
            string[] properties = new[] { "NonExistingProperty" };

            Action act = () => ExpressionParser.CreateLambdaExpression<TestObject, string>(properties);

            Assert.Throws<ArgumentException>(act);
        }
    }
}