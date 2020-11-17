using Processing.Syntax.Model;
using System.Collections.Generic;

namespace Processing.Syntax
{
    public interface ISyntaxTokenParser
    {
        IList<SyntaxToken> ParseSyntaxTokens(string expression);
    }
}
