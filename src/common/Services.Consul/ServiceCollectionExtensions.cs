using Consul;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Services.Consul.Options;

namespace Services.Consul
{
    public static class ServiceCollectionExtensions
    {
        public static void AddConsul(this IServiceCollection services, IConfiguration configuration)
        {
            var serviceDiscoveryOption = configuration
                .GetSection(ServiceDiscoveryOption.Section)
                .Get<ServiceDiscoveryOption>();

            services.AddConsul(serviceDiscoveryOption);
        }

        public static void AddConsul(this IServiceCollection services, ServiceDiscoveryOption serviceDiscoveryOption)
        {
            services.AddSingleton(serviceDiscoveryOption);
            services.TryAddSingleton<IConsulClient>(s => CreateConsulClient(serviceDiscoveryOption));
            services.TryAddSingleton<IHostedService, ServiceDiscoveryHostedService>();
        }

        private static ConsulClient CreateConsulClient(ServiceDiscoveryOption serviceConfig)
        {
            return new ConsulClient(config => { config.Address = serviceConfig.ConsulUri; });
        }
    }
}