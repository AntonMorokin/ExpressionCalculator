using Processing.Calculation;
using System;

namespace ExpressionCalculator
{
    internal sealed class ExpressionCalculatorProcessor : IExpressionCalculatorProcessor
    {
        private readonly IExpressionCalculator _calculator;

        public ExpressionCalculatorProcessor(IExpressionCalculator calculator)
        {
            _calculator = calculator;
        }

        public void Process()
        {
            try
            {
                ShowMessage("Type expression:");
                string expressionToCalculate = ReadInput();

                decimal result = _calculator.CalculateExpresion(expressionToCalculate);

                ShowMessage($"Expression result = {result}");
            }
            catch (Exception e)
            {
                ShowMessage("Error was occured:");
                ShowMessage(e.Message);
            }
        }

        private void ShowMessage(string value, bool asNewLine = true)
        {
            if (asNewLine)
            {
                Console.WriteLine(value);
            }
            else
            {
                Console.Write(value);
            }
        }

        private string ReadInput()
        {
            return Console.ReadLine();
        }
    }
}
