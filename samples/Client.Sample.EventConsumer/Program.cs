namespace Client.Sample.EventConsumer
{
    using ServiceBus.Client.IoCC;
    using ServiceBus.Client.IoCC.Options;
    using Microsoft.Extensions.DependencyInjection;
    using System;
    using System.Reflection;
    using System.Threading.Tasks;
    using Client.Sample.Common;
    using ServiceBus.Client.Contracts.Managers;

    class Program
    {
        static async Task Main(string[] args)
        {
            await Console.Out.WriteLineAsync("Starting event consumer..");

            var serviceProviderConsumerOne =
                new ServiceCollection()                   
                    .RegisterEventConsumer(
                        sp =>
                        {
                            var serviceBusOptions =
                                new ServiceBusConsumerOptions
                                {
                                    TopicName = Constants.Consumer.TopicName,
                                    PolicyName = Constants.Consumer.PolicyName,
                                    SubscriptionName = Constants.Consumer.SubscriptionOne,
                                    TokenProviderUri = new Uri(Constants.Consumer.TokenProviderEndpoint),
                                    ClientId = Constants.Consumer.ClientId,
                                    ClientSecret = Constants.Consumer.Secret,
                                    Scope = Constants.Consumer.Scope,
                                    ServiceBusApiEndpoint = new Uri(Constants.Consumer.ServiceBusApiEndpoint)
                                };
                            return serviceBusOptions;
                        },
                        typeof(EventOneHandler).GetTypeInfo().Assembly)
                    .BuildServiceProvider();

            await Console.Out.WriteLineAsync("Starting consumer one..");
            var eventConsumerOneManager = serviceProviderConsumerOne.GetService<IEventConsumerBusManager>();
            await eventConsumerOneManager.Start();

            await Console.Out.WriteLineAsync("Press enter to end consumers.");
            await Console.In.ReadLineAsync();
            await eventConsumerOneManager.Stop();
        }
    }
}
