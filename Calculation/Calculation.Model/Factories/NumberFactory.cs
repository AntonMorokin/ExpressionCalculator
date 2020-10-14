namespace Calculation.Model.Factories
{
    /// <summary>
    /// Implementation of <see cref="INumberFactory"/>/
    /// </summary>
    internal class NumberFactory : INumberFactory
    {
        /// <inheritdoc />
        public IHasValue CreateNumber(decimal value)
        {
            return new Number(value);
        }
    }
}
