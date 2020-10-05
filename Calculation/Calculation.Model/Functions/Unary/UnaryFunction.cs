using Resources;

namespace Calculation.Model.Functions.Unary
{
    /// <summary>
    /// Unary function.
    /// </summary>
    public abstract class UnaryFunction : Function
    {
        /// <inheritdoc />
        public override int NumberOfArguments => 1;

        /// <summary>
        /// Initializes new instance of <see cref="UnaryFunction"/>.
        /// </summary>
        /// <param name="resourceStore">Store of resources.</param>
        protected UnaryFunction(IResourceStore resourceStore)
            : base(resourceStore)
        {
        }

        /// <inheritdoc />
        protected override IHasValue Calculate()
        {
            var arg = Arguments[0];
            return Calculate(arg);
        }

        /// <summary>
        /// Calculates value of unary function using single arguments.
        /// </summary>
        /// <param name="argument">Function argument.</param>
        /// <returns>Value of unary function.</returns>
        protected abstract IHasValue Calculate(IHasValue argument);
    }
}
