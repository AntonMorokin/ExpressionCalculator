using System.Collections.Generic;

namespace Processing.Syntax
{
    public interface IExpressionParser
    {
        IDictionary<int, string> SplitExpressionToTokens(string expression);
    }
}