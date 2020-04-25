namespace ServiceBus.Client.Configuration
{
    using System;

    public class OpenIdConnectConfiguration
    {
        public Uri TokenProviderUri { get; }
        public string ClientId { get; }
        public string ClientSecret { get; }
        public string Scope { get; }

        public OpenIdConnectConfiguration(Uri tokenProviderUri, string clientId, string clientSecret, string scope)
        {
            TokenProviderUri = tokenProviderUri;
            ClientId = clientId;
            ClientSecret = clientSecret;
            Scope = scope;
        }
    }
}
