namespace Calculation.Model
{
    /// <summary>
    /// Types that have value.
    /// </summary>
    public interface IHasValue
    {
        /// <summary>
        /// Returns value.
        /// </summary>
        /// <returns>Value.</returns>
        decimal GetValue();
    }
}
