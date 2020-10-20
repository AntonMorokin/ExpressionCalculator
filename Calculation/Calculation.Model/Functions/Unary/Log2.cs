using Calculation.Model.Factories;
using Common;
using Resources;
using System;

namespace Calculation.Model.Functions.Unary
{
    /// <summary>
    /// "Log by 2" unary function (log2(x)).
    /// </summary>
    public sealed class Log2 : UnaryFunction
    {
        private readonly INumberFactory _numberFactory;

        /// <inheritdoc />
        public override byte Priority => 8;

        /// <summary>
        /// Initializes new instance of <see cref="Log2"/>.
        /// </summary>
        /// <param name="resourceStore">Store of resources.</param>
        /// <param name="numberFactory">Number factory.</param>
        public Log2(IResourceStore resourceStore, INumberFactory numberFactory)
            : base(resourceStore)
        {
            _numberFactory = numberFactory;
        }

        /// <inheritdoc />
        protected override IHasValue Calculate(IHasValue argument)
        {
            decimal value = (decimal)Math.Log2((double)argument.GetValue());
            return _numberFactory.CreateNumber(value);
        }

        /// <inheritdoc />
        protected override OptionalResult<string> Render()
        {
            return Arguments.Count switch
            {
                0 => OptionalResult<string>.CreateWithResult("log2"),
                1 => OptionalResult<string>.CreateWithResult($"log2({Arguments[0]})"),
                _ => OptionalResult<string>.CreateEmpty()
            };
        }
    }
}
