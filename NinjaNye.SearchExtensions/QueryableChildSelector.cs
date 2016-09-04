using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace NinjaNye.SearchExtensions
{
    public class QueryableChildSelector<TBase, TChild> : IQueryable<TBase>
    {
        private readonly IQueryable<TBase> _parent;
        private readonly Expression<Func<TBase, IEnumerable<TChild>>>[] _childProperties;

        public QueryableChildSelector(IQueryable<TBase> parent, Expression<Func<TBase, IEnumerable<TChild>>>[] childProperties)
        {
            _parent = parent;
            _childProperties = childProperties;
            Provider = _parent.Provider;
            ElementType = _parent.ElementType;
            Expression = _parent.Expression;
        }

        public QueryableChildSearch<TBase, TChild, TProperty> With<TProperty>(params Expression<Func<TChild, TProperty>>[] properties)
        {
            return new QueryableChildSearch<TBase, TChild, TProperty>(_parent, _childProperties, properties);
        }

        public QueryableChildStringSearch<TBase, TChild> With(params Expression<Func<TChild, string>>[] properties)
        {
            return new QueryableChildStringSearch<TBase, TChild>(_parent, _childProperties, properties);
        }

        public IEnumerator<TBase> GetEnumerator()
        {
            return _parent.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public Expression Expression { get; private set; }
        public Type ElementType { get; private set; }
        public IQueryProvider Provider { get; private set; }
    }
}