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

        public IList<SyntaxToken> ParseSyntaxTokens(string expression)
        {
            expression = expression?.Trim();

            if (string.IsNullOrEmpty(expression))
            {
                throw new ArgumentNullException(nameof(expression));
            }

            var syntaxTokensRaw = new Dictionary<int, string>();

            SplitExpressionToTokens(expression, 0, syntaxTokensRaw);

            var orderedTokens = syntaxTokensRaw
                .OrderBy(t => t.Key)
                .Select(t => t.Value);

            var syntaxTokens = ConvertToSyntaxTokens(orderedTokens);

            Analyze(syntaxTokens);

            return syntaxTokens;
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

        private IList<SyntaxToken> ConvertToSyntaxTokens(IEnumerable<string> orderedTokens)
        {
            var tokens = new List<SyntaxToken>();

            foreach (string tokenRaw in orderedTokens)
            {
                if (tokenRaw.StartsWith(OPENING_BRACE_CHAR))
                {
                    var expressionToParse = tokenRaw.Trim(new char[] { OPENING_BRACE_CHAR, CLOSING_BRACE_CHAR });
                    var childTokens = ParseSyntaxTokens(expressionToParse);

                    var braces = new BracesSyntaxToken(childTokens);

                    tokens.Add(braces);
                }
                else
                {
                    tokens.Add(_syntaxTokenFactory.ParseToken(tokenRaw));
                }
            }

            return tokens;
        }

        private void Analyze(IList<SyntaxToken> syntaxTokens)
        {
            for (int i = 0; i < syntaxTokens.Count; i++)
            {
                if (syntaxTokens[i].Type == SyntaxTokenTypes.UnaryFunction)
                {
                    var uf = (UnaryFunctionSyntaxToken)syntaxTokens[i];
                    var braces = (BracesSyntaxToken)syntaxTokens[i + 1];

                    uf.Braces = braces;

                    syntaxTokens.Remove(syntaxTokens[i + 1]);
                }
            }
        }
    }
}
