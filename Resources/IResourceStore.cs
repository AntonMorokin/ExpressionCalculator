namespace Resources
{
    /// <summary>
    /// Store of resources.
    /// </summary>
    public interface IResourceStore
    {
        /// <summary>
        /// Returns exception message by its key from resources.
        /// </summary>
        /// <param name="resourceName">Key of exception message in resources.</param>
        /// <returns>Exception message.</returns>
        string GetExceptionMessage(string resourceName);
    }
}
