namespace SB.Infrastructure.ServiceBus.Services
{
    using Microsoft.Azure.ServiceBus;
    using Microsoft.Azure.ServiceBus.Management;
    using Microsoft.Azure.ServiceBus.Primitives;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Newtonsoft.Json;
    using SB.Infrastructure.ServiceBus.Dtos;
    using SB.Infrastructure.ServiceBus.Factories;
    using SB.Infrastructure.ServiceBus.Options;
    using SB.WebAPI.Infrastructure.ServiceBus.Dtos;
    using SB.WebAPI.Persistence.Entities;
    using SB.WebAPI.Persistence.Repositories;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AccessRights = Microsoft.Azure.ServiceBus.Management.AccessRights;

    internal class ServiceBusService : IServiceBusService
    {
        private readonly ManagementClient _managementClient;
        private readonly ServiceBusOptions _serviceBusOptions;
        private readonly ITopicRepository _topicRepository;
        private readonly ILogger<ServiceBusService> _logger;

        public ServiceBusService(
            IServiceBusClientFactory serviceBusClientFactory,
            IOptions<ServiceBusOptions> options,
            ITopicRepository topicRepository,
            ILogger<ServiceBusService> logger)
        {
            _managementClient = serviceBusClientFactory.Build();
            _serviceBusOptions = options.Value;
            _topicRepository = topicRepository ?? throw new ArgumentNullException(nameof(topicRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<TopicDescriptionDto>> GetTopicsAsync()
        {
            var topicList = await _managementClient
                .GetTopicsAsync();

            _logger.LogInformation($"{topicList.Count} topics retrieved");
            return topicList
                .Select(topic => new TopicDescriptionDto(topic));
        }

        public async Task<TopicDescriptionDto> GetTopicAsync(string topicName)
        {
            var topic = await _managementClient
                .GetTopicAsync(topicName);

            _logger.LogInformation($"Topic {topicName} retrieved");
            return new TopicDescriptionDto(topic);
        }

        public async Task<TopicDescriptionDto> CreateTopicAsync(string topicName)
        {
            var topicDescriptor = new TopicDescription(topicName)
            {
                RequiresDuplicateDetection = true,
                SupportOrdering = true,
                EnablePartitioning = true,
            };

            var topic = await _managementClient
                .CreateTopicAsync(topicDescriptor);

            _logger.LogInformation($"Creating Topic {topicName}");

            return new TopicDescriptionDto(topic);
        }

        public async Task DeleteTopicAsync(string topicName)
        {
            await _managementClient
                    .DeleteTopicAsync(topicName);

            _logger.LogInformation($"Deleting Topic {topicName}");
        }
        public async Task<IEnumerable<SharedAccessAuthorizationPolicyDto>> GetSharedAuthenticationPoliciesAsync(string topicName)
        {
            var topic = await _managementClient
                .GetTopicAsync(topicName);

            var authPolicyList = topic.AuthorizationRules
                .Select(authRule => new SharedAccessAuthorizationPolicyDto(authRule.KeyName, authRule.Rights))
                .ToList();

            _logger.LogInformation($"Shared Authentication Policies for Topic {topicName} retrieved");

            return authPolicyList;
        }

        public async Task<SharedAccessAuthorizationPolicyDetailsDto> GetSharedAuthenticationPolicyAsync(string topicName, string policyName)
        {
            var topic = await _managementClient
                .GetTopicAsync(topicName);

            var authPolicy = topic.AuthorizationRules
                .Find(rule => rule.KeyName == policyName) as SharedAccessAuthorizationRule;
            if (authPolicy == null)
            {
                return null;
            }

            _logger.LogInformation($"Shared Authentication Policy {policyName} for Topic {topicName} retrieved");

            return new SharedAccessAuthorizationPolicyDetailsDto(authPolicy);
        }

        public async Task<SharedAccessAuthorizationPolicyDetailsDto> CreateAuthenticationPolicyAsync(
            string topicName, string policyName, List<AccessRights> accessRights)
        {

            var topicDescription = await _managementClient
                .GetTopicAsync(topicName);

            var policyToRemove = topicDescription.AuthorizationRules
                .Find(rule => rule.KeyName == policyName);

            topicDescription.AuthorizationRules.Remove(policyToRemove);

            topicDescription.AuthorizationRules.Add(new SharedAccessAuthorizationRule(policyName, accessRights));

            var updatedTopic = await _managementClient
                .UpdateTopicAsync(topicDescription);

            var createdAuthPolicy = updatedTopic.AuthorizationRules
                .Find(rule => rule.KeyName == policyName) as SharedAccessAuthorizationRule;

            _logger.LogInformation($"Authentication Policy {policyName} for Topic {topicName} created");

            return new SharedAccessAuthorizationPolicyDetailsDto(createdAuthPolicy);
        }

        public async Task<SasTokenDto> GenerateSASTokenAsync(
            string topicName, string policyName)
        {
            var topic = await _managementClient
                .GetTopicAsync(topicName);

            var authPolicy = topic.AuthorizationRules
                .Find(rule => rule.KeyName == policyName) as SharedAccessAuthorizationRule;

            if (authPolicy == null)
            {
                return null;
            }

            var sbHostName = new Uri(_serviceBusOptions.Endpoint).Host;
            var sbEndpoint = $"sb://{sbHostName}";

            var tokenProvider = TokenProvider
                .CreateSharedAccessSignatureTokenProvider(authPolicy.KeyName, authPolicy.PrimaryKey);

            var token = await tokenProvider
                .GetTokenAsync($"{_serviceBusOptions.Endpoint}{topicName}", TimeSpan.FromDays(_serviceBusOptions.TokenExpirationInDays));

            _logger.LogInformation($"SAS obtained for {policyName} for Topic {topicName}");

            return new SasTokenDto(token, sbEndpoint);
        }

        public async Task DeleteAuthenticationPolicyAsync(
            string topicName, string policyName)
        {
            var topicDescription = await _managementClient
                .GetTopicAsync(topicName);

            var policyToRemove = topicDescription.AuthorizationRules
                .Find(rule => rule.KeyName == policyName);

            topicDescription.AuthorizationRules.Remove(policyToRemove);

            await _managementClient
                .UpdateTopicAsync(topicDescription);

            _logger.LogInformation($"Auhtentication policy for {policyName} for Topic {topicName} deleted");

        }

        public async Task<SubscriptionDescriptionDto> GetSubscriptionAsync(string topicName, string subsName)
        {
            var subs = await _managementClient
                .GetSubscriptionAsync(topicName, subsName);

            _logger.LogInformation($"Subscription {subsName} for Topic {topicName} retrieved");

            return new SubscriptionDescriptionDto(subs);
        }

        public async Task<IEnumerable<SubscriptionDescriptionDto>> GetSubscriptionsAsync(string topicName)
        {
            var subsList = await _managementClient
                .GetSubscriptionsAsync(topicName);

            _logger.LogInformation($"Subscriptions for Topic {topicName} retrieved");

            return subsList
                .Select(subs => new SubscriptionDescriptionDto(subs));
        }

        public async Task<SubscriptionDescriptionDto> CreateSubscriptionAsync(string topicName, string subsName)
        {
            var subs = await _managementClient
                .CreateSubscriptionAsync(topicName, subsName);

            _logger.LogInformation($"Creating Subscription {subsName} for Topic {topicName}");

            return new SubscriptionDescriptionDto(subs);
        }

        public async Task<SubscriptionDescriptionWithRuleDto> CreateSubscriptionWithLabelRuleAsync(string topicName, string subsName, string ruleName, string labelValueForFilter)
        {
            RuleDescription labelBasedRule = new RuleDescription()
            {
                Filter = new CorrelationFilter() { Label = labelValueForFilter },
                Name = ruleName
            };

            _logger.LogInformation($"Creating Subscription {subsName} with a Rule named {ruleName} based on the label value {labelValueForFilter} for Topic {topicName}");

            var subscriptionDescriptionDto = await CreateSubscriptionWithRuleAsync(topicName, subsName, labelBasedRule);

            return subscriptionDescriptionDto;
        }

        public async Task<SubscriptionDescriptionWithRuleDto> CreateSubscriptionWithRuleAsync(string topicName, string subsName, RuleDescription rule)
        {
            var subs = await _managementClient
                .CreateSubscriptionAsync(topicName, subsName);

            await _managementClient.DeleteRuleAsync(topicName, subsName, RuleDescription.DefaultRuleName);
            await _managementClient.CreateRuleAsync(topicName, subsName, rule);

            _logger.LogInformation($"Created Subscription {subsName} with a Rule named {rule.Name} for Topic {topicName}");

            var correlationFilter = ((CorrelationFilter)rule.Filter);
            var ruleDescriptionDto = new RuleDescriptionDto()
            {
                Name = rule.Name,
                Filter = new CorrelationFilterDto()
                {
                    Label = correlationFilter.Label,
                    ContentType = correlationFilter.ContentType,
                    CorrelationId = correlationFilter.CorrelationId,
                    MessageId = correlationFilter.MessageId,
                    ReplyTo = correlationFilter.ReplyTo,
                    ReplyToSessionId = correlationFilter.ReplyToSessionId,
                    SessionId = correlationFilter.SessionId,
                    To = correlationFilter.To
                }
            };
            return new SubscriptionDescriptionWithRuleDto(subs, ruleDescriptionDto);
        }

        public async Task DeleteSubscriptionAsync(string topicName, string subsName)
        {
            _logger.LogInformation($"Deleting Subscription {subsName} for Topic {topicName}");

            await _managementClient
                .DeleteSubscriptionAsync(topicName, subsName);
        }

        public async Task<bool> TopicExistAsync(string topicName)
        {
            bool exists = await _managementClient.TopicExistsAsync(topicName);

            _logger.LogDebug($@"Topic {topicName} {(exists ? "exists" : "do not exists")}.");

            return exists;
        }

        public async Task<bool> SubscriptionExistAsync(string topicName, string subscriptionName)
        {
            bool exists = await _managementClient.SubscriptionExistsAsync(topicName, subscriptionName);

            _logger.LogDebug($@"Subscription {subscriptionName} on topic {topicName} {(exists ? "exists" : "do not exists")}.");
            return exists;
        }

        public async Task<bool> AuthenticationPolicyExistAsync(string topicName, string policyName)
        {
            bool topicExists = await _managementClient.TopicExistsAsync(topicName);
            TopicDescription topic;

            if (topicExists)
            {
                topic = await _managementClient.GetTopicAsync(topicName);
            }
            else
            {
                _logger.LogDebug($@"Topic {topicName} does not exist so the policy neither.");
                return topicExists;
            }

            bool policyExists = topic.AuthorizationRules.Find(rule => rule.KeyName == policyName) != null;

            _logger.LogDebug($@"Policy {policyExists} {(policyExists ? "exists" : "do not exists")}.");

            return policyExists;
        }

        /// <summary>
        /// Applies all actions from TopicsLog so ServiceBus is in the same state as database
        /// Any resource in ServiceBus not figuring in the database is ignored.
        /// </summary>
        /// <returns></returns>
        public async Task SynchronizeServiceBusWithDatabaseAsync()
        {
            _logger.LogInformation($@"Synchronizing ServiceBus");
            var topicLogs = (await _topicRepository.GetAllTopicLogAsync()).OrderBy(t => t.Timestamp);

            // We don't want to mess with ServiceBus so we calculate in memory how the topics should be
            var intendedStatus = CalculateIntendedStatus(topicLogs);

            // Now we update ServiceBus
            await SynchronizeServiceBus(intendedStatus);

            _logger.LogInformation($@"ServiceBus Synchronized ");
        }

        private async Task SynchronizeServiceBus(List<TopicLog> intendedStatus)
        {
            foreach (var entry in intendedStatus)
            {
                switch (entry.Action)
                {
                    case TopicAction.CreateTopic:
                        if (!await TopicExistAsync(entry.TopicName))
                            await CreateTopicAsync(entry.TopicName);
                        break;
                    case TopicAction.DeleteTopic:
                        if (await TopicExistAsync(entry.TopicName))
                            await DeleteTopicAsync(entry.TopicName);
                        break;
                    case TopicAction.CreateSubscription:
                        if (!await SubscriptionExistAsync(entry.TopicName, entry.SubscriptionName))
                            await CreateSubscriptionAsync(entry.TopicName, entry.SubscriptionName);
                        break;
                    case TopicAction.CreateSubscriptionWithRuleBasedOnLabel:
                        if (!await SubscriptionExistAsync(entry.TopicName, entry.SubscriptionName))
                        {
                            var rule = entry.Rule;
                            await CreateSubscriptionWithLabelRuleAsync(entry.TopicName, entry.SubscriptionName, rule.Name, ((CorrelationFilterDto)rule.Filter).Label);
                        }
                        break;
                    case TopicAction.DeleteSubscription:
                        if (await SubscriptionExistAsync(entry.TopicName, entry.SubscriptionName))
                            await DeleteSubscriptionAsync(entry.TopicName, entry.SubscriptionName);
                        break;
                    case TopicAction.CreateAuthenticationPolicy:
                        if (!await AuthenticationPolicyExistAsync(entry.TopicName, entry.PolicyName))
                            await CreateAuthenticationPolicyAsync(entry.TopicName, entry.PolicyName, entry.AccessRights);
                        break;
                    case TopicAction.DeleteAuthenticationPolicy:
                        if (await AuthenticationPolicyExistAsync(entry.TopicName, entry.PolicyName))
                            await DeleteAuthenticationPolicyAsync(entry.TopicName, entry.PolicyName);
                        break;
                    default:
                        break;
                }
            }
        }

        private List<TopicLog> CalculateIntendedStatus(IOrderedEnumerable<TopicLog> topicLogs)
        {
            List<TopicLog> currentStatus = new List<TopicLog>();
            foreach (var entry in topicLogs)
            {
                switch (entry.Action)
                {
                    case TopicAction.CreateTopic:
                    case TopicAction.DeleteTopic:
                        currentStatus.RemoveAll(e => e.TopicName == entry.TopicName);
                        break;
                    case TopicAction.DeleteSubscription:
                    case TopicAction.CreateSubscription:
                        currentStatus.RemoveAll(e => e.TopicName == entry.TopicName &&
                            (e.Action == TopicAction.CreateSubscription
                                || e.Action == TopicAction.DeleteSubscription));
                        break;
                    case TopicAction.CreateAuthenticationPolicy:
                    case TopicAction.DeleteAuthenticationPolicy:
                        currentStatus.RemoveAll(e => e.TopicName == entry.TopicName
                            && (e.Action == TopicAction.CreateAuthenticationPolicy
                                 || e.Action == TopicAction.DeleteAuthenticationPolicy));
                        break;
                    default:
                        break;
                }

                currentStatus.Add(entry);
            }
            return currentStatus;
        }
    }
}
