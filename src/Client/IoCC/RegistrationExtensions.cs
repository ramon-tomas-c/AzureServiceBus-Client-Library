namespace ServiceBus.Client.IoCC
{
    using Configuration;
    using Contracts.Factories;
    using Contracts.Services;
    using Factories;
    using Microsoft.Extensions.DependencyInjection;
    using Options;
    using Services;
    using System;

    internal static class RegistrationExtensions
    {
        internal static IServiceCollection RegisterCommonServices(
            this IServiceCollection serviceCollection,
            Func<IServiceProvider, ServiceBusOptions> optionsRetriever)
        {
            serviceCollection
                .AddSingleton(
                    sp =>
                    {
                        var options = optionsRetriever.Invoke(sp);
                        var serviceBusApiConfiguration = new ServiceBusApiConfiguration(options.ServiceBusApiEndpoint);
                        return serviceBusApiConfiguration;
                    });

            serviceCollection
                .AddSingleton(
                    sp =>
                    {
                        var options = optionsRetriever.Invoke(sp);
                        var openIdConnectConfiguration =
                            new OpenIdConnectConfiguration(options.TokenProviderUri, options.ClientId, options.ClientSecret, options.Scope);
                        return openIdConnectConfiguration;
                    });

            serviceCollection.AddSingleton<ITokenStore, InMemoryTokenStore>();
            serviceCollection.AddSingleton<ITokenService, TokenService>();
            serviceCollection.AddSingleton<IOpenIdConnectService, OpenIdConnectService>();
            serviceCollection.AddSingleton<IHttpClientFactory, HttpClientFactory>();
            serviceCollection.AddSingleton<IServiceBusApiService, ServiceBusApiService>();

            return serviceCollection;
        }
    }
}
