using Common;
using Processing.Semantics;
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
            var semanticsTransformer = TypeFactory.Get<ISemanticsTransformer>();
            var semanticAnalyzer = TypeFactory.Get<ISemanticAnalyzer>();

            var syntaxTokens = syntaxParser.ParseSyntaxTokens(s);
            var semanticNodes = semanticsTransformer.TransformSyntaxToSemantics(syntaxTokens);
            var tree = semanticAnalyzer.BuildSemanticTree(semanticNodes);
            var expression = semanticsTransformer.TransformSemanticTreeToCalculationModel(tree);

            Console.WriteLine($"Result = {expression.GetValue()}");
        }
    }
}
