using Calculation.Model;
using ExpressionTrees.Model.Tree;
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

                    if (!(currentNode.Value is Function currentNodeFunction
                        && nextNode.Value is Function nextNodeFunction))
                    {
                        throw new InvalidOperationException("Not supported type of root node value.");
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

                if (!(currentNode.Value is Function currentNodeFunction
                    && nextNode.Value is Function nextNodeFunction))
                {
                    throw new InvalidOperationException("Not supported type of root node value.");
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

            ConvertBracesToTree(treeNodes[0]);

            return treeNodes[0];
        }

        private void MergeNodeToLeftSide(TreeNode root, ITreeNode node)
        {
            var currentNode = root;
            while (!IsLeafFromLeftSide(currentNode.LeftChild))
            {
                currentNode = (TreeNode)currentNode.LeftChild;
            }

            currentNode.LeftChild = node;
        }

        private bool IsLeafFromLeftSide(ITreeNode treeNode)
        {
            if (treeNode is ComplexTreeNode)
            {
                return true;
            }

            if (treeNode is TreeNode tn)
            {
                return tn.LeftChild == null;
            }

            throw new InvalidOperationException("Unknown tree node type.");
        }

        private void MergeNodeToRightSide(TreeNode root, ITreeNode node)
        {
            var currentNode = root;
            while (!IsLeafFromRightSide(currentNode.RightChild))
            {
                currentNode = (TreeNode)currentNode.RightChild;
            }

            currentNode.RightChild = node;
        }

        private bool IsLeafFromRightSide(ITreeNode treeNode)
        {
            if (treeNode is ComplexTreeNode)
            {
                return true;
            }

            if (treeNode is TreeNode tn)
            {
                return tn.RightChild == null;
            }

            throw new InvalidOperationException("Unknown tree node type.");
        }

        private void ConvertBracesToTree(TreeNode treeNode)
        {
            if (treeNode.LeftChild is ComplexTreeNode lctn)
            {
                // Convert braces to tree
                var newLeftNode = TransformToTree(lctn.Values.OfType<TreeNode>().ToList());
                treeNode.LeftChild = newLeftNode;
            }
            else if (treeNode.LeftChild is TreeNode ltn
                && ltn.Value is Function)
            {
                ConvertBracesToTree(ltn);
            }

            if (treeNode.RightChild is ComplexTreeNode rctn)
            {
                // Convert braces to tree
                var newRightNode = TransformToTree(rctn.Values.OfType<TreeNode>().ToList());
                treeNode.RightChild = newRightNode;
            }
            else if (treeNode.RightChild is TreeNode rtn
                && rtn.Value is Function)
            {
                ConvertBracesToTree(rtn);
            }
        }
    }
}
