using System.Collections.Generic;

namespace ExpressionTrees.Model.List
{
    public sealed class SingleListNode<T> : IListNode<T>
    {
        public T Value { get; set; }

        public SingleListNode()
        {
        }

        public SingleListNode(T value)
            : this()
        {
            Value = value;
        }

        public IEnumerable<T> GetValues()
        {
            return new[] { Value };
        }
    }
}
