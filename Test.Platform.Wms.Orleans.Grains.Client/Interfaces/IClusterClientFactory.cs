using Orleans;

namespace Test.Platform.Wms.Orleans.Grains.Client.Interfaces
{
    public interface IClusterClientFactory
    {
        IClusterClient Create(string clusterId, string serviceName, bool isDev);
    }
}