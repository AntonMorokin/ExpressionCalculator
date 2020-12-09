using Calculation.Model.Functions.Binary;
using Calculation.Model.Functions.Unary;
using Resources;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Calculation.Model.Factories
{
    /// <summary>
    /// Factory operational model objects.
    /// </summary>
    public sealed class CalculationObjectFactory : ICalculationObjectFactory
    {
        private readonly INumberFactory _numberFactory;
        private readonly IResourceStore _resourceStore;

        private readonly IDictionary<string, Func<IHasValue>> _knownUnaryFunctions;
        private readonly IDictionary<string, Func<IHasValue>> _knownBinaryFunctions;

        /// <summary>
        /// Initializes new instance of <see cref="CalculationObjectFactory"/>.
        /// </summary>
        /// <param name="numberFactory">Factory of numbers.</param>
        /// <param name="resourceStore">Store of resources.</param>
        public CalculationObjectFactory(INumberFactory numberFactory, IResourceStore resourceStore)
        {
            _numberFactory = numberFactory;
            _resourceStore = resourceStore;

            _knownUnaryFunctions = new Dictionary<string, Func<IHasValue>>
            {
                { "log2", () => new Log2(_resourceStore, _numberFactory) }
            };

            _knownBinaryFunctions = new Dictionary<string, Func<IHasValue>>
            {
                { "+", () => new Plus(_resourceStore, _numberFactory) },
                { "-", () => new Minus(_resourceStore, _numberFactory) },
                { "*", () => new Multiply(_resourceStore, _numberFactory) },
                { "/", () => new Divide(_resourceStore, _numberFactory) }
            };
        }

        /// <inheritdoc/>
        public IHasValue Create(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException(nameof(value));
            }

            if (decimal.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal number))
            {
                return _numberFactory.CreateNumber(number);
            }

            if (_knownBinaryFunctions.TryGetValue(value, out Func<IHasValue> binaryFunctionFactory))
            {
                return binaryFunctionFactory();
            }

            if (_knownUnaryFunctions.TryGetValue(value, out Func<IHasValue> unaryFunctionFactory))
            {
                return unaryFunctionFactory();
            }

            throw new NotSupportedException(
                _resourceStore.GetExceptionMessage("UnknownCalculationObject", value));
        }
    }
}
