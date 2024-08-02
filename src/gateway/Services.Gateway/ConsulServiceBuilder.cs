using Consul;
using Ocelot.Logging;
using Ocelot.Provider.Consul;
using Ocelot.Provider.Consul.Interfaces;

namespace Services.Gateway;

public class ConsulServiceBuilder : DefaultConsulServiceBuilder
{
    public ConsulServiceBuilder(Func<ConsulRegistryConfiguration> configurationFactory, IConsulClientFactory clientFactory, IOcelotLoggerFactory loggerFactory)
        : base(configurationFactory, clientFactory, loggerFactory) { }

    protected override string GetDownstreamHost(ServiceEntry entry, Node node)
    {
        return entry.Service.Address;
    }
}