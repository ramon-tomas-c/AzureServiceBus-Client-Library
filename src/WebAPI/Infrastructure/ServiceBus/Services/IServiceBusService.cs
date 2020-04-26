namespace SB.Infrastructure.ServiceBus.Services
{
    using Microsoft.Azure.Management.ServiceBus.Fluent.Models;
    using SB.Infrastructure.ServiceBus.Dtos;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;
    using AccessRights = Microsoft.Azure.ServiceBus.Management.AccessRights;
    public interface IServiceBusService
    {
        Task<IEnumerable<TopicDescriptionDto>> GetTopicsAsync();
        Task<TopicDescriptionDto> GetTopicAsync(string topicName);
        Task<TopicDescriptionDto> CreateTopicAsync(string topicName);
        Task DeleteTopicAsync(string topicName);
        Task<SharedAccessAuthorizationPolicyDetailsDto> CreateAuthenticationPolicyAsync(string topicName, string policyName, List<AccessRights> accessRights);
        Task DeleteAuthenticationPolicyAsync(string topicName, string policyName);
        Task<SubscriptionDescriptionDto> GetSubscriptionAsync(string topicName, string subsName);
        Task<IEnumerable<SubscriptionDescriptionDto>> GetSubscriptionsAsync(string topicName);
        Task<SubscriptionDescriptionDto> CreateSubscriptionAsync(string topicName, string subsName);
        Task<SubscriptionDescriptionWithRuleDto> CreateSubscriptionWithLabelRuleAsync(string topicName, string subsName, string ruleName, string labelValueForFilter);
        Task DeleteSubscriptionAsync(string topicName, string subsName);
        Task<IEnumerable<SharedAccessAuthorizationPolicyDto>> GetSharedAuthenticationPoliciesAsync(string topicName);
        Task<SharedAccessAuthorizationPolicyDetailsDto> GetSharedAuthenticationPolicyAsync(string topicName, string policyName);
        Task<SasTokenDto> GenerateSASTokenAsync(string topicName, string policyName);
        Task<bool> TopicExistAsync(string topicName);
        Task<bool> SubscriptionExistAsync(string topicName, string subscriptionName);

        /// <summary>
        /// Applies all actions from TopicsLog so ServiceBus is in the same state as database
        /// Any resource in ServiceBus not figuring in the database is ignored.
        /// </summary>
        /// <returns></returns>
        Task SynchronizeServiceBusWithDatabaseAsync();
    }
}
