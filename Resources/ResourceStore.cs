using System;
using System.Resources;

namespace Resources
{
    /// <summary>
    /// Realization of <see cref="IResourceStore"/>.
    /// </summary>
    public class ResourceStore : IResourceStore
    {
        private readonly ResourceManager _resourceManager;

        public ResourceStore()
        {
            _resourceManager = new ResourceManager("ExceptionMessages", typeof(ResourceStore).Assembly);
        }

        /// <inheritdoc />
        public string GetExceptionMessage(string resourceName)
        {
            return _resourceManager.GetString(resourceName);
        }
    }
}
