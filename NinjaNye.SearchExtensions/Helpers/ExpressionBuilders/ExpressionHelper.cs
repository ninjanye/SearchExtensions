using System;
using System.Linq.Expressions;

namespace NinjaNye.SearchExtensions.Helpers.ExpressionBuilders
{
    internal static class ExpressionHelper
    {
        /// <summary>
        /// Join two expressions using the add operation
        /// </summary>
        /// <param name="existingExpression">Expression being summed</param>
        /// <param name="expressionToAdd">Expression to append</param>
        /// <returns>New expression containing both expressions joined using the Add operation</returns>
        public static Expression AddExpressions(Expression existingExpression, Expression expressionToAdd)
        {
            if (existingExpression == null)
            {
                return expressionToAdd;
            }

            return Expression.Add(existingExpression, expressionToAdd);
        }

        /// <summary>
        /// Join two expressions using the conditional OR operation
        /// </summary>
        /// <param name="existingExpression">Expression being joined</param>
        /// <param name="expressionToJoin">Expression to append</param>
        /// <returns>New expression containing both expressions joined using the conditional OR operation</returns>
        public static Expression JoinOrExpression(Expression existingExpression, Expression expressionToJoin)
        {
            if (existingExpression == null)
            {
                return expressionToJoin;
            }

            return Expression.OrElse(existingExpression, expressionToJoin);
        }

        /// <summary>
        /// Join two expressions using the conditional AND operation
        /// </summary>
        /// <param name="existingExpression">Expression being joined</param>
        /// <param name="expressionToJoin">Expression to append</param>
        /// <returns>New expression containing both expressions joined using the conditional OR operation</returns>
        public static Expression JoinAndAlsoExpression(Expression existingExpression, Expression expressionToJoin)
        {
            if (existingExpression == null)
            {
                return expressionToJoin;
            }

            return Expression.AndAlso(existingExpression, expressionToJoin);
        }

        /// <summary>
        /// Build a 'not null' expression for a particular string property
        /// </summary>
        public static Expression BuildNotNullExpression<T, TType>(Expression<Func<T, TType>> property)
        {
            return Expression.NotEqual(property.Body, ExpressionMethods.NullExpression);
        }
    }
}