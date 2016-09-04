using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace NinjaNye.SearchExtensions
{
    public class EnumerableChildSelector<TBase, TChild> : IEnumerable<TBase>
    {
        private readonly IEnumerable<TBase> _parent;
        private readonly Expression<Func<TBase, IEnumerable<TChild>>>[] _childProperties;

        public EnumerableChildSelector(IEnumerable<TBase> parent, Expression<Func<TBase, IEnumerable<TChild>>>[] childProperties) 
        {
            _parent = parent;
            _childProperties = childProperties;
        }

        public EnumerableChildSearch<TBase, TChild, TProperty> With<TProperty>(params Expression<Func<TChild, TProperty>>[] properties)
        {
            return new EnumerableChildSearch<TBase, TChild, TProperty>(_parent, _childProperties, properties);
        }

        public EnumerableChildStringSearch<TBase, TChild> With(params Expression<Func<TChild, string>>[] properties)
        {
            return new EnumerableChildStringSearch<TBase, TChild>(_parent, _childProperties, properties);
        }

        public IEnumerator<TBase> GetEnumerator()
        {
            return _parent.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}