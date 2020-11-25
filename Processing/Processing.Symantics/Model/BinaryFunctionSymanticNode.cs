namespace Processing.Symantics.Model
{
    public sealed class BinaryFunctionSymanticNode : SymanticNode
    {
        public SymanticNode LeftChild { get; set; }

        public SymanticNode RightChild { get; set; }

        public int Priority { get; set; }

        public BinaryFunctionSymanticNode(string value, int priority)
            : base(SymanticNodeTypes.BinaryFunction, value)
        {
            Priority = priority;
        }
    }
}
