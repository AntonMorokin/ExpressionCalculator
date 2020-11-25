using Processing.Syntax.Model;
using System.Collections.Generic;

namespace Processing.Syntax
{
    public interface ISyntaxTokenFactory
    {
        IEnumerable<char> KnownBinaryFunctions { get; }

        SyntaxToken ParseToken(string token);
    }
}
