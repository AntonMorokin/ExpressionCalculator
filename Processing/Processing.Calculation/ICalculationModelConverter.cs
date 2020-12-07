using Calculation.Model;
using Processing.Semantics.Model;

namespace Processing.Calculation
{
    public interface ICalculationModelConverter
    {
        IHasValue ConvertToCalculationModel(SemanticNode semanticTreeRoot);
    }
}