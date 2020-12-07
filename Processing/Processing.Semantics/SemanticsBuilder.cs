using Processing.Semantics.Model;
using System;
using System.Collections.Generic;

namespace Processing.Semantics
{
    internal sealed class SemanticsBuilder : ISemanticsBuilder
    {
        public SemanticNode BuildSemanticTree(IList<SemanticNode> semanticNodes)
        {
            // TODO: maybe I need to copy input list to not affect callers.

            SemanticNode currentNode = semanticNodes[0];
            SemanticNode nextNode;
            bool multipleLineExists = false;

            do
            {
                // Step over all nodes.
                for (int i = 1; i < semanticNodes.Count; i++)
                {
                    nextNode = semanticNodes[i];

                    // TODO: code is duplicated and can be moved to the method.
                    if (!(currentNode.Type == SemanticNodeTypes.BinaryFunction
                        && nextNode.Type == SemanticNodeTypes.BinaryFunction))
                    {
                        throw new NotSupportedException("Not supported type of semantic nodes.");
                    }

                    var currentNodeFunction = (BinaryFunctionSemanticNode)currentNode;
                    var nextNodeFunction = (BinaryFunctionSemanticNode)nextNode;

                    // When priorities of current and next nodes are equal
                    if (currentNodeFunction.Priority == nextNodeFunction.Priority)
                    {
                        // Then merge current node to leftmost node of next node.
                        MergeNodeToLeftSide(nextNodeFunction, currentNodeFunction);
                        semanticNodes.Remove(currentNode);
                        i--;
                    }

                    // make next node current
                    currentNode = nextNode;
                }

            } while (multipleLineExists);

            // When we steped over all nodes and did not make any swap
            // Then step over all nodes one more time
            while (semanticNodes.Count > 1)
            {
                currentNode = semanticNodes[0];
                nextNode = semanticNodes[1];

                if (!(currentNode.Type == SemanticNodeTypes.BinaryFunction
                        && nextNode.Type == SemanticNodeTypes.BinaryFunction))
                {
                    throw new NotSupportedException("Not supported type of semantic nodes.");
                }

                var currentNodeFunction = (BinaryFunctionSemanticNode)currentNode;
                var nextNodeFunction = (BinaryFunctionSemanticNode)nextNode;

                // But merge them depending on their priority.
                if (currentNodeFunction.Priority < nextNodeFunction.Priority)
                {
                    MergeNodeToRightSide(currentNodeFunction, nextNodeFunction);
                    semanticNodes.Remove(nextNode);
                }
                else
                {
                    MergeNodeToLeftSide(nextNodeFunction, currentNodeFunction);
                    semanticNodes.Remove(currentNode);
                }

            }

            var root = semanticNodes[0];

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

        private void MergeNodeToLeftSide(BinaryFunctionSemanticNode root, BinaryFunctionSemanticNode node)
        {
            var currentNode = root;
            while (currentNode.LeftChild.Type == SemanticNodeTypes.BinaryFunction)
            {
                currentNode = (BinaryFunctionSemanticNode)currentNode.LeftChild;
            }

            currentNode.LeftChild = node;
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
                        var func = (UnaryFunctionSemanticNode)semanticNode;

                        Analyze(func.Child, (nc) => func.Child = nc);
                    }
                    break;
                case SemanticNodeTypes.BinaryFunction:
                    {

                        var func = (BinaryFunctionSemanticNode)semanticNode;

                        Analyze(func.LeftChild, (nc) => func.LeftChild = nc);
                        Analyze(func.RightChild, (nc) => func.RightChild = nc);
                    }
                    break;
            }
        }
    }
}
