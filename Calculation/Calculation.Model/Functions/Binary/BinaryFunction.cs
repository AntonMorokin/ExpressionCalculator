using Resources;

namespace Calculation.Model.Functions.Binary
{
    /// <summary>
    /// Binary function.
    /// </summary>
    public abstract class BinaryFunction : Function
    {
        /// <inheritdoc />
        public override int NumberOfArguments => 2;

        /// <summary>
        /// Initializes new instance of <see cref="BinaryFunction"/>.
        /// </summary>
        /// <param name="resourceStore">Store of resources.</param>
        protected BinaryFunction(IResourceStore resourceStore)
            : base(resourceStore)
        {
        }

        /// <inheritdoc />
        protected override IHasValue Calculate()
        {
            var firstArg = Arguments[0];
            var secondArg = Arguments[1];

            return Calculate(firstArg, secondArg);
        }

        /// <summary>
        /// Calculates value of binary function using two arguments.
        /// </summary>
        /// <param name="firstArg">First arguments.</param>
        /// <param name="secondArg">Second arguments.</param>
        /// <returns>Value of binary function.</returns>
        protected abstract IHasValue Calculate(IHasValue firstArg, IHasValue secondArg);
    }
}
