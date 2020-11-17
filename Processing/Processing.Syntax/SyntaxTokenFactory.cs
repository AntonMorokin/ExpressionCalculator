using Calculation.Model.Factories;
using Calculation.Model.Functions.Binary;
using Calculation.Model.Functions.Unary;
using Processing.Syntax.Model;
using Resources;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Processing.Syntax
{
    internal class SyntaxTokenFactory : ISyntaxTokenFactory
    {
        private readonly INumberFactory _numberFactory;
        private readonly IDictionary<char, Func<BinaryFunction>> _knownBinaryFunctions;
        private readonly IDictionary<string, Func<UnaryFunction>> _knownUnaryFunctions;

        public IEnumerable<char> KnownBinaryFunctions => _knownBinaryFunctions.Keys;

        public SyntaxTokenFactory(IResourceStore resourceStore, INumberFactory numberFactory)
        {
            _numberFactory = numberFactory;

            _knownBinaryFunctions = new Dictionary<char, Func<BinaryFunction>>
            {
                { '+', () => new Plus(resourceStore, _numberFactory) },
                { '-', () => new Minus(resourceStore, _numberFactory) },
                { '*', () => new Multiply(resourceStore, _numberFactory) },
                { '/', () => new Divide(resourceStore, _numberFactory) }
            };

            _knownUnaryFunctions = new Dictionary<string, Func<UnaryFunction>>
            {
                { "log2", () => new Log2(resourceStore, _numberFactory) }
            };
        }

        public SyntaxToken ParseToken(string valueToParse)
        {
            if (decimal.TryParse(valueToParse, NumberStyles.Any, CultureInfo.CurrentCulture, out var number))
            {
                return new SyntaxToken
                {
                    MainValue = _numberFactory.CreateNumber(number)
                };
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
                && _knownBinaryFunctions.TryGetValue(value[0], out var createBinaryFunction))
            {
                parsedToken = new SyntaxToken
                {
                    MainValue = createBinaryFunction()
                };

                return true;
            }

            return false;
        }

        private SyntaxToken ParseUnaryFunction(string valueToParse)
        {
            if (_knownUnaryFunctions.TryGetValue(valueToParse, out var createUnaryFunction))
            {
                return new SyntaxToken
                {
                    MainValue = createUnaryFunction()
                };
            }

            throw new NotSupportedException($"Unknown unary function: {valueToParse}");
        }
    }
}
