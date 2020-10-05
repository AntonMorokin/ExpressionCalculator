using Calculation.Model.Factories;
using Resources;
using System;

namespace Calculation.Model.Functions.Binary
{
    /// <summary>
    /// "Divide" binary function (a / b).
    /// </summary>
    public class Divide : BinaryFunction
    {
        private readonly INumberFactory _numberFactory;

        /// <summary>
        /// Initializes new instance of <see cref="Divide"/>.
        /// </summary>
        /// <param name="resourceStore">Store of resources.</param>
        /// <param name="numberFactory">Number factory.</param>
        public Divide(IResourceStore resourceStore, INumberFactory numberFactory)
            : base(resourceStore)
        {
            _numberFactory = numberFactory;
        }

        /// <inheritdoc />
        protected override IHasValue Calculate(IHasValue firstArg, IHasValue secondArg)
        {
            decimal firstValue = firstArg.GetValue();
            decimal secondValue = secondArg.GetValue();

            if (secondValue == 0)
            {
                throw new ArgumentOutOfRangeException(nameof(secondArg), "Denominator must not be zero.");
            }

            return _numberFactory.CreateNumber(firstValue / secondValue);
        }
    }
}
