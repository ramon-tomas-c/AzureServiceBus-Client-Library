namespace ServiceBus.Client.Factories
{
    using Contracts.Factories;
    using System.Net.Http;

    public class HttpClientFactory
        : IHttpClientFactory
    {
        public HttpClient Create()
        {
            var httpClient = new HttpClient();
            return httpClient;
        }
    }
}
