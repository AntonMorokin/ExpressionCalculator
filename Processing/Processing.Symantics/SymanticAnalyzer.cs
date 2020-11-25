using Processing.Symantics.Model;
using System;
using System.Collections.Generic;

namespace Processing.Symantics
{
    internal sealed class SymanticAnalyzer : ISymanticAnalyzer
    {
        public SymanticNode BuildSymanticTree(IList<SymanticNode> symanticNodes)
        {
            // TODO: maybe I need to cope input list to not affect callers.

            SymanticNode currentNode = symanticNodes[0];
            SymanticNode nextNode;
            bool multipleLineExists = false;

            do
            {
                // Step over all nodes.
                for (int i = 1; i < symanticNodes.Count; i++)
                {
                    nextNode = symanticNodes[i];

                    // TODO: code is duplicated and can be moved to the method.
                    if (!(currentNode.Type == SymanticNodeTypes.BinaryFunction
                        && nextNode.Type == SymanticNodeTypes.BinaryFunction))
                    {
                        throw new NotSupportedException("Not supported type of symantic nodes.");
                    }

                    var currentNodeFunction = (BinaryFunctionSymanticNode)currentNode;
                    var nextNodeFunction = (BinaryFunctionSymanticNode)nextNode;

                    // When priorities of current and next nodes are equal
                    if (currentNodeFunction.Priority == nextNodeFunction.Priority)
                    {
                        // Then merge current node to leftmost node of next node.
                        MergeNodeToLeftSide(nextNodeFunction, currentNodeFunction);
                        symanticNodes.Remove(currentNode);
                        i--;
                    }

                    // make next node current
                    currentNode = nextNode;
                }

            } while (multipleLineExists);

            // When we steped over all nodes and did not make any swap
            // Then step over all nodes one more time
            while (symanticNodes.Count > 1)
            {
                currentNode = symanticNodes[0];
                nextNode = symanticNodes[1];

                if (!(currentNode.Type == SymanticNodeTypes.BinaryFunction
                        && nextNode.Type == SymanticNodeTypes.BinaryFunction))
                {
                    throw new NotSupportedException("Not supported type of symantic nodes.");
                }

                var currentNodeFunction = (BinaryFunctionSymanticNode)currentNode;
                var nextNodeFunction = (BinaryFunctionSymanticNode)nextNode;

                // But merge them depending on their priority.
                if (currentNodeFunction.Priority < nextNodeFunction.Priority)
                {
                    MergeNodeToRightSide(currentNodeFunction, nextNodeFunction);
                    symanticNodes.Remove(nextNode);
                }
                else
                {
                    MergeNodeToLeftSide(nextNodeFunction, currentNodeFunction);
                    symanticNodes.Remove(currentNode);
                }

            }

            var root = symanticNodes[0];

            if (root.Type == SymanticNodeTypes.Braces)
            {
                // The case when only one node exists and it is braces: (1 + 2 * 3)
                var braces = (BracesSymanticNode)root;
                root = BuildSymanticTree(braces.ChildNodes);
            }
            else
            {
                ConvertAllChildBracesToTrees(root);
            }

            return root;
        }

        private void MergeNodeToLeftSide(BinaryFunctionSymanticNode root, BinaryFunctionSymanticNode node)
        {
            var currentNode = root;
            while (currentNode.LeftChild.Type == SymanticNodeTypes.BinaryFunction)
            {
                currentNode = (BinaryFunctionSymanticNode)currentNode.LeftChild;
            }

            currentNode.LeftChild = node;
        }

        private void MergeNodeToRightSide(BinaryFunctionSymanticNode root, BinaryFunctionSymanticNode node)
        {
            var currentNode = root;
            while (currentNode.RightChild.Type == SymanticNodeTypes.BinaryFunction)
            {
                currentNode = (BinaryFunctionSymanticNode)currentNode.RightChild;
            }

            currentNode.RightChild = node;
        }

        private void ConvertAllChildBracesToTrees(SymanticNode symanticNode)
        {
            void Analyze(SymanticNode nodeToAnalyze, Action<SymanticNode> swapChilds)
            {
                if (nodeToAnalyze.Type == SymanticNodeTypes.Braces)
                {
                    var braces = (BracesSymanticNode)nodeToAnalyze;
                    var newChild = BuildSymanticTree(braces.ChildNodes);

                    swapChilds(newChild);
                }
                else if (nodeToAnalyze.Type != SymanticNodeTypes.Number)
                {
                    ConvertAllChildBracesToTrees(nodeToAnalyze);
                }
            }

            switch (symanticNode.Type)
            {
                case SymanticNodeTypes.UnaryFunction:
                    {
                        var func = (UnaryFunctionSymanticNode)symanticNode;

                        Analyze(func.Child, (nc) => func.Child = nc);
                    }
                    break;
                case SymanticNodeTypes.BinaryFunction:
                    {

                        var func = (BinaryFunctionSymanticNode)symanticNode;

                        Analyze(func.LeftChild, (nc) => func.LeftChild = nc);
                        Analyze(func.RightChild, (nc) => func.RightChild = nc);
                    }
                    break;
            }
        }
    }
}
