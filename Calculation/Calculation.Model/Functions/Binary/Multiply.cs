using Calculation.Model.Factories;
using Resources;

namespace Calculation.Model.Functions.Binary
{
    /// <summary>
    /// "Multiply" binary function (a * b).
    /// </summary>
    public class Multiply : BinaryFunction
    {
        private readonly INumberFactory _numberFactory;

        /// <summary>
        /// Initializes new instance of <see cref="Multiply"/>.
        /// </summary>
        /// <param name="resourceStore">Store of resources.</param>
        /// <param name="numberFactory">Number factory.</param>
        public Multiply(IResourceStore resourceStore, INumberFactory numberFactory)
            : base(resourceStore)
        {
            _numberFactory = numberFactory;
        }

        /// <inheritdoc />
        protected override IHasValue Calculate(IHasValue firstArg, IHasValue secondArg)
        {
            decimal firstValue = firstArg.GetValue();
            decimal secondValue = secondArg.GetValue();

            return _numberFactory.CreateNumber(firstValue * secondValue);
        }
    }
}
