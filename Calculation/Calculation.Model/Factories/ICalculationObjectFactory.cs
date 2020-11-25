namespace Calculation.Model.Factories
{
    /// <summary>
    /// Factory operational model objects.
    /// </summary>
    public interface ICalculationObjectFactory
    {
        /// <summary>
        /// Create operational object by its presentation.
        /// </summary>
        /// <param name="value">Operational object presentation.</param>
        /// <returns>Operational (calculation) object.</returns>
        IHasValue Create(string value);
    }
}
