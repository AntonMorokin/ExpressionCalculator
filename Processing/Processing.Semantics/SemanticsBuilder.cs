using Processing.Semantics.Model;
using Resources;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Processing.Semantics
{
    internal sealed class SemanticsBuilder : ISemanticsBuilder
    {
        private readonly IResourceStore _resourceStore;

        public SemanticsBuilder(IResourceStore resourceStore)
        {
            _resourceStore = resourceStore;
        }

        public SemanticNode BuildSemanticTree(IList<SemanticNode> semanticNodes)
        {
            ValidateSymanticNodes(semanticNodes);

            // Copy nodes to not affect callers.
            var nodes = new List<SemanticNode>(semanticNodes);

            MergeNodesWithTheSamePriority(nodes);
            MergeRemainingNodes(nodes);

            var root = nodes[0];

            if (root.Type == SemanticNodeTypes.Braces)
            {
                // The case when only one node exists and it is braces: (1 + 2 * 3)
                var braces = (BracesSemanticNode)root;
                root = BuildSemanticTree(braces.ChildNodes);
            }
            else
            {
                ConvertAllChildBracesToTrees(root);
            }

            return root;
        }

        private void ValidateSymanticNodes(IList<SemanticNode> semanticNodes)
        {
            if (semanticNodes.Count > 1
                && semanticNodes.Any(n => n.Type != SemanticNodeTypes.BinaryFunction))
            {
                throw new InvalidOperationException(
                    _resourceStore.GetExceptionMessage("InvalidSemanticsStateForTreeBuilding"));
            }
        }

        private void MergeNodesWithTheSamePriority(List<SemanticNode> nodes)
        {
            SemanticNode currentNode, nextNode;

            currentNode = nodes[0];
            bool multipleLineExists = false;

            do
            {
                // Step over all nodes.
                for (int i = 1; i < nodes.Count; i++)
                {
                    nextNode = nodes[i];

                    var currentNodeFunction = (BinaryFunctionSemanticNode)currentNode;
                    var nextNodeFunction = (BinaryFunctionSemanticNode)nextNode;

                    // When priorities of current and next nodes are equal
                    if (currentNodeFunction.Priority == nextNodeFunction.Priority)
                    {
                        // Then merge current node to leftmost node of next node.
                        MergeNodeToLeftSide(nextNodeFunction, currentNodeFunction);
                        nodes.Remove(currentNode);
                        i--;
                    }

                    // make next node current
                    currentNode = nextNode;
                }

            } while (multipleLineExists);
        }

        private void MergeNodeToLeftSide(BinaryFunctionSemanticNode root, BinaryFunctionSemanticNode node)
        {
            var currentNode = root;
            while (currentNode.LeftChild.Type == SemanticNodeTypes.BinaryFunction)
            {
                currentNode = (BinaryFunctionSemanticNode)currentNode.LeftChild;
            }

            currentNode.LeftChild = node;
        }

        private void MergeRemainingNodes(List<SemanticNode> nodes)
        {
            SemanticNode currentNode, nextNode;

            // When we steped over all nodes and did not make any swap
            // Then step over all nodes one more time
            while (nodes.Count > 1)
            {
                currentNode = nodes[0];
                nextNode = nodes[1];

                var currentNodeFunction = (BinaryFunctionSemanticNode)currentNode;
                var nextNodeFunction = (BinaryFunctionSemanticNode)nextNode;

                // But merge them depending on their priority.
                if (currentNodeFunction.Priority < nextNodeFunction.Priority)
                {
                    MergeNodeToRightSide(currentNodeFunction, nextNodeFunction);
                    nodes.Remove(nextNode);
                }
                else
                {
                    MergeNodeToLeftSide(nextNodeFunction, currentNodeFunction);
                    nodes.Remove(currentNode);
                }
            }
        }

        private void MergeNodeToRightSide(BinaryFunctionSemanticNode root, BinaryFunctionSemanticNode node)
        {
            var currentNode = root;
            while (currentNode.RightChild.Type == SemanticNodeTypes.BinaryFunction)
            {
                currentNode = (BinaryFunctionSemanticNode)currentNode.RightChild;
            }

            currentNode.RightChild = node;
        }

        private void ConvertAllChildBracesToTrees(SemanticNode semanticNode)
        {
            void Analyze(SemanticNode nodeToAnalyze, Action<SemanticNode> swapChilds)
            {
                if (nodeToAnalyze.Type == SemanticNodeTypes.Braces)
                {
                    var braces = (BracesSemanticNode)nodeToAnalyze;
                    var newChild = BuildSemanticTree(braces.ChildNodes);

                    swapChilds(newChild);
                }
                else if (nodeToAnalyze.Type != SemanticNodeTypes.Number)
                {
                    ConvertAllChildBracesToTrees(nodeToAnalyze);
                }
            }

            switch (semanticNode.Type)
            {
                case SemanticNodeTypes.UnaryFunction:
                    {
                        var funcNode = (UnaryFunctionSemanticNode)semanticNode;

                        Analyze(funcNode.Child, (nc) => funcNode.Child = nc);
                    }
                    break;
                case SemanticNodeTypes.BinaryFunction:
                    {

                        var funcNode = (BinaryFunctionSemanticNode)semanticNode;

                        Analyze(funcNode.LeftChild, (nc) => funcNode.LeftChild = nc);
                        Analyze(funcNode.RightChild, (nc) => funcNode.RightChild = nc);
                    }
                    break;
            }
        }
    }
}
