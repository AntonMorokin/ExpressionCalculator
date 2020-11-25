namespace Processing.Symantics.Model
{
    public sealed class UnaryFunctionSymanticNode : SymanticNode
    {
        public SymanticNode Child { get; set; }

        public UnaryFunctionSymanticNode(string value, BracesSymanticNode bracesNode)
            : base(SymanticNodeTypes.UnaryFunction, value)
        {
            Child = bracesNode;
        }
    }
}
