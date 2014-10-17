using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace NinjaNye.SearchExtensions
{
    public abstract class EnumerableSearchBase<T> : IEnumerable<T>
    {
        private IEnumerable<T> source;
        protected Expression CompleteExpression;
        protected readonly Expression<Func<T, string>>[] StringProperties;
        protected readonly ParameterExpression FirstParameter;

        protected EnumerableSearchBase(IEnumerable<T> source, Expression<Func<T, string>>[] stringProperties)
        {
            this.source = source;
            this.StringProperties = stringProperties;
            var firstProperty = stringProperties.FirstOrDefault();
            if (firstProperty != null)
            {
                this.FirstParameter = firstProperty.Parameters[0];
            }
        }

        /// <summary>
        /// Appends expressionToJoin to CompleteExpression using an Expression.AndAlso join
        /// </summary>
        protected virtual void BuildExpression(Expression expressionToJoin)
        {
            if (this.CompleteExpression == null)
            {
                this.CompleteExpression = expressionToJoin;
            }
            else
            {
                this.CompleteExpression = Expression.AndAlso(this.CompleteExpression, expressionToJoin);
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            if (this.CompleteExpression != null)
            {
                var finalExpression = Expression.Lambda<Func<T, bool>>(this.CompleteExpression, this.FirstParameter).Compile();
                this.source = this.source.Where(finalExpression);
            }
            return this.source.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}