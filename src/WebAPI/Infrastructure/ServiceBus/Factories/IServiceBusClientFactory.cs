using Microsoft.Azure.ServiceBus.Management;

namespace SB.Infrastructure.ServiceBus.Factories
{
    public interface IServiceBusClientFactory
    {
        ManagementClient Build();
    }
}
