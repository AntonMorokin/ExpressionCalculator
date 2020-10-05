namespace Calculation.Model.Factories
{
    /// <summary>
    /// Factory of numbers.
    /// </summary>
    public interface INumberFactory
    {
        /// <summary>
        /// Creates number by its value.
        /// </summary>
        /// <param name="value">Number value.</param>
        /// <returns>Number.</returns>
        IHasValue CreateNumber(decimal value);
    }
}
