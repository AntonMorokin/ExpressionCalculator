namespace Processing.Symantics.Model
{
    public sealed class UnaryFunctionSymanticNode : SymanticNode
    {
        public BracesSymanticNode Braces { get; set; }

        public UnaryFunctionSymanticNode(string value, BracesSymanticNode bracesNode)
            : base(SymanticNodeTypes.UnaryFunction, value)
        {
            Braces = bracesNode;
        }
    }
}
