using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace NinjaNye.SearchExtensions
{
    public abstract class EnumerableSearchBase<TSource, TProperty> : SearchBase<IEnumerable<TSource>, TSource, TProperty>, IEnumerable<TSource>
    {
        protected EnumerableSearchBase(IEnumerable<TSource> source, Expression<Func<TSource, TProperty>>[] properties)
            : base(source, properties)
        {
        }

        public virtual IEnumerator<TSource> GetEnumerator()
        {
            if (this.CompleteExpression != null)
            {
                var finalExpression = Expression.Lambda<Func<TSource, bool>>(this.CompleteExpression, this.FirstParameter).Compile();
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