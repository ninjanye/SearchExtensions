using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace NinjaNye.SearchExtensions
{
    internal static class ExpressionHelper
    {
        static readonly ConstantExpression EmptyStringExpression = Expression.Constant(string.Empty);
        static readonly ConstantExpression ZeroConstantExpression = Expression.Constant(0);
        static readonly ConstantExpression NullExpression = Expression.Constant(null);
        static readonly MethodInfo IndexOfMethod = typeof(string).GetMethod("IndexOf", new[] { typeof(string), typeof(StringComparison) });
        static readonly PropertyInfo StringLengthProperty = typeof(string).GetProperty("Length");
        static readonly MethodInfo ReplaceMethod = typeof(string).GetMethod("Replace", new[] { typeof(string), typeof(string) });
        static readonly MethodInfo CustomReplaceMethod = typeof(StringExtensions).GetMethod("Replace");

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
        /// Join two expressions using the conditional OR operation
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
        /// Build a 'contains' expression for a search term against a particular string property
        /// </summary>
        public static MethodCallExpression BuildContainsExpression<T>(Expression<Func<T, string>> stringProperty, ConstantExpression searchTermExpression)
        {
            var coalesceExpression = Expression.Coalesce(stringProperty.Body, EmptyStringExpression);
            return Expression.Call(coalesceExpression, typeof(string).GetMethod("Contains"), searchTermExpression);
        }


        /// <summary>
        /// Build a 'not null' expression for a particular string property
        /// </summary>
        public static Expression BuildNotNullExpression<T>(Expression<Func<T, string>> stringProperty)
        {
            return Expression.NotEqual(stringProperty.Body, NullExpression);
        }

        /// <summary>
        /// Build a 'indexof() >= 0' expression for a search term against a particular string property
        /// </summary>
        public static BinaryExpression BuildIndexOfExpression(Expression stringProperty, ConstantExpression searchTermExpression, ConstantExpression stringComparisonExpression)
        {
            var coalesceExpression = Expression.Coalesce(stringProperty, EmptyStringExpression);
            var indexOfCallExpresion = Expression.Call(coalesceExpression, IndexOfMethod, searchTermExpression, stringComparisonExpression);
            return Expression.GreaterThanOrEqual(indexOfCallExpresion, ZeroConstantExpression);
        }

        /// <summary>
        /// Constructs a ranked result of type T
        /// </summary>
        /// <param name="hitCountExpression">Expression representing how to calculated search hits</param>
        /// <param name="parameterExpression">property parameter</param>
        /// <returns>Expression equivalent to: new Ranked{ Hits = [hitCountExpression], Item = x }</returns>
        public static Expression ConstructRankedResult<T>(Expression hitCountExpression,
                                                           ParameterExpression parameterExpression)
        {
            var rankedType = typeof(Ranked<T>);
            var rankedCtor = Expression.New(rankedType);
            PropertyInfo hitProperty = rankedType.GetProperty("Hits");
            PropertyInfo itemProperty = rankedType.GetProperty("Item");
            var hitValueAssignment = Expression.Bind(hitProperty, hitCountExpression);
            var itemValueAssignment = Expression.Bind(itemProperty, parameterExpression);
            return Expression.MemberInit(rankedCtor, hitValueAssignment, itemValueAssignment);
        }

        /// <summary>
        /// Calculates how many search hits occured for a given property
        /// </summary>
        /// <param name="stringProperty">string property to analyse</param>
        /// <param name="searchTerm">search term to count</param>
        /// <returns>Expression equivalent to: [property].Length - ([property].Replace([searchTerm], "").Length) / [searchTerm].Length</returns>
        public static Expression CalculateHitCount<T>(Expression<Func<T, string>> stringProperty, string searchTerm)
        {
            Expression searchTermExpression = Expression.Constant(searchTerm);
            Expression searchTermLengthExpression = Expression.Constant(searchTerm.Length);
            MemberExpression lengthExpression = Expression.Property(stringProperty.Body, StringLengthProperty);
            var replaceExpression = Expression.Call(stringProperty.Body, ReplaceMethod,
                                                    searchTermExpression, EmptyStringExpression);
            var replacedLengthExpression = Expression.Property(replaceExpression, StringLengthProperty);
            var characterDiffExpression = Expression.Subtract(lengthExpression, replacedLengthExpression);
            var hitCountExpression = Expression.Divide(characterDiffExpression, searchTermLengthExpression);
            return hitCountExpression;
        }

        /// <summary>
        /// Calculates how many search hits occured for a given property
        /// </summary>
        /// <returns>Expression equivalent to: [property].Length - ([property].Replace([searchTerm], "").Length) / [searchTerm].Length</returns>
        public static Expression CalculateHitCount<T>(Expression<Func<T, string>> stringProperty, string searchTerm, StringComparison stringComparison)
        {
            Expression searchTermExpression = Expression.Constant(searchTerm);
            Expression searchTermLengthExpression = Expression.Constant(searchTerm.Length);
            Expression stringComparisonExpression = Expression.Constant(stringComparison);
            MemberExpression lengthExpression = Expression.Property(stringProperty.Body, StringLengthProperty);
            var replaceExpression = Expression.Call(CustomReplaceMethod, stringProperty.Body, searchTermExpression, EmptyStringExpression, stringComparisonExpression);
            var replacedLengthExpression = Expression.Property(replaceExpression, StringLengthProperty);
            var characterDiffExpression = Expression.Subtract(lengthExpression, replacedLengthExpression);
            var hitCountExpression = Expression.Divide(characterDiffExpression, searchTermLengthExpression);
            return hitCountExpression;
        }

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
        /// Builds an array of expressions that map to every string property on an object
        /// </summary>
        /// <typeparam name="T">Type of object to retrieve properties from</typeparam>
        /// <returns>An array of expressions pointing to each string property</returns>
        public static Expression<Func<T, string>>[] GetStringProperties<T>()
        {
            var parameter = Expression.Parameter(typeof(T));
            var stringProperties = typeof(T).GetProperties().Where(property => property.CanRead
                                                                            && property.PropertyType == typeof(String));

            var result = new List<Expression<Func<T, string>>>();
            foreach (var property in stringProperties)
            {
                PropertyInfo localProperty = property;
                Expression body = Expression.Property(parameter, localProperty);
                result.Add(Expression.Lambda<Func<T, string>>(body, parameter));
            }
            return result.ToArray();
        }

    }
}