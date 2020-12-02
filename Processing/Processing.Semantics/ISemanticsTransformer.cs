using Calculation.Model;
using Processing.Semantics.Model;
using Processing.Syntax.Model;
using System.Collections.Generic;

namespace Processing.Semantics
{
    public interface ISemanticsTransformer
    {
        IList<SemanticNode> TransformSyntaxToSemantics(IList<SyntaxToken> syntaxTokens);

        IHasValue TransformSemanticTreeToCalculationModel(SemanticNode semanticTreeRoot);
    }
}
