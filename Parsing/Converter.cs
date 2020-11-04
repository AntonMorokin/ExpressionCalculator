using Calculation.Model;
using ExpressionTrees.Model.Tree;
using Parsing.Model;
using System.Collections.Generic;

namespace Parsing
{
    public sealed class Converter
    {
        public IList<TreeNode> Convert(IList<ListNode> listNodes)
        {
            var flatTreeNodes = new List<ITreeNode>(16);

            foreach (var listNode in listNodes)
            {
                if (listNode.HasSubNodes)
                {
                    var values = Convert(listNode.SubNodes);
                    flatTreeNodes.Add(new ComplexTreeNode(values));
                }
                else
                {
                    flatTreeNodes.Add(new TreeNode(null)
                    {
                        Value = listNode.MainValue
                    });
                }
            }

            ITreeNode currentListNode;
            var treeNodes = new List<TreeNode>(16);

            for (int i = 0; i < flatTreeNodes.Count; i++)
            {
                currentListNode = flatTreeNodes[i];
                if (currentListNode is TreeNode treeNode
                    && treeNode.Value is Function)
                {
                    treeNode.LeftChild = flatTreeNodes[i - 1];
                    treeNode.RightChild = flatTreeNodes[i + 1];

                    treeNodes.Add(treeNode);

                    i++;
                    continue;
                }
            }

            return treeNodes;
        }
    }
}
