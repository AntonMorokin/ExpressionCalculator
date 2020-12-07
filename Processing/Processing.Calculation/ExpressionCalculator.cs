using Processing.Semantics;
using Processing.Syntax;

namespace Processing.Calculation
{
    internal sealed class ExpressionCalculator : IExpressionCalculator
    {
        private readonly ISyntaxProcessor _syntaxProcessor;
        private readonly ISemanticsProcessor _semanticsProcessor;
        private readonly ICalculationModelConverter _converter;

        public ExpressionCalculator(ISyntaxProcessor syntaxProcessor,
            ISemanticsProcessor semanticsProcessor,
            ICalculationModelConverter converter)
        {
            _syntaxProcessor = syntaxProcessor;
            _semanticsProcessor = semanticsProcessor;
            _converter = converter;
        }

        public decimal CalculateExpresion(string expression)
        {
            var syntaxModel = _syntaxProcessor.CreateSyntaxModel(expression);
            var semnaticModel = _semanticsProcessor.CreateSemanticModel(syntaxModel);

            var calculationModel = _converter.ConvertToCalculationModel(semnaticModel);

            return calculationModel.GetValue();
        }
    }
}
