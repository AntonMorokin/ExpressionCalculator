namespace Processing.Semantics.Model
{
    public sealed class UnaryFunctionSemanticNode : SemanticNode
    {
        public SemanticNode Child { get; set; }

        public UnaryFunctionSemanticNode(string value, BracesSemanticNode bracesNode)
            : base(SemanticNodeTypes.UnaryFunction, value)
        {
            Child = bracesNode;
        }
    }
}
