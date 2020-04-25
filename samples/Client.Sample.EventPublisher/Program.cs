namespace Client.Sample.EventPublisher
{
    using Common;
    using ServiceBus.Client.IoCC;
    using ServiceBus.Client.IoCC.Options;
    using Microsoft.Extensions.DependencyInjection;
    using System;
    using System.Threading.Tasks;
    using Client.Abstractions;

    class Program
    {
        static async Task Main(string[] args)
        {
            await Console.Out.WriteLineAsync("Starting event publisher..");

            var serviceProvider =
                new ServiceCollection()
                    .RegisterEventPublisher(
                        sp =>
                        {
                            var serviceBusOptions =
                                new ServiceBusPublisherOptions
                                {
                                    TopicName = Constants.Publisher.TopicName,
                                    PolicyName = Constants.Publisher.PolicyName,
                                    TokenProviderUri = new Uri(Constants.Publisher.TokenProviderEndpoint),
                                    ClientId = Constants.Publisher.ClientId,
                                    ClientSecret = Constants.Publisher.Secret,
                                    Scope = Constants.Publisher.Scope,
                                    ServiceBusApiEndpoint = new Uri(Constants.Publisher.ServiceBusApiEndpoint)
                                };
                            return serviceBusOptions;
                        })
                    .BuildServiceProvider();

            var eventBus = serviceProvider.GetService<IEventBus>();
            var iterations = 1;
            for (var i = 0; i < iterations; i++)
            {
                await Console.Out.WriteLineAsync("Publishing event one..");
                await eventBus.Publish(new EventOne(Guid.NewGuid(), "One", DateTime.UtcNow));
                await Console.Out.WriteLineAsync("Publishing event two..");
                await eventBus.Publish(new EventTwo(Guid.NewGuid(), "Two", DateTime.UtcNow));
                await Console.Out.WriteLineAsync("Publishing event three..");
                await eventBus.Publish(new EventThree(Guid.NewGuid(), "Three", DateTime.UtcNow));
                await Console.Out.WriteLineAsync("Publishing event unknown..");
                await eventBus.Publish(new EventUnknown(Guid.NewGuid(), "Unknown", DateTime.UtcNow));
            }

            await Console.Out.WriteLineAsync("Press enter to end publisher.");
            await Console.In.ReadLineAsync();
        }
    }
}
