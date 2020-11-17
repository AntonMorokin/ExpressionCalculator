using Calculation.Model;
using Calculation.Model.Functions.Binary;
using System;
using System.Collections.Generic;
using System.Text;

namespace Parsing.Model
{
    public class TreeNode
    {
        public IHasValue Value { get; set; }

        public IList<TreeNode> SubNodes { get; set; }

        public TreeNode LeftChild { get; set; }

        public TreeNode RightChild { get; set; }

        public bool HasSubNodes => (SubNodes?.Count ?? 0) > 0;
    }
}
