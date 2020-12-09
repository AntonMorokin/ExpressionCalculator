using Resources;
using System;
using System.Collections.Generic;

namespace Processing.Semantics.Factories
{
    internal sealed class FunctionPriorityStore : IFunctionPriorityStore
    {
        private readonly IDictionary<string, int> _knownFunctionPriorities
            = new Dictionary<string, int>
            {
                { "+", 0 },
                { "-", 0 },
                { "*", 1 },
                { "/", 1 }
            };

        private readonly IResourceStore _resourceStore;

        public FunctionPriorityStore(IResourceStore resourceStore)
        {
            _resourceStore = resourceStore;
        }

        public int GetPripority(string function)
        {
            if (_knownFunctionPriorities.TryGetValue(function, out int prioroty))
            {
                return prioroty;
            }

            throw new NotSupportedException(
                _resourceStore.GetExceptionMessage("UnknownFunctionForPrioritization", function));
        }
    }
}
