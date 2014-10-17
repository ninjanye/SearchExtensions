using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace NinjaNye.SearchExtensions
{
    public abstract class EnumerableSearchBase<T> : IEnumerable<T>
    {
        protected IEnumerable<T> Source;
        private Expression completeExpression;
        protected readonly Expression<Func<T, string>>[] StringProperties;
        protected readonly ParameterExpression FirstParameter;

        protected EnumerableSearchBase(IEnumerable<T> source, Expression<Func<T, string>>[] stringProperties)
        {
            this.Source = source;
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
            if (this.completeExpression == null)
            {
                this.completeExpression = expressionToJoin;
            }
            else
            {
                this.completeExpression = Expression.AndAlso(this.completeExpression, expressionToJoin);
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            if (this.completeExpression != null)
            {
                var finalExpression = Expression.Lambda<Func<T, bool>>(this.completeExpression, this.FirstParameter).Compile();
                this.Source = this.Source.Where(finalExpression);
            }
            return this.Source.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}