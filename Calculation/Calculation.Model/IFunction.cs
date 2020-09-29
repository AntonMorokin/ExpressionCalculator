namespace Calculation.Model
{
    /// <summary>
    /// Function of several arguments.
    /// </summary>
    interface IFunction : IHasValue
    {
        /// <summary>
        /// Number of arguments that function uses.
        /// </summary>
        int NumberOfArguments { get; }

        /// <summary>
        /// Set arguments of function.
        /// </summary>
        /// <param name="arguments">Function arguments.</param>
        void SetArguments(params decimal[] arguments);
    }
}
