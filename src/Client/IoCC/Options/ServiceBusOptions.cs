namespace ServiceBus.Client.IoCC.Options
{
    using System;

    public abstract class ServiceBusOptions
    {
        public string TopicName { get; set; }
        public string PolicyName { get; set; }
        public Uri ServiceBusApiEndpoint { get; set; }
        public Uri TokenProviderUri { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string Scope { get; set; }
    }
}
