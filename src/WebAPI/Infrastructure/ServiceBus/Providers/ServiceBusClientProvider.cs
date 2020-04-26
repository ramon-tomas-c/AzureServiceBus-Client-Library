using Microsoft.Azure.ServiceBus.Management;
using Microsoft.Extensions.Options;
using SB.Infrastructure.ServiceBus.Factories;
using SB.Infrastructure.ServiceBus.Options;

namespace SB.Infrastructure.ServiceBus.Providers
{
    public class ServiceBusClientProvider : IServiceBusClientFactory
    {
        private readonly ServiceBusOptions _serviceBusOptions;

        public ServiceBusClientProvider(IOptions<ServiceBusOptions> options)
        {
            _serviceBusOptions = options.Value;
        }

        public ManagementClient Build()
        {
            return new ManagementClient(_serviceBusOptions.ConnectionString);            
        }
    }
}
