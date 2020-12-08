using Processing.Syntax.Model;
using System;
using System.Collections.Generic;

namespace Processing.Syntax
{
    internal sealed class SyntaxAnalyzer : ISyntaxAnalyzer
    {
        public void AnalyzeCorrectness(IList<SyntaxToken> syntaxTokens)
        {
            if (syntaxTokens.Count == 0)
            {
                throw new InvalidOperationException("Expression cannot be empty.");
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
                    throw new InvalidOperationException("Binary function cannot be single in the expression.");
                }
                case SyntaxTokenTypes.UnaryFunction:
                {
                    throw new InvalidOperationException("Unary function cannot be single in the expression.");
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

        private static void ValidateFirstToken(IList<SyntaxToken> syntaxTokens)
        {
            var firstToken = syntaxTokens[0];
            if (firstToken.Type == SyntaxTokenTypes.BinaryFunction)
            {
                throw new InvalidOperationException("Binary function cannot be first in the expression.");
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
                            throw new InvalidOperationException("There must be binary function after number.");
                        }

                        break;
                    }
                    case SyntaxTokenTypes.UnaryFunction:
                    {
                        if (nextToken.Type != SyntaxTokenTypes.Braces)
                        {
                            throw new InvalidOperationException("There must be braces with value after unary function.");
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
                            throw new InvalidOperationException("There must be binary function after braces.");
                        }

                        break;
                    }
                    default:
                        throw new NotSupportedException($"Unknown type of syntax token: {currentToken.Type}.");
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
                    throw new InvalidOperationException("Unary function cannot be last in the expression.");
                }
                case SyntaxTokenTypes.BinaryFunction:
                {
                    throw new InvalidOperationException("Binary function cannot be last in the expression.");
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
