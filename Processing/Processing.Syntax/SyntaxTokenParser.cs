using Processing.Syntax.Factories;
using Processing.Syntax.Model;
using System;
using System.Collections.Generic;

namespace Processing.Syntax
{
    internal sealed class SyntaxTokenParser : ISyntaxTokenParser
    {
        private const char OPENING_BRACE_CHAR = '(';
        private const char CLOSING_BRACE_CHAR = ')';

        private readonly IExpressionParser _expressionParser;
        private readonly ISyntaxTokenFactory _syntaxTokenFactory;

        public SyntaxTokenParser(IExpressionParser expressionParser, ISyntaxTokenFactory syntaxTokenFactory)
        {
            _syntaxTokenFactory = syntaxTokenFactory;
            _expressionParser = expressionParser;
        }

        public IList<SyntaxToken> ParseSyntaxTokens(IDictionary<int, string> syntaxTokensRaw)
        {
            if (syntaxTokensRaw == null)
            {
                throw new ArgumentNullException(nameof(syntaxTokensRaw));
            }

            return ConvertToSyntaxTokens(syntaxTokensRaw.Values);
        }

        private IList<SyntaxToken> ConvertToSyntaxTokens(IEnumerable<string> orderedTokens)
        {
            var tokens = new List<SyntaxToken>();

            foreach (string tokenRaw in orderedTokens)
            {
                if (tokenRaw.StartsWith(OPENING_BRACE_CHAR))
                {
                    string expressionToParse = tokenRaw.Trim(new char[] { OPENING_BRACE_CHAR, CLOSING_BRACE_CHAR });
                    var childTokensRaw = _expressionParser.SplitExpressionToTokens(expressionToParse);

                    var childTokens = ParseSyntaxTokens(childTokensRaw);

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
    }
}
