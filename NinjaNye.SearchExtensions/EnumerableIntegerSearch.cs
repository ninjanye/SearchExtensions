using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using NinjaNye.SearchExtensions.Helpers.ExpressionBuilders.EqualsExpressionBuilder;

namespace NinjaNye.SearchExtensions
{
    public class EnumerableIntegerSearch<T> : EnumerableSearchBase<T, int>
    {
        public EnumerableIntegerSearch(IEnumerable<T> source, Expression<Func<T, int>>[] properties) 
            : base(source, properties)
        {
        }

        public EnumerableIntegerSearch<T> IsEqual(params int[] values)
        {
            var equalsExpression = ExpressionBuilder.EqualsExpression(Properties, values);
            this.BuildExpression(equalsExpression);
            return this;
        }

        public EnumerableIntegerSearch<T> GreaterThan(int value)
        {
            var greaterThanExpression = ExpressionBuilder.GreaterThanExpression(Properties, value);
            BuildExpression(greaterThanExpression);
            return this;
        }

        public EnumerableIntegerSearch<T> LessThan(int value)
        {
            var greaterThanExpression = ExpressionBuilder.LessThanExpression(Properties, value);
            BuildExpression(greaterThanExpression);
            return this;
        } 
    }
}