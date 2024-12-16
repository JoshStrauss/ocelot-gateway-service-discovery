using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Consul.Options
{
    public class HealthCheckEndPointOption
    {
        public int? DeregisterCriticalServiceAfter { get; set; }

        public int? Internal { get; set; }

        public int? Timeout { get; set; }

        public Uri Uri { get; set; }
    }
}
