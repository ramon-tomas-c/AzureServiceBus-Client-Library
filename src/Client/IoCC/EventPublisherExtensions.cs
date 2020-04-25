namespace ServiceBus.Client.IoCC
{
    using Configuration;
    using Contracts.Factories;
    using Factories;
    using global::Client.Abstractions;
    using Microsoft.Extensions.DependencyInjection;
    using Options;
    using System;

    public static class EventPublisherExtensions
    {
        public static IServiceCollection RegisterEventPublisher(
            this IServiceCollection serviceCollection,
            Func<IServiceProvider, ServiceBusPublisherOptions> optionsRetriever)
        {
            serviceCollection.RegisterCommonServices(optionsRetriever);

            serviceCollection
                .AddSingleton(
                    sp =>
                    {
                        var options = optionsRetriever.Invoke(sp);
                        var azureServiceBusConfiguration =
                            new AzureServiceBusPublisherConfiguration(options.TopicName, options.PolicyName);
                        return azureServiceBusConfiguration;
                    });

            serviceCollection.AddSingleton<ITopicClientFactory, TopicClientFactory>();
            serviceCollection.AddSingleton<IMessageFactory, MessageFactory>();
            serviceCollection.AddSingleton<IEventBus, EventBus>();

            return serviceCollection;
        }
    }
}
