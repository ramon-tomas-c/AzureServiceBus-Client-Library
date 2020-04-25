namespace Client.Sample.Common
{
    public static class Constants
    {
        internal static class Common
        {
            public const string TopicName = "";
            public const string TokenProviderEndpoint = "";
            public const string ClientId = "";
            public const string Secret = "";
            public const string Scope = "";
            public const string ServiceBusApiEndpoint = "";
        }

        public static class Publisher
        {
            public const string TopicName = Common.TopicName;
            public const string PolicyName = "default";
            public const string TokenProviderEndpoint = Common.TokenProviderEndpoint;
            public const string ClientId = Common.ClientId;
            public const string Secret = Common.Secret;
            public const string Scope = Common.Scope;
            public const string ServiceBusApiEndpoint = Common.ServiceBusApiEndpoint;
        }

        public static class Consumer
        {
            public const string TopicName = Common.TopicName;
            public const string PolicyName = "default";
            public const string TokenProviderEndpoint = Common.TokenProviderEndpoint;
            public const string ClientId = Common.ClientId;
            public const string Secret = Common.Secret;
            public const string Scope = Common.Scope;
            public const string ServiceBusApiEndpoint = Common.ServiceBusApiEndpoint;
            public const string SubscriptionOne = "";
        }
    }
}
