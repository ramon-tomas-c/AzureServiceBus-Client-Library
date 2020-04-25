namespace ServiceBus.Client.Services
{
    using Configuration;
    using Contracts.Factories;
    using Contracts.Services;
    using Exceptions;
    using IdentityModel.Client;
    using System;
    using System.Threading.Tasks;
    using TokenResponse = Models.TokenResponse;

    public class OpenIdConnectService
        : IOpenIdConnectService
    {
        private readonly OpenIdConnectConfiguration _openIdConnectConfiguration;
        private readonly IHttpClientFactory _httpClientFactory;

        public OpenIdConnectService(
            OpenIdConnectConfiguration openIdConnectConfiguration,
            IHttpClientFactory httpClientFactory)
        {
            _openIdConnectConfiguration = openIdConnectConfiguration;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<TokenResponse> GetAccessToken()
        {
            var tokenEndpoint = _openIdConnectConfiguration.TokenProviderUri.AbsoluteUri;
            var clientId = _openIdConnectConfiguration.ClientId;
            var clientSecret = _openIdConnectConfiguration.ClientSecret;
            var scope = _openIdConnectConfiguration.Scope;

            using var client = _httpClientFactory.Create();
            var tokenRequest =
                new ClientCredentialsTokenRequest
                {
                    Address = tokenEndpoint,
                    ClientId = clientId,
                    ClientSecret = clientSecret,
                    Scope = scope
                };
            try
            {
                var response = await client.RequestClientCredentialsTokenAsync(tokenRequest);
                if (response.IsError
                    || string.IsNullOrWhiteSpace(response.AccessToken))
                {
                    var message = "The token provider returned an error";
                    throw new OpenIdConnectException(message);
                }
                var token = response.AccessToken;
                var lifeInSeconds = response.ExpiresIn;
                var tokenResponse = new TokenResponse(token, lifeInSeconds);
                return tokenResponse;
            }
            catch (Exception exception)
            {
                var message = $"Unable to retrieve token from {tokenEndpoint} for client Id {clientId} and scope: {scope}";
                throw new OpenIdConnectException(message, exception);
            }
        }
    }
}
