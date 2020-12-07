using Processing.Syntax.Model;
using System.Collections.Generic;

namespace Processing.Syntax
{
    internal sealed class SyntaxProcessor : ISyntaxProcessor
    {
        private readonly IExpressionParser _expressionParser;
        private readonly ISyntaxTokenParser _tokenParser;
        private readonly ISyntaxAnalyzer _syntaxAnalyzer;

        public SyntaxProcessor(IExpressionParser expressionParser,
            ISyntaxTokenParser tokenParser,
            ISyntaxAnalyzer syntaxAnalyzer)
        {
            _expressionParser = expressionParser;
            _tokenParser = tokenParser;
            _syntaxAnalyzer = syntaxAnalyzer;
        }

        public IList<SyntaxToken> CreateSyntaxModel(string expression)
        {
            var tokensRaw = _expressionParser.SplitExpressionToTokens(expression);
            var syntaxTokens = _tokenParser.ParseSyntaxTokens(tokensRaw);

            _syntaxAnalyzer.AnalyzeCorrectness(syntaxTokens);

            BindBracesToUnaryFunctions(syntaxTokens);

            return syntaxTokens;
        }

        private void BindBracesToUnaryFunctions(IList<SyntaxToken> syntaxTokens)
        {
            for (int i = 0; i < syntaxTokens.Count - 1; i++)
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
