namespace ServiceBus.Client.Factories
{
    using Contracts.Factories;
    using Contracts.Services;
    using Microsoft.Azure.ServiceBus;
    using Microsoft.Azure.ServiceBus.Primitives;
    using System;
    using System.Threading.Tasks;

    public class SubscriptionClientFactory
        : ISubscriptionClientFactory
    {
        private readonly IServiceBusApiService _serviceBusApiService;

        public SubscriptionClientFactory(IServiceBusApiService serviceBusApiService)
        {
            _serviceBusApiService = serviceBusApiService;
        }
        public async Task<ISubscriptionClient> Create(string topicName, string subscriptionName, string policyName)
        {
            var sasTokenResponse = await _serviceBusApiService.GetTopicSasToken(topicName, policyName);
            var tokenProvider = TokenProvider.CreateSharedAccessSignatureTokenProvider(sasTokenResponse.TokenValue);
            return new SubscriptionClient(sasTokenResponse.Endpoint, topicName, subscriptionName, tokenProvider);
        }
    }
}
