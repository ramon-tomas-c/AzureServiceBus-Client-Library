namespace ServiceBus.Client.Services
{
    using Configuration;
    using Contracts.Factories;
    using Contracts.Services;
    using Exceptions;
    using Models;
    using Newtonsoft.Json;
    using System;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;

    public class ServiceBusApiService
        : IServiceBusApiService
    {
        private readonly ServiceBusApiConfiguration _serviceBusApiConfiguration;
        private readonly ITokenService _tokenService;
        private readonly IHttpClientFactory _httpClientFactory;

        public ServiceBusApiService(
            ServiceBusApiConfiguration serviceBusApiConfiguration,
            ITokenService tokenService,
            IHttpClientFactory httpClientFactory)
        {
            _serviceBusApiConfiguration = serviceBusApiConfiguration;
            _tokenService = tokenService;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<TopicSharedAccessPolicy> GetTopicSharedAccessPolicy(string topicName, string policyName)
        {
            var authToken = await _tokenService.GetAuthToken();
            var serviceBusApiHost = _serviceBusApiConfiguration.Endpoint;
            using var client = _httpClientFactory.Create();
            var relativeUri = $"api/topics/{topicName}/authpolicies/{policyName}";
            client.BaseAddress = serviceBusApiHost;
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);
            try
            {
                var response = await client.GetAsync(relativeUri);
                var responseJson = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    var url = new Uri(serviceBusApiHost, relativeUri);
                    var responseCode = response.StatusCode;
                    throw new ServiceBusApiException($"Unexpected http response {responseCode}: {responseJson} when retrieving topic shared access policy from {url}");
                }

                var topicSharedAccessPolicy = JsonConvert.DeserializeObject<TopicSharedAccessPolicy>(responseJson);
                return topicSharedAccessPolicy;
            }
            catch (Exception exception)
            {
                var url = new Uri(serviceBusApiHost, relativeUri);
                throw new ServiceBusApiException($"Could not retrieve topic shared access policy from {url}", exception);
            }
        }

        public async Task<SasTokenResponse> GetTopicSasToken(string topicName, string policyName)
        {
            var authToken = await _tokenService.GetAuthToken();
            var serviceBusApiHost = _serviceBusApiConfiguration.Endpoint;
            using var client = _httpClientFactory.Create();
            var relativeUri = $"api/topics/{topicName}/authpolicies/{policyName}/sas";
            client.BaseAddress = serviceBusApiHost;
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);
            try
            {
                var response = await client.GetAsync(relativeUri);
                var responseJson = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    var url = new Uri(serviceBusApiHost, relativeUri);
                    var responseCode = response.StatusCode;
                    throw new ServiceBusApiException($"Unexpected http response {responseCode}: {responseJson} when retrieving topic SAS token from {url}");
                }

                return JsonConvert.DeserializeObject<SasTokenResponse>(responseJson);
            }
            catch (Exception exception)
            {
                var url = new Uri(serviceBusApiHost, relativeUri);
                throw new ServiceBusApiException($"Could not retrieve topic Sas Token from {url}", exception);
            }
        }
    }
}
