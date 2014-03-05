using System;
using System.Linq.Expressions;
using System.Reflection;

namespace NinjaNye.SearchExtensions
{
    internal static class DbExpressionHelper
    {
        static readonly MethodInfo IndexOfMethod = typeof(string).GetMethod("IndexOf", new[] { typeof(string) });
        static readonly ConstantExpression EmptyStringExpression = Expression.Constant(string.Empty);
        static readonly ConstantExpression ZeroConstantExpression = Expression.Constant(0);
        static readonly PropertyInfo StringLengthProperty = typeof(string).GetProperty("Length");
        static readonly MethodInfo ContainsMethod = typeof(string).GetMethod("Contains");

        /// <summary>
        /// Build a 'contains' expression for a search term against a particular string property
        /// </summary>
        public static Expression BuildContainsExpression<T>(Expression<Func<T, string>> stringProperty, ConstantExpression searchTermExpression)
        {
            return Expression.Call(stringProperty.Body, ContainsMethod, searchTermExpression);
        }

        /// <summary>
        /// Build a 'indexof() == 0' expression for a search term against a particular string property
        /// </summary>
        public static BinaryExpression BuildStartsWithExpression<T>(Expression<Func<T, string>> stringProperty, string[] searchTerms, bool nullCheck = true)
        {
            BinaryExpression completeExpression = null;
            foreach (var searchTerm in searchTerms)
            {
                var startsWithExpression = BuildStartsWithExpression(stringProperty, searchTerm, nullCheck);
                completeExpression = completeExpression == null ? startsWithExpression
                                                                : Expression.OrElse(completeExpression, startsWithExpression);
            }
            return completeExpression;
        }

        /// <summary>
        /// Build an 'indexof() == 0' expression for a search term against a particular string property
        /// </summary>
        public static BinaryExpression BuildStartsWithExpression<T>(Expression<Func<T, string>> stringProperty, string searchTerm, bool nullCheck = true)
        {
            var searchTermExpression = Expression.Constant(searchTerm);
            if (nullCheck)
            {
                var coalesceExpression = Expression.Coalesce(stringProperty.Body, EmptyStringExpression);
                var nullCheckExpresion = Expression.Call(coalesceExpression, IndexOfMethod, searchTermExpression);
                return Expression.Equal(nullCheckExpresion, ZeroConstantExpression);
            }

            var indexOfCallExpresion = Expression.Call(stringProperty.Body, IndexOfMethod, searchTermExpression);
            return Expression.Equal(indexOfCallExpresion, ZeroConstantExpression);
        }

        /// <summary>
        /// Build an 'indexof() == 0' expression for a search term against a particular string property
        /// </summary>
        public static BinaryExpression BuildEndsWithExpression<T>(Expression<Func<T, string>> stringProperty, string searchTerm, bool nullCheck = true)
        {
            int expectedIndexOf = searchTerm.Length;
            var lengthExpression = Expression.Constant(expectedIndexOf);
            var searchTermExpression = Expression.Constant(searchTerm);
            if (nullCheck)
            {
                var coalesceExpression = Expression.Coalesce(stringProperty.Body, EmptyStringExpression);
                var nullCheckExpresion = Expression.Call(coalesceExpression, IndexOfMethod, searchTermExpression);
                return Expression.Equal(nullCheckExpresion, lengthExpression);
            }

            var indexOfCallExpresion = Expression.Call(stringProperty.Body, IndexOfMethod, searchTermExpression);
            var lengthPropertyExpression = Expression.Property(stringProperty.Body, StringLengthProperty);
            var expectedLengthExpression = Expression.Subtract(lengthPropertyExpression, lengthExpression);
            return Expression.Equal(indexOfCallExpresion, expectedLengthExpression);
        }

        /// <summary>
        /// Build a 'indexof() == 0' expression for a search term against a particular string property
        /// </summary>
        public static BinaryExpression BuildEndsWithExpression<T>(Expression<Func<T, string>> stringProperty, string[] searchTerms, bool nullCheck = true)
        {
            BinaryExpression completeExpression = null;
            foreach (var searchTerm in searchTerms)
            {
                var endsWithExpression = BuildEndsWithExpression(stringProperty, searchTerm, nullCheck);
                completeExpression = completeExpression == null ? endsWithExpression
                                         : Expression.OrElse(completeExpression, endsWithExpression);
            }

            return completeExpression;
        }

        /// <summary>
        /// Build a 'equals' expression for a search term against a particular string property
        /// </summary>
        public static Expression BuildEqualsExpression<TSource, TType>(Expression<Func<TSource, TType>> property, string[] terms)
        {
            Expression completeExpression = null;
            foreach (var term in terms)
            {
                var searchTermExpression = Expression.Constant(term);
                var equalsExpression = Expression.Equal(property.Body, searchTermExpression);
                completeExpression = ExpressionHelper.JoinOrExpression(completeExpression, equalsExpression);
            }
            return completeExpression;
        }

    }
}