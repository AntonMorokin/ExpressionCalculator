using System.Collections.Generic;
using System.Linq;

namespace Processing.Symantics.Model
{
    public sealed class BracesSymanticNode : SymanticNode
    {
        public IList<SymanticNode> ChildNodes { get; set; }

        public BracesSymanticNode(IEnumerable<SymanticNode> childNodes)
            : base(SymanticNodeTypes.Braces, null)
        {
            ChildNodes = childNodes?.ToList() ?? new List<SymanticNode>();
        }
    }
}
