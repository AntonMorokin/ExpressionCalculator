using Common;
using Processing.Calculation;
using System;

namespace ExpressionCalculator
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Type expression:");
            var s = Console.ReadLine();

            var calculator = TypeFactory.Get<IExpressionCalculator>();
            var result = calculator.CalculateExpresion(s);

            Console.WriteLine($"Result = {result}");
        }
    }
}
