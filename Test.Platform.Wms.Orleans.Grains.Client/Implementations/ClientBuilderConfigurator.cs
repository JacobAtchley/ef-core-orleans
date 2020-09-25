using Orleans;
using Orleans.Configuration;
using Test.Platform.Wms.Orleans.Grains.Client.Interfaces;

namespace Test.Platform.Wms.Orleans.Grains.Client.Implementations
{
    public class ClientBuilderConfigurator : IClientBuilderConfigurator
    {
        public IClientBuilder Configure(IClientBuilder builder, string clusterId, string serviceId, bool isDevelopment)
        {
            if (builder == null)
            {
                builder = new ClientBuilder();
            }
            
            builder.Configure<ClusterOptions>(opt =>
            {
                opt.ClusterId = clusterId;
                opt.ServiceId = serviceId;
            });

            if (isDevelopment)
            {
                builder = builder.UseLocalhostClustering();
            }

            return builder;
        }
    }
}