namespace Processing.Symantics.Model
{
    public sealed class BinaryFunctionSymanticNode : SymanticNode
    {
        public SymanticNode LeftChild { get; set; }

        public SymanticNode RightChild { get; set; }

        public BinaryFunctionSymanticNode(string value)
            : base(SymanticNodeTypes.BinaryFunction, value)
        {
        }
    }
}
