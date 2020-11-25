using Processing.Syntax.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Processing.Syntax
{
    internal class SyntaxTokenFactory : ISyntaxTokenFactory
    {
        private readonly IReadOnlyCollection<char> _knownBinaryFunctions = new List<char>
        {
            '+',
            '-',
            '*',
            '/'
        };

        private readonly IReadOnlyCollection<string> _knownUnaryFunctions = new List<string>
        {
            "log2"
        };

        public IEnumerable<char> KnownBinaryFunctions => _knownBinaryFunctions;

        public SyntaxToken ParseToken(string valueToParse)
        {
            if (decimal.TryParse(valueToParse, NumberStyles.Any, CultureInfo.InvariantCulture, out _))
            {
                return new SyntaxToken(SyntaxTokenTypes.Number, valueToParse);
            }

            if (TryParseBinaryFunction(valueToParse, out var commandNode))
            {
                return commandNode;
            }

            return ParseUnaryFunction(valueToParse);
        }

        private bool TryParseBinaryFunction(string value, out SyntaxToken parsedToken)
        {
            parsedToken = null;

            if (value.Length == 1
                && _knownBinaryFunctions.Any(f => f == value[0]))
            {
                parsedToken = new SyntaxToken(SyntaxTokenTypes.BinaryFunction, value);

                return true;
            }

            return false;
        }

        private SyntaxToken ParseUnaryFunction(string valueToParse)
        {
            if (_knownUnaryFunctions.Any(f => f == valueToParse))
            {
                return new UnaryFunctionSyntaxToken(valueToParse);
            }

            throw new NotSupportedException($"Unknown unary function: {valueToParse}");
        }
    }
}
