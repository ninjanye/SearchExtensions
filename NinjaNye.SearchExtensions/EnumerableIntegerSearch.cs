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
            Expression equalsExpression = EnumerableEqualsExpressionBuilder.Build(Properties, values);
            this.BuildExpression(equalsExpression);
            return this;
        } 
    }
}