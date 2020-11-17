using Calculation.Model;
using Calculation.Model.Functions.Binary;
using Parsing.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Parsing
{
    public sealed class Transformer
    {
        public TreeNode TransformToTree(IList<TreeNode> treeNodes)
        {
            TreeNode currentNode = treeNodes[0];
            TreeNode nextNode;
            bool multipleLineExists = false;

            do
            {
                // Step over all nodes.
                for (int i = 1; i < treeNodes.Count; i++)
                {
                    nextNode = treeNodes[i];

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
                        treeNodes.Remove(currentNode);
                        i--;
                    }

                    // make next node current
                    currentNode = nextNode;
                }

            } while (multipleLineExists);

            // When we steped over all nodes and did not make any swap
            // Then step over all nodes one more time
            while (treeNodes.Count > 1)
            {
                currentNode = treeNodes[0];
                nextNode = treeNodes[1];

                if (!(currentNode.Value is BinaryFunction currentNodeFunction
                    && nextNode.Value is BinaryFunction nextNodeFunction))
                {
                    throw new NotSupportedException("Not supported type of root node value.");
                }

                // But merge them depending on their priority.
                if (currentNodeFunction.Priority < nextNodeFunction.Priority)
                {
                    MergeNodeToRightSide(currentNode, nextNode);
                    treeNodes.Remove(nextNode);
                }
                else
                {
                    MergeNodeToLeftSide(nextNode, currentNode);
                    treeNodes.Remove(currentNode);
                }

            }

            ConvertSubNodesToTree(treeNodes[0]);

            return treeNodes[0];
        }

        private void MergeNodeToLeftSide(TreeNode root, TreeNode node)
        {
            var currentNode = root;
            while (!LeftChildIsLeaf(currentNode.LeftChild))
            {
                currentNode = currentNode.LeftChild;
            }

            currentNode.LeftChild = node;
        }

        private bool LeftChildIsLeaf(TreeNode treeNode)
        {
            if (treeNode.HasSubNodes)
            {
                return true;
            }

            return treeNode.LeftChild == null;
        }

        private void MergeNodeToRightSide(TreeNode root, TreeNode node)
        {
            var currentNode = root;
            while (!RightChildIsLeaf(currentNode.RightChild))
            {
                currentNode = currentNode.RightChild;
            }

            currentNode.RightChild = node;
        }

        private bool RightChildIsLeaf(TreeNode treeNode)
        {
            if (treeNode.HasSubNodes)
            {
                return true;
            }

            return treeNode.RightChild == null;
        }

        private void ConvertSubNodesToTree(TreeNode treeNode)
        {
            if (treeNode.LeftChild != null)
            {
                var leftChild = treeNode.LeftChild;

                if (leftChild.HasSubNodes)
                {
                    var newLeftNode = TransformToTree(leftChild.SubNodes);
                    if (leftChild.Value == null)
                    {
                        treeNode.LeftChild = newLeftNode;
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

            if (treeNode.RightChild != null)
            {
                var rightChild = treeNode.RightChild;

                if (rightChild.HasSubNodes)
                {
                    var newRightNode = TransformToTree(rightChild.SubNodes);
                    if (rightChild.Value == null)
                    {
                        treeNode.RightChild = newRightNode;
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
