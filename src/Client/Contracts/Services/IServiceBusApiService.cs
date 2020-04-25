namespace ServiceBus.Client.Contracts.Services
{
    using Models;
    using System.Threading.Tasks;

    public interface IServiceBusApiService
    {
        Task<TopicSharedAccessPolicy> GetTopicSharedAccessPolicy(string topicName, string policyName);
        Task<SasTokenResponse> GetTopicSasToken(string topicName, string policyName);
    }
}