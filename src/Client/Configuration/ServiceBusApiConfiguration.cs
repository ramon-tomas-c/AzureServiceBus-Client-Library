namespace ServiceBus.Client.Configuration
{
    using System;

    public class ServiceBusApiConfiguration
    {
        public Uri Endpoint { get; }

        public ServiceBusApiConfiguration(Uri endpoint)
        {
            Endpoint = endpoint;
        }
    }
}
