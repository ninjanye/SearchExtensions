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

    public abstract class EnumerableSearchBase<TInput, TOutput> : SearchBase<IEnumerable<TInput>, TInput>, IEnumerable<TOutput>
    {
        protected EnumerableSearchBase(IEnumerable<TInput> source, Expression<Func<TInput, string>>[] stringProperties)
            : base(source, stringProperties)
        {
        }

        public abstract IEnumerator<TOutput> GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}