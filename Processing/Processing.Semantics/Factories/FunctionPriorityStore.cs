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

        public int GetPripority(string function)
        {
            if (_knownFunctionPriorities.TryGetValue(function, out int prioroty))
            {
                return prioroty;
            }

            throw new NotSupportedException($"Unknown function: {function}.");
        }
    }
}
