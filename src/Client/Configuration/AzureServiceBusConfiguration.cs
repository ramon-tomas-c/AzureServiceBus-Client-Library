namespace ServiceBus.Client.Configuration
{
    using System;

    public abstract class AzureServiceBusConfiguration
    {
        public string TopicName { get; }
        public string PolicyName { get; }

        protected AzureServiceBusConfiguration(string topicName, string policyName)
        {
            TopicName = topicName;
            PolicyName = policyName;
        }
    }
}
