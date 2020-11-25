using Processing.Symantics.Model;
using System.Collections.Generic;

namespace Processing.Symantics
{
    public interface ISymanticAnalyzer
    {
        SymanticNode BuildSymanticTree(IList<SymanticNode> symanticNodes);
    }
}
