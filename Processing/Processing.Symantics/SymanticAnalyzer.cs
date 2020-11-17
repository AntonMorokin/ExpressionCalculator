using Calculation.Model;
using Calculation.Model.Functions.Binary;
using Processing.Symantics.Model;
using System;
using System.Collections.Generic;

namespace Processing.Symantics
{
    internal sealed class SymanticAnalyzer : ISymanticAnalyzer
    {
        public SymanticNode BuildSymanticTree(IList<SymanticNode> symanticNodes)
        {
            SymanticNode currentNode = symanticNodes[0];
            SymanticNode nextNode;
            bool multipleLineExists = false;

            do
            {
                // Step over all nodes.
                for (int i = 1; i < symanticNodes.Count; i++)
                {
                    nextNode = symanticNodes[i];

                    if (!(currentNode.Value is BinaryFunction currentNodeFunction
                        && nextNode.Value is BinaryFunction nextNodeFunction))
                    {
                        throw new NotSupportedException("Not supported type of root node value.");
                    }

                    // When priorities of current and next nodes are equal
                    if (currentNodeFunction.Priority == nextNodeFunction.Priority)
                    {
                        // Then merge current node to leftmost node of next node.
                        MergeNodeToLeftSide(nextNode, currentNode);
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

                if (!(currentNode.Value is BinaryFunction currentNodeFunction
                    && nextNode.Value is BinaryFunction nextNodeFunction))
                {
                    throw new NotSupportedException("Not supported type of root node value.");
                }

                // But merge them depending on their priority.
                if (currentNodeFunction.Priority < nextNodeFunction.Priority)
                {
                    MergeNodeToRightSide(currentNode, nextNode);
                    symanticNodes.Remove(nextNode);
                }
                else
                {
                    MergeNodeToLeftSide(nextNode, currentNode);
                    symanticNodes.Remove(currentNode);
                }

            }

            ConvertSubNodesToTree(symanticNodes[0]);

            return symanticNodes[0];
        }

        private void MergeNodeToLeftSide(SymanticNode root, SymanticNode node)
        {
            var currentNode = root;
            while (!LeftChildIsLeaf(currentNode.LeftChild))
            {
                currentNode = currentNode.LeftChild;
            }

            currentNode.LeftChild = node;
        }

        private bool LeftChildIsLeaf(SymanticNode SymanticToken)
        {
            if (SymanticToken.HasSubNodes)
            {
                return true;
            }

            return SymanticToken.LeftChild == null;
        }

        private void MergeNodeToRightSide(SymanticNode root, SymanticNode node)
        {
            var currentNode = root;
            while (!RightChildIsLeaf(currentNode.RightChild))
            {
                currentNode = currentNode.RightChild;
            }

            currentNode.RightChild = node;
        }

        private bool RightChildIsLeaf(SymanticNode SymanticToken)
        {
            if (SymanticToken.HasSubNodes)
            {
                return true;
            }

            return SymanticToken.RightChild == null;
        }

        private void ConvertSubNodesToTree(SymanticNode SymanticToken)
        {
            if (SymanticToken.LeftChild != null)
            {
                var leftChild = SymanticToken.LeftChild;

                if (leftChild.HasSubNodes)
                {
                    var newLeftNode = BuildSymanticTree(leftChild.SubNodes);
                    if (leftChild.Value == null)
                    {
                        SymanticToken.LeftChild = newLeftNode;
                    }
                    else
                    {
                        leftChild.LeftChild = newLeftNode;
                        leftChild.SubNodes = null;
                    }
                }
                else if (leftChild.Value is Function)
                {
                    ConvertSubNodesToTree(leftChild);
                }
            }

            if (SymanticToken.RightChild != null)
            {
                var rightChild = SymanticToken.RightChild;

                if (rightChild.HasSubNodes)
                {
                    var newRightNode = BuildSymanticTree(rightChild.SubNodes);
                    if (rightChild.Value == null)
                    {
                        SymanticToken.RightChild = newRightNode;
                    }
                    else
                    {
                        rightChild.LeftChild = newRightNode;
                        rightChild.SubNodes = null;
                    }
                }
                else if (rightChild.Value is Function)
                {
                    ConvertSubNodesToTree(rightChild);
                }
            }

        }
    }
}
