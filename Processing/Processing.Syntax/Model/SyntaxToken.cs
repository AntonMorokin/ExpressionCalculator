using Calculation.Model;
using System.Collections.Generic;

namespace Processing.Syntax.Model
{
    public class SyntaxToken
    {
        public IHasValue MainValue { get; set; }

        public IList<SyntaxToken> SubTokens { get; set; }

        public bool HasSubTokens => (SubTokens?.Count ?? 0) > 0;
    }
}
