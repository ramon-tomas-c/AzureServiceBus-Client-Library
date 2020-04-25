namespace ServiceBus.Client.Contracts.Factories
{
    using Microsoft.Azure.ServiceBus;
    using System;
    using System.Threading.Tasks;

    public interface ITopicClientFactory
    {
        Task<ITopicClient> Create(string topicName, string policyName);
    }
}