namespace ServiceBus.Client.IoCC
{
    using Configuration;
    using Contracts.Factories;
    using Contracts.Managers;
    using Factories;
    using global::Client.Abstractions;
    using Managers;
    using Microsoft.Extensions.DependencyInjection;
    using Options;
    using System;
    using System.Linq;
    using System.Reflection;

    public static class EventConsumerExtensions
    {
        public static IServiceCollection RegisterEventConsumer(
            this IServiceCollection serviceCollection,
            Func<IServiceProvider, ServiceBusConsumerOptions> optionsRetriever,
            params Assembly[] assembliesToDiscover)
        {
            serviceCollection.RegisterCommonServices(optionsRetriever);

            serviceCollection
                .AddSingleton(
                    sp =>
                    {
                        var options = optionsRetriever.Invoke(sp);
                        var azureServiceBusConfiguration =
                            new AzureServiceBusConsumerConfiguration(
                                options.TopicName,
                                options.PolicyName,
                                options.SubscriptionName);
                        return azureServiceBusConfiguration;
                    });

            serviceCollection.AddSingleton<IEventConsumerBusManager, EventConsumerBusManager>();
            serviceCollection.AddSingleton<ISubscriptionClientFactory, SubscriptionClientFactory>();
            serviceCollection.AddSingleton<IEventFactory>(new EventFactory(assembliesToDiscover));

            serviceCollection.Scan(
                x => x.FromAssemblies(assembliesToDiscover)
                        .AddClasses(classes => classes.AssignableTo(typeof(IEventHandler<>)))
                        .AsImplementedInterfaces()
                        .WithTransientLifetime());

            return serviceCollection;
        }
    }
}
