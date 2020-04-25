namespace ServiceBus.Client.Contracts.Factories
{
    using System.Net.Http;

    public interface IHttpClientFactory
    {
        HttpClient Create();
    }
}