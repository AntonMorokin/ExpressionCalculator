using Processing.Syntax.Model;
using Resources;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Processing.Syntax.Factories
{
    internal class SyntaxTokenFactory : ISyntaxTokenFactory
    {
        private readonly IReadOnlyCollection<string> _knownBinaryFunctions = new List<string>
        {
            "+",
            "-",
            "*",
            "/"
        };

        private readonly IReadOnlyCollection<string> _knownUnaryFunctions = new List<string>
        {
            "log2"
        };

        private readonly IResourceStore _resourceStore;

        public SyntaxTokenFactory(IResourceStore resourceStore)
        {
            _resourceStore = resourceStore;
        }

        public IEnumerable<string> KnownBinaryFunctions => _knownBinaryFunctions;

        public SyntaxToken ParseToken(string valueToParse)
        {
            if (decimal.TryParse(valueToParse, NumberStyles.Any, CultureInfo.InvariantCulture, out _))
            {
                return new SyntaxToken(SyntaxTokenTypes.Number, valueToParse);
            }

            if (TryParseBinaryFunction(valueToParse, out var binaryFunctionToken))
            {
                return binaryFunctionToken;
            }

            if (TryParseUnaryFunction(valueToParse, out var unaryFunctionToken))
            {
                return unaryFunctionToken;
            }

            string message = _resourceStore.GetExceptionMessage("UnknownSyntaxToken", valueToParse);
            throw new NotSupportedException(message);
        }

        private bool TryParseBinaryFunction(string value, out SyntaxToken parsedToken)
        {
            if (_knownBinaryFunctions.Any(f => f == value))
            {
                parsedToken = new SyntaxToken(SyntaxTokenTypes.BinaryFunction, value);
                return true;
            }

            parsedToken = null;
            return false;
        }

        private bool TryParseUnaryFunction(string valueToParse, out SyntaxToken parsedToken)
        {
            if (_knownUnaryFunctions.Any(f => f == valueToParse))
            {
                parsedToken = new UnaryFunctionSyntaxToken(valueToParse);
                return true;
            }

            parsedToken = null;
            return false;
        }
    }
}
