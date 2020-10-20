namespace Common
{
    /// <summary>
    /// Result with optional value.
    /// </summary>
    /// <typeparam name="T">Type of value.</typeparam>
    public sealed class OptionalResult<T>
    {
        /// <summary>
        /// Indicates that value exists.
        /// </summary>
        public bool HasValue { get; }

        /// <summary>
        /// Result value.
        /// </summary>
        public T Value { get; }

        /// <summary>
        /// Initialize new object of <see cref="OptionalResult{T}"/>.
        /// </summary>
        /// <param name="isCalculated">Indicates that value exists.</param>
        /// <param name="value">Result value.</param>
        private OptionalResult(bool isCalculated, T value)
        {
            HasValue = isCalculated;
            Value = value;
        }

        /// <summary>
        /// Creates <see cref="OptionalResult{T}"/> without value.
        /// </summary>
        /// <returns><see cref="OptionalResult{T}"/> without value and <see cref="HasValue"/> = <c>false</c>.</returns>
        public static OptionalResult<T> CreateEmpty()
        {
            return new OptionalResult<T>(false, default);
        }

        /// <summary>
        /// Creates <see cref="OptionalResult{T}"/> with value.
        /// </summary>
        /// <param name="value">Result value.</param>
        /// <returns><see cref="OptionalResult{T}"/> without value and <see cref="HasValue"/> = <c>true</c>.</returns>
        public static OptionalResult<T> CreateWithResult(T value)
        {
            return new OptionalResult<T>(true, value);
        }
    }
}
