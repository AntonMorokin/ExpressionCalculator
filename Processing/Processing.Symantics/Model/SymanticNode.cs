namespace Processing.Symantics.Model
{
    public abstract class SymanticNode
    {
        public SymanticNodeTypes Type { get; }

        public string Value { get; }

        protected SymanticNode(SymanticNodeTypes type, string value)
        {
            Type = type;
            Value = value;
        }
    }
}
