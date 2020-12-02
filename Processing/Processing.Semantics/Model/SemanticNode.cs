namespace Processing.Semantics.Model
{
    public abstract class SemanticNode
    {
        public SemanticNodeTypes Type { get; }

        public string Value { get; }

        protected SemanticNode(SemanticNodeTypes type, string value)
        {
            Type = type;
            Value = value;
        }
    }
}
