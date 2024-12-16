namespace Services.Consul.Options;

public class ServiceDiscoveryOption
{
    public static string Section => "ServiceDiscovery";

    public string Id { get; set; }

    public string Name { get; set; }

    public Uri ConsulUri { get; set; }

    public string Address { get; set; }

    public int Port { get; set; }

    public HealthCheckEndPointOption HealthCheckEndPoint { get; set; }
}