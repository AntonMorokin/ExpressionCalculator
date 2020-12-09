using Processing.Semantics.Factories;
using Processing.Semantics.Model;
using Processing.Syntax.Model;
using Resources;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Processing.Semantics
{
    internal sealed class SemanticsConverter : ISemanticsConverter
    {
        private readonly IResourceStore _resourceStore;
        private readonly IFunctionPriorityStore _functionPriorityStore;

        public SemanticsConverter(IResourceStore resourceStore, IFunctionPriorityStore functionPriorityStore)
        {
            _functionPriorityStore = functionPriorityStore;
            _resourceStore = resourceStore;
        }

        public IList<SemanticNode> ConvertSyntaxToSemantics(IList<SyntaxToken> syntaxTokens)
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
                            var childNodes = ConvertSyntaxToSemantics(bst.ChildTokens);

                            node = new BracesSemanticNode(childNodes);
                        }
                        break;
                    case SyntaxTokenTypes.UnaryFunction:
                        {
                            var ufst = (UnaryFunctionSyntaxToken)token;
                            var braces = (BracesSemanticNode)ConvertSyntaxToSemantics(new[] { ufst.Braces }).Single();

                            node = new UnaryFunctionSemanticNode(ufst.Value, braces);
                        }
                        break;
                    default:
                        {
                            throw new NotSupportedException(
                                _resourceStore.GetExceptionMessage("UnknownSyntaxTokenType", token.Type));
                        }
                }

                flatSemanticNodes.Add(node);
            }

            return flatSemanticNodes;
        }
    }
}
