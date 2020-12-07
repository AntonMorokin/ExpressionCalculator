using Processing.Syntax.Factories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Processing.Syntax
{
    internal sealed class ExpressionParser : IExpressionParser
    {
        private const char OPENING_BRACE_CHAR = '(';
        private const char CLOSING_BRACE_CHAR = ')';

        private readonly ISyntaxTokenFactory _syntaxTokenFactory;

        public ExpressionParser(ISyntaxTokenFactory syntaxTokenFactory)
        {
            _syntaxTokenFactory = syntaxTokenFactory;
        }

        public IDictionary<int, string> SplitExpressionToTokens(string expression)
        {
            expression = expression?.Trim();

            if (string.IsNullOrEmpty(expression))
            {
                throw new ArgumentNullException(nameof(expression));
            }

            var syntaxTokensRaw = new Dictionary<int, string>();

            SplitExpressionToTokens(expression, 0, syntaxTokensRaw);

            return syntaxTokensRaw
                .OrderBy(t => t.Key)
                .ToDictionary(p => p.Key, p => p.Value);
        }

        private void SplitExpressionToTokens(string expression, int startIndex, IDictionary<int, string> syntaxTokensRaw)
        {
            bool bracesWereFound = false;
            char currentChar;

            for (int i = startIndex; i < expression.Length; i++)
            {
                currentChar = expression[i];

                if (currentChar == OPENING_BRACE_CHAR)
                {
                    bracesWereFound = true;
                    int lastIndexInclusive = FindBracesBoundary(expression, i) + 1;

                    string before = expression[startIndex..i];
                    SplitxpressionByBinaryFunctions(before, startIndex, syntaxTokensRaw);

                    string braces = expression[i..lastIndexInclusive];
                    AddDetectingWhiteSpaces(braces, syntaxTokensRaw, i);

                    if (expression.Length - lastIndexInclusive > 0)
                    {
                        SplitExpressionToTokens(expression, lastIndexInclusive, syntaxTokensRaw);
                    }

                    // We already split the last part
                    // OR there is nothing to parse.
                    return;
                }
            }

            if (!bracesWereFound)
            {
                string last = expression[startIndex..];
                SplitxpressionByBinaryFunctions(last, startIndex, syntaxTokensRaw);
            }
        }

        private int FindBracesBoundary(string expression, int startIndex)
        {
            int nestingLevel = 0;
            char currentChar;

            for (int i = startIndex + 1; i < expression.Length; i++)
            {
                currentChar = expression[i];

                if (currentChar == OPENING_BRACE_CHAR)
                {
                    nestingLevel++;
                }

                if (currentChar == CLOSING_BRACE_CHAR)
                {
                    if (nestingLevel == 0)
                    {
                        return i;
                    }

                    nestingLevel--;
                }
            }

            throw new FormatException($"Cannot find closing brace char starting from {startIndex} index in expression {expression}.");
        }

        private void SplitxpressionByBinaryFunctions(string expression, int indexesOffset, IDictionary<int, string> syntaxTokensRaw)
        {
            int startIndex = 0;

            for (int i = 1; i <= expression.Length; i++)
            {
                string part = expression[startIndex..i];

                if (ContainsBinaryFunction(part, out string bf))
                {
                    string before = part[0..(part.Length - bf.Length)];

                    if (!string.IsNullOrWhiteSpace(before))
                    {
                        AddDetectingWhiteSpaces(before, syntaxTokensRaw, indexesOffset + startIndex);
                    }

                    AddDetectingWhiteSpaces(bf, syntaxTokensRaw, indexesOffset + startIndex + before.Length);

                    startIndex += part.Length;
                }
            }

            string last = expression[startIndex..];
            if (!string.IsNullOrWhiteSpace(last))
            {
                AddDetectingWhiteSpaces(last, syntaxTokensRaw, indexesOffset + startIndex);
            }
        }

        private bool ContainsBinaryFunction(string expression, out string foundBinaryFunction)
        {
            // '1.234 +'
            // '12.34+'
            // 12.43plus

            foreach (string bf in _syntaxTokenFactory.KnownBinaryFunctions)
            {
                if (expression.EndsWith(bf, StringComparison.OrdinalIgnoreCase))
                {
                    foundBinaryFunction = bf;
                    return true;
                }
            }

            foundBinaryFunction = null;
            return false;
        }

        private void AddDetectingWhiteSpaces(string syntaxTokenRaw, IDictionary<int, string> syntaxTokensRaw, int baseIndex)
        {
            var handledValue = syntaxTokenRaw.TrimStart();
            int offset = syntaxTokenRaw.Length - handledValue.Length;

            syntaxTokensRaw.Add(baseIndex + offset, handledValue.TrimEnd());
        }
    }
}
