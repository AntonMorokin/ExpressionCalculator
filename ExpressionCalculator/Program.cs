using Common;
using Processing.Symantics;
using Processing.Syntax;
using System;

namespace ExpressionCalculator
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Type expression:");
            var s = Console.ReadLine();

            var syntaxParser = TypeFactory.Get<ISyntaxTokenParser>();
            var symanticsTransformer = TypeFactory.Get<ISymanticsTransformer>();
            var symanticAnalyzer = TypeFactory.Get<ISymanticAnalyzer>();

            var syntaxTokens = syntaxParser.ParseSyntaxTokens(s);
            var symanticNodes = symanticsTransformer.TransformSyntaxToSymantics(syntaxTokens);
            var tree = symanticAnalyzer.BuildSymanticTree(symanticNodes);
            var expression = symanticsTransformer.TransformSymanticTreeToCalculationModel(tree);

            Console.WriteLine($"Result = {expression.GetValue()}");
        }
    }
}
