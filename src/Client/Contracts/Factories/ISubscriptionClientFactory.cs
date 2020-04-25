namespace ServiceBus.Client.Contracts.Factories
{
    using Microsoft.Azure.ServiceBus;
    using System;
    using System.Threading.Tasks;

    public interface ISubscriptionClientFactory
    {
        Task<ISubscriptionClient> Create(string topicName, string subscriptionName, string policyName);
    }
}