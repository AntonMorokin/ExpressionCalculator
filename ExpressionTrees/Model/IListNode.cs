using System.Collections.Generic;

namespace ExpressionTrees.Model
{
    public interface IListNode<T>
    {
        IEnumerable<T> GetValues();
    }
}
