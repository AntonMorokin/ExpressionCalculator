using Resources;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace Calculation.Model
{
    /// <summary>
    /// Abstract realization of <see cref="IFunction"/>.
    /// </summary>
    public abstract class Function : IFunction
    {
        /// <summary>
        /// Store of resources.
        /// </summary>
        protected readonly IResourceStore ResourceStore;

        /// <summary>
        /// Arguments of function.
        /// </summary>
        protected IReadOnlyCollection<decimal> Arguments;

        /// <inheritdoc />
        public abstract int NumberOfArguments { get; }

        /// <summary>
        /// Initialize new child instance of <see cref="Function"/>.
        /// </summary>
        /// <param name="resourceStore">Store of resources.</param>
        protected Function(IResourceStore resourceStore)
        {
            ResourceStore = resourceStore;
        }

        /// <inheritdoc />
        public void SetArguments(params decimal[] arguments)
        {
            if (arguments.Length != NumberOfArguments)
            {
                throw new ArgumentOutOfRangeException(
                    ResourceStore.GetExceptionMessage("NumberOfArgumentsMismatchExceptionMessage"));
            }

            Arguments = ImmutableList.CreateRange(arguments);
        }

        /// <summary>
        /// Calculate the value of function depended on current arguments.
        /// </summary>
        /// <returns>The value of function depended on current arguments.</returns>
        protected abstract decimal Calculate();

        /// <inheritdoc />
        public decimal GetValue()
        {
            return Calculate();
        }
    }
}
