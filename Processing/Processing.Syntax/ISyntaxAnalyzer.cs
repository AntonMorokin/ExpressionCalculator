using Processing.Syntax.Model;
using System.Collections.Generic;

namespace Processing.Syntax
{
    public interface ISyntaxAnalyzer
    {
        void AnalyzeCorrectness(IList<SyntaxToken> syntaxTokens);
    }
}