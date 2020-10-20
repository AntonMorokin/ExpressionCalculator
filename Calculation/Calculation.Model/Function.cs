using Common;
using Resources;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace Calculation.Model
{
    /// <summary>
    /// Abstract implementation of <see cref="IFunction"/>.
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
        protected IReadOnlyList<IHasValue> Arguments;

        /// <inheritdoc />
        public abstract byte Priority { get; }

        /// <inheritdoc />
        public abstract int NumberOfArguments { get; }

        /// <summary>
        /// Initialize new child instance of <see cref="Function"/>.
        /// </summary>
        /// <param name="resourceStore">Store of resources.</param>
        protected Function(IResourceStore resourceStore)
        {
            ResourceStore = resourceStore;
            Arguments = ImmutableList<IHasValue>.Empty;
        }

        /// <inheritdoc />
        public void SetArguments(params IHasValue[] arguments)
        {
            if (arguments.Length != NumberOfArguments)
            {
                throw new ArgumentOutOfRangeException(
                    ResourceStore.GetExceptionMessage("NumberOfArgumentsMismatch"));
            }

            Arguments = ImmutableList.CreateRange(arguments);
        }

        /// <summary>
        /// Calculate the value of function depended on current arguments.
        /// </summary>
        /// <returns>The value of function depended on current arguments.</returns>
        protected abstract IHasValue Calculate();

        /// <inheritdoc />
        public decimal GetValue()
        {
            return Calculate().GetValue();
        }

        /// <summary>
        /// Creates string representation of this object.
        /// </summary>
        /// <returns>String representation of this object.</returns>
        protected abstract OptionalResult<string> Render();

        /// <inheritdoc />
        public override string ToString()
        {
            var result = Render();

            if (result.HasValue)
            {
                return result.Value;
            }

            throw new InvalidOperationException(ResourceStore.GetExceptionMessage("CannotRenderFunction"));
        }
    }
}
