using Calculation.Model;

namespace ExpressionTrees.Model.Tree
{
    /// <summary>
    /// Node of expression (binary) tree.
    /// </summary>
    public class TreeNode : ITreeNode
    {
        /// <summary>
        /// Parent of node.
        /// </summary>
        public ITreeNode Parent { get; set; }

        /// <summary>
        /// Left child of node.
        /// </summary>
        public ITreeNode LeftChild { get; set; }

        /// <summary>
        /// Right child of node.
        /// </summary>
        public ITreeNode RightChild { get; set; }

        /// <summary>
        /// Node value.
        /// </summary>
        public IHasValue Value { get; set; }

        /// <summary>
        /// Initializes instance of <see cref="TreeNode{T}"/>.
        /// </summary>
        /// <param name="parent">Parent of node.</param>
        public TreeNode(ITreeNode parent)
        {
            Parent = parent;
        }
    }
}
