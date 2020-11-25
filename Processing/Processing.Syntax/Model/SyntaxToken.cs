namespace Processing.Syntax.Model
{
    public class SyntaxToken
    {
        public SyntaxTokenTypes Type { get; }

        public string Value { get; }

        public SyntaxToken(SyntaxTokenTypes type, string value)
        {
            Type = type;
            Value = value;
        }
    }
}
