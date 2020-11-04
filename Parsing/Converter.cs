using Calculation.Model;
using ExpressionTrees.Model.List;
using ExpressionTrees.Model.Tree;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Parsing
{
    public sealed class Converter
    {
        public IReadOnlyCollection<ITreeNode> Convert(IList<IListNode<IHasValue>> listNodes)
        {
            var flatTreeNodes = new List<ITreeNode>(16);

            foreach (var listNode in listNodes)
            {
                if (listNode is MultipleListNode<IHasValue> multipleListNode)
                {
                    var values = Convert(multipleListNode.Nodes.ToList()).ToList();
                    flatTreeNodes.Add(new ComplexTreeNode(values));
                }
                else
                {
                    flatTreeNodes.Add(new TreeNode(null)
                    {
                        Value = ((SingleListNode<IHasValue>)listNode).Value
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

            return treeNodes.ToImmutableList();
        }
    }
}
