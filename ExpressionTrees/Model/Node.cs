namespace ExpressionTrees.Model
{
    /// <summary>
    /// Node of expression (binary) tree.
    /// </summary>
    /// <typeparam name="T">Type of node value.</typeparam>
    public class Node<T>
    {
        /// <summary>
        /// Parent of node.
        /// </summary>
        public Node<T> Parent { get; set; }

        /// <summary>
        /// Left child of node.
        /// </summary>
        public Node<T> LeftChild { get; set; }

        /// <summary>
        /// Right child of node.
        /// </summary>
        public Node<T> RightChild { get; set; }

        /// <summary>
        /// Node value.
        /// </summary>
        public T Value { get; set; }

        /// <summary>
        /// Initializes instance of <see cref="Node{T}"/>.
        /// </summary>
        /// <param name="parent">Parent of node.</param>
        public Node(Node<T> parent)
        {
            Parent = parent;
        }
    }
}
