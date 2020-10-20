using Calculation.Model.Factories;
using Common;
using Resources;
using System;

namespace Calculation.Model.Functions.Binary
{
    /// <summary>
    /// "Divide" binary function (a / b).
    /// </summary>
    public sealed class Divide : BinaryFunction
    {
        private readonly INumberFactory _numberFactory;

        /// <inheritdoc />
        public override byte Priority => 2;

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
                throw new ArgumentOutOfRangeException(nameof(secondArg),
                    ResourceStore.GetExceptionMessage("DenominatorIsZero"));
            }

            return _numberFactory.CreateNumber(firstValue / secondValue);
        }

        /// <inheritdoc />
        protected override OptionalResult<string> Render()
        {
            return Arguments.Count switch
            {
                0 => OptionalResult<string>.CreateWithResult("/"),
                1 => OptionalResult<string>.CreateWithResult($"{Arguments[0]} /"),
                2 => OptionalResult<string>.CreateWithResult($"{Arguments[0]} / {Arguments[1]}"),
                _ => OptionalResult<string>.CreateEmpty()
            };
        }
    }
}
