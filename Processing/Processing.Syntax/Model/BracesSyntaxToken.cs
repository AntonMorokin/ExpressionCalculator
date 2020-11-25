using System.Collections.Generic;
using System.Linq;

namespace Processing.Syntax.Model
{
    public sealed class BracesSyntaxToken : SyntaxToken
    {
        public BracesSyntaxToken(IEnumerable<SyntaxToken> childTokens)
            : base(SyntaxTokenTypes.Braces, null)
        {
            ChildTokens = childTokens?.ToList() ?? new List<SyntaxToken>(4);
        }

        public IList<SyntaxToken> ChildTokens { get; set; }
    }
}
