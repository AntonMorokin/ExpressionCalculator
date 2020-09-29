namespace Calculation.Model
{
    /// <summary>
    /// Simple number.
    /// </summary>
    public class Number : IHasValue
    {
        private readonly decimal _value;

        /// <summary>
        /// Initialize new instance of <see cref="Number"/>.
        /// </summary>
        /// <param name="value"></param>
        public Number(decimal value)
        {
            _value = value;
        }

        /// <inheritdoc />
        public decimal GetValue() => _value;
    }
}
