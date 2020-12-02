using System.Collections.Generic;
using System.Linq;

namespace Processing.Semantics.Model
{
    public sealed class BracesSemanticNode : SemanticNode
    {
        public IList<SemanticNode> ChildNodes { get; set; }

        public BracesSemanticNode(IEnumerable<SemanticNode> childNodes)
            : base(SemanticNodeTypes.Braces, null)
        {
            ChildNodes = childNodes?.ToList() ?? new List<SemanticNode>();
        }
    }
}
