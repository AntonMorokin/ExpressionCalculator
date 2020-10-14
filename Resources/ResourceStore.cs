namespace Resources
{
    /// <summary>
    /// Implementation of <see cref="IResourceStore"/>.
    /// </summary>
    internal class ResourceStore : IResourceStore
    {
        /// <inheritdoc />
        public string GetExceptionMessage(string resourceName)
        {
            return ExceptionMessages.ResourceManager.GetString(resourceName);
        }
    }
}
