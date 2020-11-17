using Calculation.Model;
using Calculation.Model.Functions.Binary;
using Calculation.Model.Functions.Unary;
using Processing.Symantics.Model;
using Processing.Syntax.Model;
using System;
using System.Collections.Generic;

namespace Processing.Symantics
{
    internal sealed class SymanticsTransformer : ISymanticsTransformer
    {
        public IList<SymanticNode> TransformSyntaxToSymantics(IList<SyntaxToken> syntaxTokens)
        {
            var flatSymanticTokens = new List<SymanticNode>(16);

            foreach (var listNode in syntaxTokens)
            {
                var node = new SymanticNode
                {
                    Value = listNode.MainValue
                };

                if (listNode.HasSubTokens)
                {
                    node.SubNodes = TransformSyntaxToSymantics(listNode.SubTokens);
                }

                flatSymanticTokens.Add(node);
            }

            SymanticNode currentListNode;
            var SymanticTokens = new List<SymanticNode>(16);

            // TODO: Temporary
            if (flatSymanticTokens.Count < 2)
            {
                return flatSymanticTokens;
            }

            for (int i = 0; i < flatSymanticTokens.Count; i++)
            {
                currentListNode = flatSymanticTokens[i];
                if (currentListNode.Value is BinaryFunction)
                {
                    currentListNode.LeftChild = flatSymanticTokens[i - 1];
                    currentListNode.RightChild = flatSymanticTokens[i + 1];

                    SymanticTokens.Add(currentListNode);

                    i++;
                    continue;
                }
            }

            return SymanticTokens;
        }

        public IHasValue TransformSymanticTreeToCalculationModel(SymanticNode symanticTree)
        {
            var value = symanticTree.Value;

            if (value is Number)
            {
                return value;
            }

            if (value is UnaryFunction uf)
            {
                var unaryFunctionValue = TransformSymanticTreeToCalculationModel(symanticTree.LeftChild);

                uf.SetArguments(unaryFunctionValue);

                return uf;
            }

            if (value is BinaryFunction bf)
            {
                var firstValue = TransformSymanticTreeToCalculationModel(symanticTree.LeftChild);
                var secondValue = TransformSymanticTreeToCalculationModel(symanticTree.RightChild);

                bf.SetArguments(firstValue, secondValue);

                return bf;
            }

            throw new InvalidOperationException($"Unknown value type {value?.GetType().Name}");
        }
    }
}
