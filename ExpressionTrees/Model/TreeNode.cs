namespace ExpressionTrees.Model
{
    /// <summary>
    /// Node of expression (binary) tree.
    /// </summary>
    /// <typeparam name="T">Type of node value.</typeparam>
    public class TreeNode<T>
    {
        /// <summary>
        /// Parent of node.
        /// </summary>
        public TreeNode<T> Parent { get; set; }

        /// <summary>
        /// Left child of node.
        /// </summary>
        public TreeNode<T> LeftChild { get; set; }

        /// <summary>
        /// Right child of node.
        /// </summary>
        public TreeNode<T> RightChild { get; set; }

        /// <summary>
        /// Node value.
        /// </summary>
        public T Value { get; set; }

        /// <summary>
        /// Initializes instance of <see cref="TreeNode{T}"/>.
        /// </summary>
        /// <param name="parent">Parent of node.</param>
        public TreeNode(TreeNode<T> parent)
        {
            Parent = parent;
        }
    }
}
