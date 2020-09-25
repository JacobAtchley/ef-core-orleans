using Orleans;
using Test.Platform.Wms.Orleans.Grains.Client.Interfaces;

namespace Test.Platform.Wms.Orleans.Grains.Client.Implementations
{
    public class ClusterClientFactory : IClusterClientFactory
    {
        private readonly IClientBuilderConfigurator _configurator;

        public ClusterClientFactory(IClientBuilderConfigurator configurator)
        {
            _configurator = configurator;
        }

        public IClusterClient Create(string clusterId, string serviceName, bool isDev)
        {
            var builder = _configurator.Configure(null, clusterId, serviceName, isDev);

            var client = builder.Build();

            return client;
        }
    }
}