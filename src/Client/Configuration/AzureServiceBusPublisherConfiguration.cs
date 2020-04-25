namespace ServiceBus.Client.Configuration
{
    using System;

    public class AzureServiceBusPublisherConfiguration
        : AzureServiceBusConfiguration
    {
        public AzureServiceBusPublisherConfiguration(string topicName, string policyName)
            : base(topicName, policyName)
        {
        }
    }
}
