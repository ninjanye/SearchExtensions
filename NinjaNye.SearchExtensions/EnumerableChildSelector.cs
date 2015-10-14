using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace NinjaNye.SearchExtensions
{
    public class EnumerableChildSelector<TBase, TChild> : IEnumerable<TBase>
    {
        private readonly IEnumerable<TBase> _parent;
        private readonly Expression<Func<TBase, IEnumerable<TChild>>> _child;

        public EnumerableChildSelector(IEnumerable<TBase> parent, Expression<Func<TBase, IEnumerable<TChild>>> child) 
        {
            this._parent = parent;
            this._child = child;
        }

        public EnumerableChildSearch<TBase, TChild, TProperty> With<TProperty>(Expression<Func<TChild, TProperty>> property)
        {
            return new EnumerableChildSearch<TBase, TChild, TProperty>(this._parent, this._child, property);
        }

        public IEnumerator<TBase> GetEnumerator()
        {
            return this._parent.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}