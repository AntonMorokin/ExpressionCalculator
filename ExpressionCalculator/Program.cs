using Calculation.Model.Factories;
using Common;
using ExpressionTrees.Model.Tree;
using Parsing;
using Resources;
using System;
using System.Linq;

namespace ExpressionCalculator
{
    class Program
    {
        static void Main(string[] args)
        {
            var s = Console.ReadLine();

            var parser = new ExpressionParser(TypeFactory.Get<IResourceStore>(), TypeFactory.Get<INumberFactory>());
            var list = parser.ParseToSimpleList(s);

            var converter = new Converter();
            var rootList = converter.Convert(list.ToList());

            var transformer = new Transformer();
            var tree = transformer.TransformToTree(rootList.OfType<TreeNode>().ToList());

            var treeConverter = new TreeConverter();
            var expression = treeConverter.Convert(tree);

            Console.WriteLine($"Result = {expression.GetValue()}");
        }
    }
}
