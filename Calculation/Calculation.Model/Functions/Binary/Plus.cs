﻿using Calculation.Model.Factories;
using Resources;

namespace Calculation.Model.Functions.Binary
{
    /// <summary>
    /// "Plus" binary function (a + b).
    /// </summary>
    public sealed class Plus : BinaryFunction
    {
        private readonly INumberFactory _numberFactory;

        /// <inheritdoc />
        public override byte Priority => 0;

        /// <summary>
        /// Initializes new instance of <see cref="Plus"/>.
        /// </summary>
        /// <param name="resourceStore">Store of resources.</param>
        /// <param name="numberFactory">Number factory.</param>
        public Plus(IResourceStore resourceStore, INumberFactory numberFactory)
            : base(resourceStore)
        {
            _numberFactory = numberFactory;
        }

        /// <inheritdoc />
        protected override IHasValue Calculate(IHasValue firstArg, IHasValue secondArg)
        {
            decimal firstValue = firstArg.GetValue();
            decimal secondValue = secondArg.GetValue();

            return _numberFactory.CreateNumber(firstValue + secondValue);
        }
    }
}
