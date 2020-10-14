using Calculation.Model;
using Calculation.Model.Factories;
using Calculation.Model.Functions.Binary;
using ExpressionTrees.Model;
using Resources;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Parsing
{
    public sealed class ExpressionParser
    {
        private static readonly IEnumerable<char> __knownOperationChars = new[]
        {
            '+', '-', '*', '/'
        };

        private const char OPENING_BRACE_CHAR = '(';
        private const char CLOSING_BRACE_CHAR = ')';

        private readonly IResourceStore _resourceStore;
        private readonly INumberFactory _numberFactory;

        public ExpressionParser(IResourceStore resourceStore, INumberFactory numberFactory)
        {
            _resourceStore = resourceStore;
            _numberFactory = numberFactory;
        }

        public IReadOnlyCollection<IListNode<IHasValue>> ParseToSimpleList(string expression)
        {
            expression = expression?.Trim();

            if (string.IsNullOrEmpty(expression))
            {
                throw new ArgumentNullException(nameof(expression));
            }

            var list = new List<IListNode<IHasValue>>(16);

            char currentChar;
            int lastSignificantCharIndex = 0;
            int i;

            for (i = 0; i < expression.Length; i++)
            {
                currentChar = expression[i];
                if (currentChar == OPENING_BRACE_CHAR)
                {
                    (int lastIndex, var bracesNode) = ParseBraces(expression, i + 1);

                    list.Add(bracesNode);

                    i = lastIndex;
                    lastSignificantCharIndex = i + 1;

                    continue;
                }

                if (__knownOperationChars.Contains(currentChar))
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

            return list.ToImmutableList();
        }

        private (int lastIndex, IListNode<IHasValue> node) ParseBraces(string expression, int startIndex)
        {
            int nestingLevel = 0;
            string expressionInBraces = string.Empty;
            char currentChar;

            for (int i = startIndex; i < expression.Length; i++)
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
                    expressionInBraces = expression[startIndex..i];
                    var nodes = ParseToSimpleList(expressionInBraces);

                    return (i, new MultipleListNode<IHasValue>(nodes));
                }
            }

            throw new InvalidOperationException($"Did not find closing brace from {startIndex}: {expression}");
        }

        private IListNode<IHasValue> ParseNode(string valueToParse)
        {
            var node = new SingleListNode<IHasValue>();

            if (valueToParse.Length == 1)
            {
                char @char = valueToParse[0];
                switch (@char)
                {
                    case '+':
                        node.Value = new Plus(_resourceStore, _numberFactory);
                        return node;
                    case '-':
                        node.Value = new Minus(_resourceStore, _numberFactory);
                        return node;
                    case '*':
                        node.Value = new Multiply(_resourceStore, _numberFactory);
                        return node;
                    case '/':
                        node.Value = new Divide(_resourceStore, _numberFactory);
                        return node;
                }
            }
            
            node.Value = _numberFactory.CreateNumber(decimal.Parse(valueToParse, CultureInfo.CurrentCulture));

            return node;
        }
    }
}
