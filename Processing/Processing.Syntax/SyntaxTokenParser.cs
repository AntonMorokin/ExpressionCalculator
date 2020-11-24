using Processing.Syntax.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Processing.Syntax
{
    internal sealed class SyntaxTokenParser : ISyntaxTokenParser
    {
        private const char OPENING_BRACE_CHAR = '(';
        private const char CLOSING_BRACE_CHAR = ')';

        private readonly ISyntaxTokenFactory _syntaxTokenFactory;

        public SyntaxTokenParser(ISyntaxTokenFactory syntaxTokenFactory)
        {
            _syntaxTokenFactory = syntaxTokenFactory;
        }

        public IList<SyntaxTokenOld> ParseSyntaxTokens(string expression)
        {
            expression = expression?.Trim();

            if (string.IsNullOrEmpty(expression))
            {
                throw new ArgumentNullException(nameof(expression));
            }

            var list = new List<SyntaxTokenOld>(16);

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
                        var unaryFunctionNode = _syntaxTokenFactory.ParseToken(
                            expressionBeforeBraces.Trim().TrimEnd(OPENING_BRACE_CHAR));

                        unaryFunctionNode.SubTokens = bracesNode.SubTokens;

                        list.Add(unaryFunctionNode);
                    }

                    continue;
                }

                if (_syntaxTokenFactory.KnownBinaryFunctions.Contains(currentChar))
                {
                    // When false it means we just parsed expression in braces.
                    // Before it there is no value - only function.
                    if (lastSignificantCharIndex < i)
                    {
                        string valueToParse = expression[lastSignificantCharIndex..i].Trim();

                        list.Add(_syntaxTokenFactory.ParseToken(valueToParse));
                    }

                    list.Add(_syntaxTokenFactory.ParseToken(currentChar.ToString()));

                    lastSignificantCharIndex = i + 1;

                    continue;
                }
            }

            if (lastSignificantCharIndex < i)
            {
                string valueToParse = expression[lastSignificantCharIndex..i].Trim();

                list.Add(_syntaxTokenFactory.ParseToken(valueToParse));
            }

            return list;
        }

        private (int lastIndex, SyntaxTokenOld node) ExtractBraces(string expression, int startIndex)
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
                    var subNodes = ParseSyntaxTokens(expressionInBraces);

                    var braceNode = new SyntaxTokenOld
                    {
                        SubTokens = subNodes.ToList()
                    };

                    return (i, braceNode);
                }
            }

            throw new InvalidOperationException($"Did not find closing brace from {startIndex}: {expression}");
        }
    }
}
