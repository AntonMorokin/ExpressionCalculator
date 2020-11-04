using Calculation.Model;
using Calculation.Model.Factories;
using Calculation.Model.Functions.Binary;
using Calculation.Model.Functions.Unary;
using Parsing.Model;
using Resources;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.Linq;

namespace Parsing
{
    public sealed class Parser
    {
        private const char OPENING_BRACE_CHAR = '(';
        private const char CLOSING_BRACE_CHAR = ')';

        private readonly INumberFactory _numberFactory;
        private readonly IDictionary<char, BinaryFunction> _knownBinaryFunctions;
        private readonly IDictionary<string, UnaryFunction> _knownUnaryFunctions;

        public Parser(IResourceStore resourceStore, INumberFactory numberFactory)
        {
            _numberFactory = numberFactory;

            _knownBinaryFunctions = new Dictionary<char, BinaryFunction>
            {
                { '+', new Plus(resourceStore, _numberFactory) },
                { '-', new Minus(resourceStore, _numberFactory) },
                { '*', new Multiply(resourceStore, _numberFactory) },
                { '/', new Divide(resourceStore, _numberFactory) }
            };

            _knownUnaryFunctions = new Dictionary<string, UnaryFunction>
            {
                { "log2", new Log2(resourceStore, _numberFactory) }
            };
        }

        public IList<ListNode> ParseToSimpleList(string expression)
        {
            expression = expression?.Trim();

            if (string.IsNullOrEmpty(expression))
            {
                throw new ArgumentNullException(nameof(expression));
            }

            var list = new List<ListNode>(16);

            char currentChar;
            int lastSignificantCharIndex = 0;
            int i;

            for (i = 0; i < expression.Length; i++)
            {
                currentChar = expression[i];
                if (char.IsWhiteSpace(currentChar))
                {
                    continue;
                }

                if (currentChar == OPENING_BRACE_CHAR)
                {
                    var expressionBeforeBraces = expression[lastSignificantCharIndex..i];

                    (int lastIndex, var bracesNode) = ExtractBraces(expression, i);

                    i = lastIndex;
                    lastSignificantCharIndex = i + 1;

                    if (string.IsNullOrWhiteSpace(expressionBeforeBraces))
                    {
                        list.Add(bracesNode);
                    }
                    // We found opening brace, but before it and last command something exists.
                    // Assume this is unary function, i.e. 1 + log2(8), and parse it.
                    else
                    {
                        var unaryFunctionNode = ParseNode(expressionBeforeBraces);
                        unaryFunctionNode.SubNodes = new List<ListNode>(1)
                        {
                            bracesNode
                        };

                        list.Add(unaryFunctionNode);
                    }

                    continue;
                }

                if (_knownBinaryFunctions.Keys.Contains(currentChar))
                {
                    // When false it means we just parsed expression in braces.
                    // Before it there is no value - only function.
                    if (lastSignificantCharIndex < i)
                    {
                        string valueToParse = expression[lastSignificantCharIndex..i].Trim();

                        list.Add(ParseNode(valueToParse));
                    }

                    list.Add(ParseNode(currentChar.ToString()));

                    lastSignificantCharIndex = i + 1;

                    continue;
                }
            }

            if (lastSignificantCharIndex < i)
            {
                string valueToParse = expression[lastSignificantCharIndex..i].Trim();

                list.Add(ParseNode(valueToParse));
            }

            return list;
        }

        private (int lastIndex, ListNode node) ExtractBraces(string expression, int startIndex)
        {
            int nestingLevel = 0;
            char currentChar;

            for (int i = startIndex + 1; i < expression.Length; i++)
            {
                currentChar = expression[i];

                if (currentChar == OPENING_BRACE_CHAR)
                {
                    nestingLevel++;
                    continue;
                }

                if (currentChar == CLOSING_BRACE_CHAR
                    && nestingLevel > 0)
                {
                    nestingLevel--;
                    continue;
                }

                if (currentChar == CLOSING_BRACE_CHAR
                    && nestingLevel == 0)
                {
                    string expressionInBraces = expression[(startIndex + 1)..i];
                    var subNodes = ParseToSimpleList(expressionInBraces);

                    var braceNode = new ListNode
                    {
                        SubNodes = subNodes.ToList()
                    };

                    return (i, braceNode);
                }
            }

            throw new InvalidOperationException($"Did not find closing brace from {startIndex}: {expression}");
        }

        private ListNode ParseNode(string valueToParse)
        {
            if (decimal.TryParse(valueToParse, NumberStyles.Any, CultureInfo.CurrentCulture, out var number))
            {
                return new ListNode
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

        private bool TryParseBinaryFunction(string value, out ListNode parsedCommand)
        {
            parsedCommand = null;

            if (value.Length == 1
                && _knownBinaryFunctions.TryGetValue(value[0], out var binaryFunction))
            {
                parsedCommand = new ListNode
                {
                    MainValue = binaryFunction
                };

                return true;
            }

            return false;
        }

        private ListNode ParseUnaryFunction(string valueToParse)
        {
            valueToParse = valueToParse?.Trim().TrimEnd(OPENING_BRACE_CHAR);

            if (_knownUnaryFunctions.TryGetValue(valueToParse, out var unaryFunction))
            {
                return new ListNode
                {
                    MainValue = unaryFunction
                };
            }

            throw new NotSupportedException($"Unknown unary function: {valueToParse}");
        }
    }
}
