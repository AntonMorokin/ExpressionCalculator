using Autofac;
using Autofac.Configuration;
using Microsoft.Extensions.Configuration;
using System;

namespace Common
{
    /// <summary>
    /// Type factory.
    /// </summary>
    public static class TypeFactory
    {
        private static readonly Lazy<IContainer> __container
            = new Lazy<IContainer>(InitializeContainer, System.Threading.LazyThreadSafetyMode.ExecutionAndPublication);

        /// <summary>
        /// Resolves instance of type <typeparamref name="T" /> from IoC container.
        /// </summary>
        /// <typeparam name="T">Type of instance.</typeparam>
        /// <returns>Instance of type <typeparamref name="T"/></returns>
        public static T Get<T>()
        {
            return __container.Value.Resolve<T>();
        }

        private static IContainer InitializeContainer()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("autofac.json")
                .Build();

            var module = new ConfigurationModule(config);
            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterModule(module);

            return containerBuilder.Build();
        }
    }
}
