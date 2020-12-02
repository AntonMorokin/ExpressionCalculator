namespace Processing.Semantics.Model
{
    public sealed class NumberSemanticNode : SemanticNode
    {
        public NumberSemanticNode(string value)
            : base(SemanticNodeTypes.Number, value)
        {
        }
    }
}
