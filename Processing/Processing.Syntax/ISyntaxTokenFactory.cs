using Processing.Syntax.Model;
using System.Collections.Generic;

namespace Processing.Syntax
{
    public interface ISyntaxTokenFactory
    {
        IEnumerable<string> KnownBinaryFunctions { get; }

        SyntaxToken ParseToken(string token);
    }
}
