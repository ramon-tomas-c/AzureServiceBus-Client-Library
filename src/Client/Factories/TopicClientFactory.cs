namespace ServiceBus.Client.Factories
{
    using Contracts.Factories;
    using Contracts.Services;
    using Microsoft.Azure.ServiceBus;
    using Microsoft.Azure.ServiceBus.Primitives;
    using System;
    using System.Threading.Tasks;

    public class TopicClientFactory
        : ITopicClientFactory
    {
        private readonly IServiceBusApiService _serviceBusApiService;

        public TopicClientFactory(IServiceBusApiService serviceBusApiService)
        {
            _serviceBusApiService = serviceBusApiService;
        }

        public async Task<ITopicClient> Create(string topicName, string policyName)
        {
            var sasTokenResponse = await _serviceBusApiService.GetTopicSasToken(topicName, policyName);
            var tokenProvider = TokenProvider.CreateSharedAccessSignatureTokenProvider(sasTokenResponse.TokenValue);
            return new TopicClient(sasTokenResponse.Endpoint, topicName, tokenProvider);
        }
    }
}
