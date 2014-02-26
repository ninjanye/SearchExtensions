using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace NinjaNye.SearchExtensions.Fluent
{
    public static class FluentSearch
    {
        public static FluentInterface<T> Search<T>(this IEnumerable<T> source, params Expression<Func<T, string>>[] stringProperties)
        {
            Ensure.ArgumentNotNull(stringProperties, "stringProperties");
            return new FluentInterface<T>(source, stringProperties);
        }

        public static FluentInterface<T> SearchStrings<T>(this IEnumerable<T> source)
        {
            var properties = ExpressionHelper.GetProperties<T, string>();
            return new FluentInterface<T>(source, properties);
        } 
    }

    public class FluentInterface<T> : IEnumerable<T>
    {
        private IEnumerable<T> source;
        private Expression completeExpression;
        private readonly Expression<Func<T, string>>[] stringProperties;
        private readonly ParameterExpression firstParameter;

        public FluentInterface(IEnumerable<T> source, Expression<Func<T, string>>[] stringProperties)
        {
            this.source = source;
            this.stringProperties = stringProperties;
            var firstProperty = stringProperties.FirstOrDefault();
            if (firstProperty != null)
            {
                this.firstParameter = firstProperty.Parameters[0];
            }
        }

        public FluentInterface<T> Containing(string term)
        {
            source = source.Search(term, stringProperties);
            return this;
        }

        public FluentInterface<T> StartsWith(string term)
        {
            Expression fullExpression = null;
            foreach (var stringProperty in stringProperties)
            {
                var swappedParamExpression = SwapExpressionVisitor.Swap(stringProperty,
                                                                        stringProperty.Parameters.Single(),
                                                                        this.firstParameter);

                var startsWithExpression = ExpressionHelper.BuildStartsWithExpression(swappedParamExpression, term, false);
                fullExpression = fullExpression == null ? startsWithExpression 
                                                        : Expression.OrElse(fullExpression, startsWithExpression);
            }
            this.BuildExpression(fullExpression);
            return this;
        }

        public FluentInterface<T> EndsWith(string term)
        {
            Expression fullExpression = null;
            foreach (var stringProperty in stringProperties)
            {
                var swappedParamExpression = SwapExpressionVisitor.Swap(stringProperty,
                                                                        stringProperty.Parameters.Single(),
                                                                        this.firstParameter);
                var endsWithExpression = ExpressionHelper.BuildEndsWithExpression(swappedParamExpression, term, false);
                fullExpression = fullExpression == null ? endsWithExpression
                                                        : Expression.OrElse(fullExpression, endsWithExpression);
            }
            this.BuildExpression(fullExpression);
            return this;
        }

        public FluentInterface<T> IsEqual(string term)
        {
            Expression fullExpression = null;
            foreach (var stringProperty in stringProperties)
            {
                var swappedParamExpression = SwapExpressionVisitor.Swap(stringProperty,
                                                                        stringProperty.Parameters.Single(),
                                                                        this.firstParameter);
                var termExpression = Expression.Constant(term);
                var isEqualExpression = ExpressionHelper.BuildEqualsExpression(swappedParamExpression, termExpression);
                fullExpression = fullExpression == null ? isEqualExpression
                                                        : Expression.OrElse(fullExpression, isEqualExpression);
            }
            this.BuildExpression(fullExpression);
            return this;
        }

        public IEnumerator<T> GetEnumerator()
        {
            if (completeExpression != null)
            {
                var finalExpression = Expression.Lambda<Func<T, bool>>(completeExpression, this.firstParameter).Compile();
                source = source.Where(finalExpression);
            }
            return this.source.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        private void BuildExpression(Expression expressionToJoin)
        {
            if (this.completeExpression == null)
            {
                this.completeExpression = expressionToJoin;
            }
            else
            {
                this.completeExpression = Expression.AndAlso(this.completeExpression, expressionToJoin);
            }
        }
    }
}
