using Calculation.Model;
using System.Collections.Generic;

namespace Processing.Symantics.Model
{
    public class SymanticNodeOld
    {
        public IHasValue Value { get; set; }

        public IList<SymanticNodeOld> SubNodes { get; set; }

        public SymanticNodeOld LeftChild { get; set; }

        public SymanticNodeOld RightChild { get; set; }

        public bool HasSubNodes => (SubNodes?.Count ?? 0) > 0;
    }
}
