using Calculation.Model;
using Calculation.Model.Factories;
using Processing.Semantics.Factories;
using Processing.Semantics.Model;
using Processing.Syntax.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Processing.Semantics
{
    internal sealed class SemanticsTransformer : ISemanticsTransformer
    {
        private readonly IFunctionPriorityStore _functionPriorityStore;
        private readonly ICalculationObjectFactory _calculationObjectFactory;

        public SemanticsTransformer(IFunctionPriorityStore functionPriorityStore, ICalculationObjectFactory calculationObjectFactory)
        {
            _functionPriorityStore = functionPriorityStore;
            _calculationObjectFactory = calculationObjectFactory;
        }

        public IList<SemanticNode> TransformSyntaxToSemantics(IList<SyntaxToken> syntaxTokens)
        {
            var flatSemanticNodes = CreateFlatList(syntaxTokens);

            if (!flatSemanticNodes.Any(n => n.Type == SemanticNodeTypes.BinaryFunction))
            {
                return flatSemanticNodes;
            }

            SemanticNode currentNode;
            var semanticTokens = new List<SemanticNode>(16);

            for (int i = 0; i < flatSemanticNodes.Count; i++)
            {
                currentNode = flatSemanticNodes[i];
                if (currentNode.Type == SemanticNodeTypes.BinaryFunction)
                {
                    var binaryFunctionNode = (BinaryFunctionSemanticNode)currentNode;

                    binaryFunctionNode.LeftChild = flatSemanticNodes[i - 1];
                    binaryFunctionNode.RightChild = flatSemanticNodes[i + 1];

                    semanticTokens.Add(binaryFunctionNode);

                    i++;
                    continue;
                }
            }

            return semanticTokens;
        }

        private List<SemanticNode> CreateFlatList(IList<SyntaxToken> syntaxTokens)
        {
            var flatSemanticNodes = new List<SemanticNode>(16);

            foreach (var token in syntaxTokens)
            {
                SemanticNode node;

                switch (token.Type)
                {
                    case SyntaxTokenTypes.Number:
                        {
                            node = new NumberSemanticNode(token.Value);
                        }
                        break;
                    case SyntaxTokenTypes.BinaryFunction:
                        {
                            var priority = _functionPriorityStore.GetPripority(token.Value);
                            node = new BinaryFunctionSemanticNode(token.Value, priority);
                        }
                        break;
                    case SyntaxTokenTypes.Braces:
                        {
                            var bst = (BracesSyntaxToken)token;
                            var childNodes = TransformSyntaxToSemantics(bst.ChildTokens);

                            node = new BracesSemanticNode(childNodes);
                        }
                        break;
                    case SyntaxTokenTypes.UnaryFunction:
                        {
                            var ufst = (UnaryFunctionSyntaxToken)token;
                            var braces = (BracesSemanticNode)TransformSyntaxToSemantics(new[] { ufst.Braces }).Single();

                            node = new UnaryFunctionSemanticNode(ufst.Value, braces);
                        }
                        break;
                    default:
                        {
                            throw new NotSupportedException($"Unknown syntax token type: {token.Type}.");
                        }
                }

                flatSemanticNodes.Add(node);
            }

            return flatSemanticNodes;
        }

        public IHasValue TransformSemanticTreeToCalculationModel(SemanticNode semanticTreeRoot)
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

                        var firstValue = TransformSemanticTreeToCalculationModel(binaryFunctionNode.LeftChild);
                        var secondValue = TransformSemanticTreeToCalculationModel(binaryFunctionNode.RightChild);

                        var func = (Function)_calculationObjectFactory.Create(binaryFunctionNode.Value);

                        func.SetArguments(firstValue, secondValue);

                        return func;
                    }
                case SemanticNodeTypes.UnaryFunction:
                    {
                        var unaryFunctionNode = (UnaryFunctionSemanticNode)semanticTreeRoot;

                        var value = TransformSemanticTreeToCalculationModel(unaryFunctionNode.Child);

                        var func = (Function)_calculationObjectFactory.Create(unaryFunctionNode.Value);

                        func.SetArguments(value);

                        return func;
                    }
                default:
                    throw new NotSupportedException($"Unknown semantic node type: {semanticTreeRoot.Type}.");
            }
        }
    }
}
