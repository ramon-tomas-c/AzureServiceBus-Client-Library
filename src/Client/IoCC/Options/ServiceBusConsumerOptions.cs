namespace ServiceBus.Client.IoCC.Options
{
    public sealed class ServiceBusConsumerOptions
        : ServiceBusOptions
    {
        public string SubscriptionName { get; set; }
    }
}