using Calculation.Model;
using System.Collections.Generic;

namespace Parsing.Model
{
    public class ListNode
    {
        public IHasValue MainValue { get; set; }

        public IList<ListNode> SubNodes { get; set; }

        public bool HasSubNodes => (SubNodes?.Count ?? 0) > 0;
    }
}
