using Processing.Syntax.Model;
using Resources;
using System;
using System.Collections.Generic;

namespace Processing.Syntax
{
    internal sealed class SyntaxAnalyzer : ISyntaxAnalyzer
    {
        private readonly IResourceStore _resourceStore;

        public SyntaxAnalyzer(IResourceStore resourceStore)
        {
            _resourceStore = resourceStore;
        }

        public void AnalyzeCorrectness(IList<SyntaxToken> syntaxTokens)
        {
            if (syntaxTokens.Count == 0)
            {
                throw new InvalidOperationException(_resourceStore.GetExceptionMessage("ExpressionCannotBeEmpty"));
            }

            if (syntaxTokens.Count < 2)
            {
                ValidateSingleToken(syntaxTokens[0]);
                return;
            }

            ValidateFirstToken(syntaxTokens);
            ValidateMiddleTokens(syntaxTokens);
            ValidateLastToken(syntaxTokens);
        }

        private void ValidateSingleToken(SyntaxToken singleToken)
        {
            switch (singleToken.Type)
            {
                case SyntaxTokenTypes.BinaryFunction:
                {
                    throw new InvalidOperationException(
                        _resourceStore.GetExceptionMessage("FunctionCannotBeSingle", "Binary"));
                }
                case SyntaxTokenTypes.UnaryFunction:
                {
                    throw new InvalidOperationException(
                        _resourceStore.GetExceptionMessage("FunctionCannotBeSingle", "Unary"));
                }
                case SyntaxTokenTypes.Braces:
                {
                    ValidateBraces(singleToken);
                    return;
                }
                default:
                {
                    return;
                }
            }
        }

        private void ValidateBraces(SyntaxToken syntaxToken)
        {
            var braces = (BracesSyntaxToken)syntaxToken;
            AnalyzeCorrectness(braces.ChildTokens);
        }

        private void ValidateFirstToken(IList<SyntaxToken> syntaxTokens)
        {
            var firstToken = syntaxTokens[0];
            if (firstToken.Type == SyntaxTokenTypes.BinaryFunction)
            {
                throw new InvalidOperationException(
                    _resourceStore.GetExceptionMessage("BinaryFunctionCannotBeFirst"));
            }
        }

        private void ValidateMiddleTokens(IList<SyntaxToken> syntaxTokens)
        {
            SyntaxToken currentToken;
            SyntaxToken nextToken;

            for (int i = 0; i < syntaxTokens.Count - 1; i++)
            {
                currentToken = syntaxTokens[i];
                nextToken = syntaxTokens[i + 1];

                switch (currentToken.Type)
                {
                    case SyntaxTokenTypes.Number:
                    {
                        if (nextToken.Type != SyntaxTokenTypes.BinaryFunction)
                        {
                            string message = _resourceStore.GetExceptionMessage(
                                "InvalidSyntaxTokensOrder", "binary function", "number");

                            throw new InvalidOperationException(message);
                        }

                        break;
                    }
                    case SyntaxTokenTypes.UnaryFunction:
                    {
                        if (nextToken.Type != SyntaxTokenTypes.Braces)
                        {
                            var message = _resourceStore.GetExceptionMessage(
                                "InvalidSyntaxTokensOrder", "braces with value", "unary function");

                            throw new InvalidOperationException(message);
                        }

                        break;
                    }
                    case SyntaxTokenTypes.BinaryFunction:
                        break;
                    case SyntaxTokenTypes.Braces:
                    {
                        ValidateBraces(currentToken);

                        if (nextToken.Type != SyntaxTokenTypes.BinaryFunction)
                        {
                            string message = _resourceStore.GetExceptionMessage(
                                "InvalidSyntaxTokensOrder", "binary function", "braces");

                            throw new InvalidOperationException(message);
                        }

                        break;
                    }
                    default:
                        throw new NotSupportedException(
                            _resourceStore.GetExceptionMessage("UnknownSyntaxTokenType", currentToken.Type));
                }
            }
        }

        private void ValidateLastToken(IList<SyntaxToken> syntaxTokens)
        {
            var lastToken = syntaxTokens[syntaxTokens.Count - 1];

            switch (lastToken.Type)
            {
                case SyntaxTokenTypes.UnaryFunction:
                {
                    throw new InvalidOperationException(
                        _resourceStore.GetExceptionMessage("FunctionCannotBeLast", "Unary"));
                }
                case SyntaxTokenTypes.BinaryFunction:
                {
                    throw new InvalidOperationException(
                        _resourceStore.GetExceptionMessage("FunctionCannotBeLast", "Binary"));
                }
                case SyntaxTokenTypes.Braces:
                {
                    ValidateBraces(lastToken);
                    return;
                }
                default:
                    return;
            }
        }
    }
}
