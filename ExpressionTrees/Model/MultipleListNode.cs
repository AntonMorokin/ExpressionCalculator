using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace ExpressionTrees.Model
{
    public sealed class MultipleListNode<T> : IListNode<T>
    {
        private readonly List<IListNode<T>> _values;

        public MultipleListNode()
        {
            _values = new List<IListNode<T>>();
        }

        public MultipleListNode(IEnumerable<IListNode<T>> values)
            : this()
        {
            if (values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            _values.AddRange(values);
        }

        public IEnumerable<IListNode<T>> Nodes => _values;

        /// <summary>
        /// Flat sequence of <typeparamref name="T"/>.
        /// </summary>
        /// <returns>Flat sequence of <typeparamref name="T"/>.</returns>
        public IEnumerable<T> GetValues() => _values.SelectMany(v => v.GetValues()).ToImmutableList();
    }
}
