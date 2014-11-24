using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace NinjaNye.SearchExtensions
{
    public abstract class EnumerableSearchBase<T> : EnumerableSearchBase<T, T>
    {
        protected EnumerableSearchBase(IEnumerable<T> source, Expression<Func<T, string>>[] stringProperties) 
            : base(source, stringProperties)
        {
        }

        public override IEnumerator<T> GetEnumerator()
        {
            if (this.CompleteExpression != null)
            {
                var finalExpression = Expression.Lambda<Func<T, bool>>(this.CompleteExpression, this.FirstParameter).Compile();
                this.Source = this.Source.Where(finalExpression);
            }
            return this.Source.GetEnumerator();
        }
    }

    public abstract class EnumerableSearchBase<TInput, TOutput> : IEnumerable<TOutput>
    {
        protected IEnumerable<TInput> Source;
        protected Expression CompleteExpression;
        protected readonly Expression<Func<TInput, string>>[] StringProperties;
        protected readonly ParameterExpression FirstParameter;

        protected EnumerableSearchBase(IEnumerable<TInput> source, Expression<Func<TInput, string>>[] stringProperties)
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
            if (this.CompleteExpression == null)
            {
                this.CompleteExpression = expressionToJoin;
            }
            else
            {
                this.CompleteExpression = Expression.AndAlso(this.CompleteExpression, expressionToJoin);
            }
        }

        public abstract IEnumerator<TOutput> GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}