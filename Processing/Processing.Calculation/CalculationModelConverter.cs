using Calculation.Model;
using Calculation.Model.Factories;
using Processing.Semantics.Model;
using Resources;
using System;

namespace Processing.Calculation
{
    internal sealed class CalculationModelConverter : ICalculationModelConverter
    {
        private readonly IResourceStore _resourceStore;
        private readonly ICalculationObjectFactory _calculationObjectFactory;

        public CalculationModelConverter(IResourceStore resourceStore, ICalculationObjectFactory calculationObjectFactory)
        {
            _calculationObjectFactory = calculationObjectFactory;
            _resourceStore = resourceStore;
        }

        public IHasValue ConvertToCalculationModel(SemanticNode semanticTreeRoot)
        {
            var nodeType = semanticTreeRoot.Type;

            switch (nodeType)
            {
                case SemanticNodeTypes.Number:
                    {
                        return _calculationObjectFactory.Create(semanticTreeRoot.Value);
                    }
                case SemanticNodeTypes.BinaryFunction:
                    {
                        var binaryFunctionNode = (BinaryFunctionSemanticNode)semanticTreeRoot;

                        var firstValue = ConvertToCalculationModel(binaryFunctionNode.LeftChild);
                        var secondValue = ConvertToCalculationModel(binaryFunctionNode.RightChild);

                        var func = (Function)_calculationObjectFactory.Create(binaryFunctionNode.Value);

                        func.SetArguments(firstValue, secondValue);

                        return func;
                    }
                case SemanticNodeTypes.UnaryFunction:
                    {
                        var unaryFunctionNode = (UnaryFunctionSemanticNode)semanticTreeRoot;

                        var value = ConvertToCalculationModel(unaryFunctionNode.Child);

                        var func = (Function)_calculationObjectFactory.Create(unaryFunctionNode.Value);

                        func.SetArguments(value);

                        return func;
                    }
                default:
                    throw new NotSupportedException(
                        _resourceStore.GetExceptionMessage("UnknownSemanticNodeType", semanticTreeRoot.Type));
            }
        }
    }
}
