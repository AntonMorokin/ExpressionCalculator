using Calculation.Model;
using System.Collections.Generic;

namespace Processing.Syntax.Model
{
    public class SyntaxTokenOld
    {
        public IHasValue MainValue { get; set; }

        public IList<SyntaxTokenOld> SubTokens { get; set; }

        public bool HasSubTokens => (SubTokens?.Count ?? 0) > 0;
    }
}
