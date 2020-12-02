using Processing.Semantics.Model;
using System.Collections.Generic;

namespace Processing.Semantics
{
    public interface ISemanticAnalyzer
    {
        SemanticNode BuildSemanticTree(IList<SemanticNode> semanticNodes);
    }
}
