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
            if (CompleteExpression != null)
            {
                var finalExpression = Expression.Lambda<Func<TSource, bool>>(CompleteExpression, FirstParameter).Compile();
                Source = Source.Where(finalExpression);
            }
            return Source.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

}