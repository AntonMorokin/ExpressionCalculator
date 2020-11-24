namespace Processing.Symantics.Model
{
    public sealed class NumberSymanticNode : SymanticNode
    {
        public NumberSymanticNode(string value)
            : base(SymanticNodeTypes.Number, value)
        {
        }
    }
}
