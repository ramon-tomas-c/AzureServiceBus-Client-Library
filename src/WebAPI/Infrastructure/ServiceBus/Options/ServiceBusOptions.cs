using System;
using System.Collections.Generic;
using System.Text;

namespace SB.Infrastructure.ServiceBus.Options
{
    public class ServiceBusOptions
    {
        public string Endpoint { get; set; }
        public string ConnectionString { get; set; }
        public string Uri { get; set; }
        public int TokenExpirationInDays { get; set; }
    }
}
