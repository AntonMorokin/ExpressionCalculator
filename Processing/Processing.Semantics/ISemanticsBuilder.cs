using Processing.Semantics.Model;
using System.Collections.Generic;

namespace Processing.Semantics
{
    public interface ISemanticsBuilder
    {
        SemanticNode BuildSemanticTree(IList<SemanticNode> semanticNodes);
    }
}
