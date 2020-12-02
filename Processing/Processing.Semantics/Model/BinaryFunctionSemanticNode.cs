namespace Processing.Semantics.Model
{
    public sealed class BinaryFunctionSemanticNode : SemanticNode
    {
        public SemanticNode LeftChild { get; set; }

        public SemanticNode RightChild { get; set; }

        public int Priority { get; set; }

        public BinaryFunctionSemanticNode(string value, int priority)
            : base(SemanticNodeTypes.BinaryFunction, value)
        {
            Priority = priority;
        }
    }
}
