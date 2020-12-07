using Processing.Semantics.Model;
using Processing.Syntax.Model;
using System.Collections.Generic;

namespace Processing.Semantics
{
    public interface ISemanticsConverter
    {
        IList<SemanticNode> ConvertSyntaxToSemantics(IList<SyntaxToken> syntaxTokens);
    }
}
