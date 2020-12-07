using Processing.Syntax.Model;
using System.Collections.Generic;

namespace Processing.Syntax
{
    public interface ISyntaxProcessor
    {
        IList<SyntaxToken> CreateSyntaxModel(string expression);
    }
}