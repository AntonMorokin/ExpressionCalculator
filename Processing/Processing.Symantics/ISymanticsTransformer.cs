using Calculation.Model;
using Processing.Symantics.Model;
using Processing.Syntax.Model;
using System.Collections.Generic;

namespace Processing.Symantics
{
    public interface ISymanticsTransformer
    {
        IList<SymanticNodeOld> TransformSyntaxToSymantics(IList<SyntaxTokenOld> syntaxTokens);

        IHasValue TransformSymanticTreeToCalculationModel(SymanticNodeOld symanticTree);
    }
}
