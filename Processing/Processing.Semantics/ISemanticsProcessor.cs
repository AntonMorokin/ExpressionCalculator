using Processing.Semantics.Model;
using Processing.Syntax.Model;
using System.Collections.Generic;

namespace Processing.Semantics
{
    public interface ISemanticsProcessor
    {
        SemanticNode CreateSemanticModel(IList<SyntaxToken> syntaxTokens);
    }
}