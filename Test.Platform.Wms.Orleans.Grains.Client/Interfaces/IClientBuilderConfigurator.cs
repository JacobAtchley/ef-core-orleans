using Orleans;

namespace Test.Platform.Wms.Orleans.Grains.Client.Interfaces
{
    public interface IClientBuilderConfigurator
    {
        IClientBuilder Configure(IClientBuilder builder, string clusterId, string serviceId, bool isDevelopment);
    }
}