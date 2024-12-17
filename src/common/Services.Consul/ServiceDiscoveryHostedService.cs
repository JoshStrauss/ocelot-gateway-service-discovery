using Consul;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Services.Consul.Options;

namespace Services.Consul
{
    public class ServiceDiscoveryHostedService : IHostedService
    {
        private readonly IConsulClient _client;
        private readonly ILogger<ServiceDiscoveryHostedService> _logger;
        private readonly ServiceDiscoveryOption _config;
        private AgentServiceRegistration _registration;

        public ServiceDiscoveryHostedService(IConsulClient client, ILogger<ServiceDiscoveryHostedService> logger, IOptions<ServiceDiscoveryOption> config)
        {
            _client = client;
            _logger = logger;
            _config = config.Value;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            if (_config.ConsulUri == null)
            {
                _logger.LogWarning("Consul URI is not configured. Service discovery will not be started.");
                return;
            }

            _logger.LogInformation("Starting service discovery for {ServiceName} with ID {ServiceId}", _config.Name, _config.Id);

            _registration = new AgentServiceRegistration
            {
                ID = _config.Id,
                Name = _config.Name,
                Address = _config.Address,
                Port = _config.Port,
                Check = new AgentServiceCheck
                {
                    DeregisterCriticalServiceAfter = _config.HealthCheckEndPoint.DeregisterCriticalServiceAfter != null 
                        ? TimeSpan.FromMilliseconds(_config.HealthCheckEndPoint.DeregisterCriticalServiceAfter.Value) 
                        : null,
                    Interval = TimeSpan.FromSeconds(_config.HealthCheckEndPoint.Internal ?? 10),
                    Timeout = TimeSpan.FromSeconds(_config.HealthCheckEndPoint.Timeout ?? 10),
                    HTTP = _config.HealthCheckEndPoint.Uri.ToString()
                }
            };

            await _client.Agent.ServiceDeregister(_registration.ID, cancellationToken).ConfigureAwait(false);

            await _client.Agent.ServiceRegister(_registration, cancellationToken).ConfigureAwait(false);

            _logger.LogInformation("Service {ServiceName} with ID {ServiceId} registered successfully", _registration.Name, _registration.ID);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Stopping service discovery for {ServiceName} with ID {ServiceId}", _registration.Name, _registration.ID);
            await _client.Agent.ServiceDeregister(_registration.ID, cancellationToken).ConfigureAwait(false);
            _logger.LogInformation("Service {ServiceName} with ID {ServiceId} deregistered successfully", _registration.Name, _registration.ID);
        }
    }
}