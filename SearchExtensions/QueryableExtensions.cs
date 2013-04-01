using System;
using System.Linq;
using System.Linq.Expressions;

namespace SearchExtensions
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> Search<T>(this IQueryable<T> source, Expression<Func<T, string>> stringProperty, string searchTerm)
        {
            if (String.IsNullOrEmpty(searchTerm))
            {
                return source;
            }

            //Create expression to represent x.[property] != null
            var isNotNullExpression = Expression.NotEqual(stringProperty.Body, Expression.Constant(null));

            //Create expression to represent x.[property].Contains(searchTerm)
            var searchTermExpression = Expression.Constant(searchTerm);
            var containsExpression = BuildContainsExpression(stringProperty, searchTermExpression);

            //Join not null and contains expressions
            var notNullAndContainsExpression = Expression.AndAlso(isNotNullExpression, containsExpression);

            var completeExpression = Expression.Lambda<Func<T, bool>>(notNullAndContainsExpression, stringProperty.Parameters);
            return source.Where(completeExpression);
        }

        /// <summary>
        /// Search multiple properties for a particular search term
        /// </summary>
        /// <param name="source">Source data to query</param>
        /// <param name="searchTerm">search term to look for</param>
        /// <param name="stringProperties">properties to search against</param>
        public static IQueryable<T> Search<T>(this IQueryable<T> source, string searchTerm, params Expression<Func<T, string>>[] stringProperties)
        {
            if (String.IsNullOrEmpty(searchTerm))
            {
                return source;
            }

            // The below represents the following lamda:
            // source.Where(x => x.[property1].Contains(searchTerm)
            //                || x.[property2].Contains(searchTerm)
            //                || x.[property3].Contains(searchTerm)...)

            var searchTermExpression = Expression.Constant(searchTerm);

            //Variable to hold merged 'OR' expression
            Expression orExpression = null;
            //Retrieve first parameter to use accross all expressions
            var singleParameter = stringProperties[0].Parameters.Single();

            //Build a contains expression for each property
            foreach (var stringProperty in stringProperties)
            {
                //Syncronise single parameter accross each property
                var swappedParamExpression = SwapExpressionVisitor.Swap(stringProperty, stringProperty.Parameters.Single(), singleParameter);

                //Build expression to represent x.[propertyX].Contains(searchTerm)
                var containsExpression = BuildContainsExpression(swappedParamExpression, searchTermExpression);

                orExpression = BuildOrExpression(orExpression, containsExpression);
            }

            var completeExpression = Expression.Lambda<Func<T, bool>>(orExpression, singleParameter);
            return source.Where(completeExpression);
        }

        private static Expression BuildOrExpression(Expression existingExpression, Expression expressionToAdd)
        {
            if (existingExpression == null)
            {
                return expressionToAdd;
            }

            //Build 'OR' expression for each property
            return Expression.OrElse(existingExpression, expressionToAdd);
        }

        private static MethodCallExpression BuildContainsExpression<T>(Expression<Func<T, string>> stringProperty, ConstantExpression searchTermExpression)
        {
            return Expression.Call(stringProperty.Body, typeof(string).GetMethod("Contains"), searchTermExpression);
        }
    }
}
