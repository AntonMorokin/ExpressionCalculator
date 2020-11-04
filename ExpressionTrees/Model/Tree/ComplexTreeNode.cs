using System.Collections.Generic;

namespace ExpressionTrees.Model.Tree
{
    public sealed class ComplexTreeNode : ITreeNode
    {
        public IList<ITreeNode> Values { get; }

        public ComplexTreeNode(IList<ITreeNode> values)
        {
            Values = values;
        }
    }
}
