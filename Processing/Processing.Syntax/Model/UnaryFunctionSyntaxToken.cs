namespace Processing.Syntax.Model
{
    public sealed class UnaryFunctionSyntaxToken : SyntaxToken
    {
        public BracesSyntaxToken Braces { get; set; }

        public UnaryFunctionSyntaxToken(string value, BracesSyntaxToken bracesToken)
            : base(SyntaxTokenTypes.UnaryFunction, value)
        {
            Braces = bracesToken;
        }
    }
}
