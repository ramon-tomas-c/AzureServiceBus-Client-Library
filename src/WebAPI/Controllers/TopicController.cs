namespace SB.WebAPI.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Azure.ServiceBus.Management;
    using SB.Infrastructure.ServiceBus.Dtos;
    using SB.Infrastructure.ServiceBus.Services;
    using SB.WebAPI.Persistence.Entities;
    using SB.WebAPI.Persistence.Repositories;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    [ApiController]
    [ApiVersion("1.0")]
    [Authorize]
    [Route("api/[controller]")]
    public class TopicsController : ControllerBase
    {
        private readonly IServiceBusService _serviceBusService;
        private readonly ITopicRepository _topicRepository;

        public TopicsController(
            IServiceBusService serviceBusService,
            ITopicRepository topicRepository)
        {
            _serviceBusService = serviceBusService ?? throw new ArgumentNullException(nameof(serviceBusService));
            _topicRepository = topicRepository ?? throw new ArgumentNullException(nameof(topicRepository));
        }

        // GET api/topics
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<TopicDescriptionDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult> GetAsync()
        {
            var result = await _serviceBusService.GetTopicsAsync();
            return Ok(result);
        }

        // GET api/topics/topicKeyName
        [Route("{topicKeyName}")]
        [HttpGet]
        [ProducesResponseType(typeof(TopicDescriptionDto), StatusCodes.Status200OK)]
        public async Task<ActionResult> GetAsync(string topicKeyName)
        {
            if (!await _serviceBusService.TopicExistAsync(topicKeyName))
            {
                return NotFound();
            }

            var result = await _serviceBusService.GetTopicAsync(topicKeyName);
            return Ok(result);
        }

        // POST api/topics
        [HttpPost]
        [Authorize(Policy = "AdminScope")]
        [ProducesResponseType(typeof(TopicDescriptionDto), StatusCodes.Status200OK)]
        public async Task<ActionResult> PostAsync(string topicKeyName)
        {
            if (await _serviceBusService.TopicExistAsync(topicKeyName))
            {
                var topic = await _serviceBusService.GetTopicAsync(topicKeyName);
                return Ok(topic);
            }

            var result = await _serviceBusService.CreateTopicAsync(topicKeyName);
            await _topicRepository.AddTopicLogAsync(new TopicLog() { TopicName = topicKeyName, Action = TopicAction.CreateTopic });
            return Ok(result);
        }

        // DELETE api/topics/topicKeyName
        [Route("{topicKeyName}")]
        [HttpDelete]
        [Authorize(Policy = "AdminScope")]
        [ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
        public async Task<ActionResult> DeleteAsync(string topicKeyName)
        {
            if (!await _serviceBusService.TopicExistAsync(topicKeyName))
            {
                return NotFound();
            }

            await _serviceBusService.DeleteTopicAsync(topicKeyName);
            await _topicRepository.AddTopicLogAsync(new TopicLog() { TopicName = topicKeyName, Action = TopicAction.DeleteTopic });
            return Ok();
        }

        // GET api/topics/topicKeyName/authpolicies
        [Route("{topicKeyName}/authpolicies")]
        [HttpGet]
        [Authorize(Policy = "TopicOrAdminScope")]
        [ProducesResponseType(typeof(IEnumerable<SharedAccessAuthorizationPolicyDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult> GetAuthPoliciesAsync(string topicKeyName)
        {
            if (!await _serviceBusService.TopicExistAsync(topicKeyName))
            {
                return NotFound();
            }

            var result = await _serviceBusService.GetSharedAuthenticationPoliciesAsync(topicKeyName);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        // GET api/topics/topicKeyName/authpolicies/policyKeyName
        [Route("{topicKeyName}/authpolicies/{policyKeyName}")]
        [HttpGet]
        [Authorize(Policy = "AdminScope")]
        [ProducesResponseType(typeof(SharedAccessAuthorizationPolicyDetailsDto), StatusCodes.Status200OK)]
        public async Task<ActionResult> GetAuthPolicyAsync(string topicKeyName, string policyKeyName)
        {
            if (!await _serviceBusService.TopicExistAsync(topicKeyName))
            {
                return NotFound();
            }

            var result = await _serviceBusService.GetSharedAuthenticationPolicyAsync(topicKeyName, policyKeyName);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        // POST api/topics/topicKeyName/authpolicies
        [Route("{topicKeyName}/authpolicies")]
        [HttpPost]
        [Authorize(Policy = "AdminScope")]
        [ProducesResponseType(typeof(SharedAccessAuthorizationPolicyDetailsDto), StatusCodes.Status200OK)]
        public async Task<ActionResult> PostAuthPolicyAsync(string topicKeyName, string policyName, List<AccessRights> accessRights)
        {
            if (!await _serviceBusService.TopicExistAsync(topicKeyName))
            {
                return NotFound();
            }

            var result = await _serviceBusService.CreateAuthenticationPolicyAsync(topicKeyName, policyName, accessRights);
            await _topicRepository.AddTopicLogAsync(new TopicLog() { TopicName = topicKeyName, PolicyName = policyName, AccessRights = accessRights, Action = TopicAction.CreateAuthenticationPolicy });
            return Ok(result);
        }

        // DELETE api/topics/topicKeyName/authpolicies/policyKeyName
        [Route("{topicKeyName}/authpolicies/{policyKeyName}")]
        [HttpDelete]
        [Authorize(Policy = "AdminScope")]
        [ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
        public async Task<ActionResult> DeleteAuthPolicyAsync(string topicKeyName, string policyKeyName)
        {
            if (!await _serviceBusService.TopicExistAsync(topicKeyName))
            {
                return NotFound();
            }

            await _serviceBusService.DeleteAuthenticationPolicyAsync(topicKeyName, policyKeyName);
            await _topicRepository.AddTopicLogAsync(new TopicLog() { TopicName = topicKeyName, PolicyName = policyKeyName, Action = TopicAction.DeleteAuthenticationPolicy });
            return Ok();
        }

        // GET api/topics/topicKeyName/authpolicies/policyKeyName/sas
        [Route("{topicKeyName}/authpolicies/{policyKeyName}/sas")]
        [HttpGet]
        [Authorize(Policy = "TopicOrAdminScope")]
        [ProducesResponseType(typeof(SasTokenDto), StatusCodes.Status200OK)]
        public async Task<ActionResult> GetSasTokenAsync(string topicKeyName, string policyKeyName)
        {
            if (!await _serviceBusService.TopicExistAsync(topicKeyName))
            {
                return NotFound();
            }

            var result = await _serviceBusService.GenerateSASTokenAsync(topicKeyName, policyKeyName);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        // GET api/topics/topicKeyName/subscriptions
        [Route("{topicKeyName}/subscriptions")]
        [HttpGet]
        [Authorize(Policy = "TopicOrAdminScope")]
        [ProducesResponseType(typeof(IEnumerable<SubscriptionDescriptionDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult> GetSubscriptionsAsync(string topicKeyName)
        {
            if (!await _serviceBusService.TopicExistAsync(topicKeyName))
            {
                return NotFound();
            }

            var result = await _serviceBusService.GetSubscriptionsAsync(topicKeyName);
            return Ok(result);
        }

        // GET api/topics/topicKeyName/subscriptions/subscriptionKeyName
        [Route("{topicKeyName}/subscriptions/{subscriptionKeyName}")]
        [HttpGet]
        [Authorize(Policy = "TopicOrAdminScope")]
        [ProducesResponseType(typeof(SubscriptionDescriptionDto), StatusCodes.Status200OK)]
        public async Task<ActionResult> GetSubscriptionAsync(string topicKeyName, string subscriptionKeyName)
        {
            if (!await _serviceBusService.SubscriptionExistAsync(topicKeyName, subscriptionKeyName))
            {
                return NotFound();
            }

            var result = await _serviceBusService.GetSubscriptionAsync(topicKeyName, subscriptionKeyName);
            return Ok(result);
        }

        // POST api/topics/topicKeyName/subscriptions
        [Route("{topicKeyName}/subscriptions")]
        [HttpPost]
        [Authorize(Policy = "AdminScope")]
        [ProducesResponseType(typeof(SubscriptionDescriptionDto), StatusCodes.Status200OK)]
        public async Task<ActionResult> CreateSubscriptionAsync(string topicKeyName, string subscriptionKeyName)
        {
            if (await _serviceBusService.SubscriptionExistAsync(topicKeyName, subscriptionKeyName))
            {
                var subscription = await _serviceBusService.GetSubscriptionAsync(topicKeyName, subscriptionKeyName);
                return Ok(subscription);
            }

            var result = await _serviceBusService.CreateSubscriptionAsync(topicKeyName, subscriptionKeyName);
            await _topicRepository.AddTopicLogAsync(new TopicLog() { TopicName = topicKeyName, SubscriptionName = subscriptionKeyName, Action = TopicAction.CreateSubscription });

            return Ok(result);
        }

        // POST api/topics/topicKeyName/subscriptions
        [Route("{topicKeyName}/subscriptions/labelrule")]
        [HttpPost]
        [Authorize(Policy = "AdminScope")]
        [ProducesResponseType(typeof(SubscriptionDescriptionDto), StatusCodes.Status200OK)]
        public async Task<ActionResult> CreateSubscriptionWithLabelRuleAsync(string topicKeyName, string subscriptionKeyName, string ruleName, string labelValueForFilter)
        {
            if (ruleName == null || labelValueForFilter == null)
            {
                return BadRequest("ruleName or labelValueForFilter are mandatory parameters. Check that the request contains these params.");
            }

            if (await _serviceBusService.SubscriptionExistAsync(topicKeyName, subscriptionKeyName))
            {
                var subscription = await _serviceBusService.GetSubscriptionAsync(topicKeyName, subscriptionKeyName);
                return Ok(subscription);
            }

            var result = await _serviceBusService.CreateSubscriptionWithLabelRuleAsync(topicKeyName, subscriptionKeyName, ruleName, labelValueForFilter);
            await _topicRepository.AddTopicLogAsync(new TopicLog() { TopicName = topicKeyName, SubscriptionName = subscriptionKeyName, Action = TopicAction.CreateSubscriptionWithRuleBasedOnLabel, Rule = result.Rule });

            return Ok(result);
        }

        // DELETE api/topics/topicKeyName/subscriptions/subscriptionKeyName
        [Route("{topicKeyName}/subscriptions/{subscriptionKeyName}")]
        [HttpDelete]
        [Authorize(Policy = "AdminScope")]
        [ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
        public async Task<ActionResult> DeleteSubscriptionAsync(string topicKeyName, string subscriptionKeyName)
        {
            if (!await _serviceBusService.SubscriptionExistAsync(topicKeyName, subscriptionKeyName))
            {
                return NotFound();
            }

            await _serviceBusService.DeleteSubscriptionAsync(topicKeyName, subscriptionKeyName);
            await _topicRepository.AddTopicLogAsync(new TopicLog() { TopicName = topicKeyName, SubscriptionName = subscriptionKeyName, Action = TopicAction.DeleteSubscription });
            return Ok();
        }
    }
}