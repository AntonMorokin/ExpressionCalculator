using Processing.Semantics.Model;
using Processing.Syntax.Model;
using System;
using System.Collections.Generic;

namespace Processing.Semantics
{
    internal sealed class SemanticsProcessor : ISemanticsProcessor
    {
        private readonly ISemanticsConverter _semanticsConverter;
        private readonly ISemanticsBuilder _semanticsBuilder;

        public SemanticsProcessor(ISemanticsConverter semanticsConverter, ISemanticsBuilder semanticsBuilder)
        {
            _semanticsConverter = semanticsConverter;
            _semanticsBuilder = semanticsBuilder;
        }

        public SemanticNode CreateSemanticModel(IList<SyntaxToken> syntaxTokens)
        {
            if (syntaxTokens == null)
            {
                throw new ArgumentNullException();
            }

            var semanticNodes = _semanticsConverter.ConvertSyntaxToSemantics(syntaxTokens);
            return _semanticsBuilder.BuildSemanticTree(semanticNodes);
        }
    }
}
