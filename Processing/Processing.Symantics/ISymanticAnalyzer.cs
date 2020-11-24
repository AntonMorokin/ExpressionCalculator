using Processing.Symantics.Model;
using System.Collections.Generic;

namespace Processing.Symantics
{
    public interface ISymanticAnalyzer
    {
        SymanticNodeOld BuildSymanticTree(IList<SymanticNodeOld> symanticNodes);
    }
}
