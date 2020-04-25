namespace ServiceBus.Client.Configuration
{
    using System;

    public class AzureServiceBusConsumerConfiguration
        : AzureServiceBusConfiguration
    {
        public string SubscriptionName { get; }
        public AzureServiceBusConsumerConfiguration(string topicName, string policyName, string subscriptionName)
            : base(topicName, policyName)
        {
            SubscriptionName = subscriptionName;
        }
    }
}
