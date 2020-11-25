using Calculation.Model;
using Calculation.Model.Factories;
using Processing.Symantics.Factories;
using Processing.Symantics.Model;
using Processing.Syntax.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Processing.Symantics
{
    internal sealed class SymanticsTransformer : ISymanticsTransformer
    {
        private readonly IFunctionPriorityStore _functionPriorityStore;
        private readonly ICalculationObjectFactory _calculationObjectFactory;

        public SymanticsTransformer(IFunctionPriorityStore functionPriorityStore, ICalculationObjectFactory calculationObjectFactory)
        {
            _functionPriorityStore = functionPriorityStore;
            _calculationObjectFactory = calculationObjectFactory;
        }

        public IList<SymanticNode> TransformSyntaxToSymantics(IList<SyntaxToken> syntaxTokens)
        {
            var flatSymanticNodes = CreateFlatList(syntaxTokens);

            if (!flatSymanticNodes.Any(n => n.Type == SymanticNodeTypes.BinaryFunction))
            {
                return flatSymanticNodes;
            }

            SymanticNode currentNode;
            var symanticTokens = new List<SymanticNode>(16);

            for (int i = 0; i < flatSymanticNodes.Count; i++)
            {
                currentNode = flatSymanticNodes[i];
                if (currentNode.Type == SymanticNodeTypes.BinaryFunction)
                {
                    var binaryFunctionNode = (BinaryFunctionSymanticNode)currentNode;

                    binaryFunctionNode.LeftChild = flatSymanticNodes[i - 1];
                    binaryFunctionNode.RightChild = flatSymanticNodes[i + 1];

                    symanticTokens.Add(binaryFunctionNode);

                    i++;
                    continue;
                }
            }

            return symanticTokens;
        }

        private List<SymanticNode> CreateFlatList(IList<SyntaxToken> syntaxTokens)
        {
            var flatSymanticNodes = new List<SymanticNode>(16);

            foreach (var token in syntaxTokens)
            {
                SymanticNode node;

                switch (token.Type)
                {
                    case SyntaxTokenTypes.Number:
                        {
                            node = new NumberSymanticNode(token.Value);
                        }
                        break;
                    case SyntaxTokenTypes.BinaryFunction:
                        {
                            var priority = _functionPriorityStore.GetPripority(token.Value);
                            node = new BinaryFunctionSymanticNode(token.Value, priority);
                        }
                        break;
                    case SyntaxTokenTypes.Braces:
                        {
                            var bst = (BracesSyntaxToken)token;
                            var childNodes = TransformSyntaxToSymantics(bst.ChildTokens);

                            node = new BracesSymanticNode(childNodes);
                        }
                        break;
                    case SyntaxTokenTypes.UnaryFunction:
                        {
                            var ufst = (UnaryFunctionSyntaxToken)token;
                            var braces = (BracesSymanticNode)TransformSyntaxToSymantics(new[] { ufst.Braces }).Single();

                            node = new UnaryFunctionSymanticNode(ufst.Value, braces);
                        }
                        break;
                    default:
                        {
                            throw new NotSupportedException($"Unknown syntax token type: {token.Type}.");
                        }
                }

                flatSymanticNodes.Add(node);
            }

            return flatSymanticNodes;
        }

        public IHasValue TransformSymanticTreeToCalculationModel(SymanticNode symanticTreeRoot)
        {
            var nodeType = symanticTreeRoot.Type;

            switch (nodeType)
            {
                case SymanticNodeTypes.Number:
                    {
                        return _calculationObjectFactory.Create(symanticTreeRoot.Value);
                    }
                case SymanticNodeTypes.BinaryFunction:
                    {
                        var binaryFunctionNode = (BinaryFunctionSymanticNode)symanticTreeRoot;

                        var firstValue = TransformSymanticTreeToCalculationModel(binaryFunctionNode.LeftChild);
                        var secondValue = TransformSymanticTreeToCalculationModel(binaryFunctionNode.RightChild);

                        var func = (Function)_calculationObjectFactory.Create(binaryFunctionNode.Value);

                        func.SetArguments(firstValue, secondValue);

                        return func;
                    }
                case SymanticNodeTypes.UnaryFunction:
                    {
                        var unaryFunctionNode = (UnaryFunctionSymanticNode)symanticTreeRoot;

                        var value = TransformSymanticTreeToCalculationModel(unaryFunctionNode.Child);

                        var func = (Function)_calculationObjectFactory.Create(unaryFunctionNode.Value);

                        func.SetArguments(value);

                        return func;
                    }
                default:
                    throw new NotSupportedException($"Unknown symantic node type: {symanticTreeRoot.Type}.");
            }
        }
    }
}
