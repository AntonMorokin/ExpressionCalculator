using Calculation.Model.Functions.Binary;
using Parsing.Model;
using System.Collections.Generic;

namespace Parsing
{
    public sealed class Converter
    {
        public IList<TreeNode> Convert(IList<ListNode> listNodes)
        {
            var flatTreeNodes = new List<TreeNode>(16);

            foreach (var listNode in listNodes)
            {
                var node = new TreeNode
                {
                    Value = listNode.MainValue
                };

                if (listNode.HasSubNodes)
                {
                    node.SubNodes = Convert(listNode.SubNodes);
                }

                flatTreeNodes.Add(node);
            }

            TreeNode currentListNode;
            var treeNodes = new List<TreeNode>(16);

            // TODO: Temporary
            if (flatTreeNodes.Count < 2)
            {
                return flatTreeNodes;
            }

            for (int i = 0; i < flatTreeNodes.Count; i++)
            {
                currentListNode = flatTreeNodes[i];
                if (currentListNode.Value is BinaryFunction)
                {
                    currentListNode.LeftChild = flatTreeNodes[i - 1];
                    currentListNode.RightChild = flatTreeNodes[i + 1];

                    treeNodes.Add(currentListNode);

                    i++;
                    continue;
                }
            }

            return treeNodes;
        }
    }
}
