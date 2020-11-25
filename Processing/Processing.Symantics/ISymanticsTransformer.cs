using Calculation.Model;
using Processing.Symantics.Model;
using Processing.Syntax.Model;
using System.Collections.Generic;

namespace Processing.Symantics
{
    public interface ISymanticsTransformer
    {
        IList<SymanticNode> TransformSyntaxToSymantics(IList<SyntaxToken> syntaxTokens);

        IHasValue TransformSymanticTreeToCalculationModel(SymanticNode symanticTreeRoot);
    }
}
