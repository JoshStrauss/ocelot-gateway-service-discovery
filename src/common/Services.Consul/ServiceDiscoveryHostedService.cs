using Consul;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Services.Consul.Options;

namespace Services.Consul
{
	public class ServiceDiscoveryHostedService : IHostedService
	{
		private readonly IConsulClient _client;
		private readonly ServiceDiscoveryOption _config;
		private AgentServiceRegistration _registration;

		public ServiceDiscoveryHostedService(IConsulClient client, IOptions<ServiceDiscoveryOption> config)
		{
			_client = client;
			_config = config.Value;
		}

		public async Task StartAsync(CancellationToken cancellationToken)
		{
            if (_config.ConsulUri == null)
                return;

            Console.WriteLine(_config.Name);

			_registration = new AgentServiceRegistration
			{
				ID = _config.Id,
				Name = _config.Name,
				Address = _config.Address,
				Port = _config.Port,
                Check = new AgentServiceCheck
                {
					DeregisterCriticalServiceAfter = _config.HealthCheckEndPoint.DeregisterCriticalServiceAfter != null ? TimeSpan.FromMilliseconds(_config.HealthCheckEndPoint.DeregisterCriticalServiceAfter.Value) : null,
                    Interval = TimeSpan.FromSeconds(_config.HealthCheckEndPoint.Internal ?? 10),
                    Timeout = TimeSpan.FromSeconds(_config.HealthCheckEndPoint.Timeout ?? 10),
                    HTTP = _config.HealthCheckEndPoint.Uri.ToString()
                }
			};

			await _client.Agent.ServiceDeregister(_registration.ID, cancellationToken).ConfigureAwait(false);
			await _client.Agent.ServiceRegister(_registration, cancellationToken).ConfigureAwait(false);
		}

		public async Task StopAsync(CancellationToken cancellationToken)
		{
			await _client.Agent.ServiceDeregister(_registration.ID, cancellationToken).ConfigureAwait(false);
		}
	}
}