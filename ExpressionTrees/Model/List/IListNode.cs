using System.Collections.Generic;

namespace ExpressionTrees.Model.List
{
    public interface IListNode<T>
    {
        IEnumerable<T> GetValues();
    }
}
