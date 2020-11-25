namespace Processing.Syntax.Model
{
    public sealed class UnaryFunctionSyntaxToken : SyntaxToken
    {
        public BracesSyntaxToken Braces { get; set; }

        public UnaryFunctionSyntaxToken(string value)
            : base(SyntaxTokenTypes.UnaryFunction, value)
        {
        }
    }
}
