using System.Collections.Generic;

namespace ExpressionTrees.Model.Tree
{
    public sealed class ComplexTreeNode : ITreeNode
    {
        public IList<TreeNode> Values { get; }

        public ComplexTreeNode(IList<TreeNode> values)
        {
            Values = values;
        }
    }
}
