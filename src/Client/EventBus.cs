namespace ServiceBus.Client
{
    using Configuration;
    using Contracts.Factories;
    using global::Client.Abstractions;
    using System.Threading.Tasks;

    public class EventBus
        : IEventBus
    {
        private readonly AzureServiceBusPublisherConfiguration _azureServiceBusPublisherConfiguration;
        private readonly ITopicClientFactory _topicClientFactory;
        private readonly IMessageFactory _messageFactory;

        public EventBus(
            AzureServiceBusPublisherConfiguration azureServiceBusPublisherConfiguration,
            ITopicClientFactory topicClientFactory,
            IMessageFactory messageFactory)
        {
            _azureServiceBusPublisherConfiguration = azureServiceBusPublisherConfiguration;
            _topicClientFactory = topicClientFactory;
            _messageFactory = messageFactory;
        }

        public async Task Publish<TEvent>(TEvent @event)
            where TEvent : class
        {
            var topicName = _azureServiceBusPublisherConfiguration.TopicName;
            var policyName = _azureServiceBusPublisherConfiguration.PolicyName;
            var topicClient = await _topicClientFactory.Create(topicName, policyName);
            var message = _messageFactory.Create(@event);
            await topicClient.SendAsync(message);
        }
    }
}
