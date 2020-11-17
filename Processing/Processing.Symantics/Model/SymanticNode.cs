using Calculation.Model;
using System.Collections.Generic;

namespace Processing.Symantics.Model
{
    public class SymanticNode
    {
        public IHasValue Value { get; set; }

        public IList<SymanticNode> SubNodes { get; set; }

        public SymanticNode LeftChild { get; set; }

        public SymanticNode RightChild { get; set; }

        public bool HasSubNodes => (SubNodes?.Count ?? 0) > 0;
    }
}
