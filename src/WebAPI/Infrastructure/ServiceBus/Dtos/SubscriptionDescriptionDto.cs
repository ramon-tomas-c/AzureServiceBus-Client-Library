using Microsoft.Azure.ServiceBus.Management;
using System;
using System.Collections.Generic;
using System.Text;

namespace SB.Infrastructure.ServiceBus.Dtos
{
    public class SubscriptionDescriptionDto
    {
        public SubscriptionDescriptionDto(SubscriptionDescription subscriptionDescription)
        {
            Status = subscriptionDescription.Status;
            SubscriptionName = subscriptionDescription.SubscriptionName;
            TopicPath = subscriptionDescription.TopicPath;
            RequiresSession = subscriptionDescription.RequiresSession;
            MaxDeliveryCount = subscriptionDescription.MaxDeliveryCount;
            EnableDeadLetteringOnFilterEvaluationExceptions = subscriptionDescription.EnableDeadLetteringOnFilterEvaluationExceptions;
            EnableDeadLetteringOnMessageExpiration = subscriptionDescription.EnableDeadLetteringOnMessageExpiration;
        }

        public SubscriptionDescriptionDto() { }

        public EntityStatus Status { get; }
        public string SubscriptionName { get; }
        public string TopicPath { get; }       
        public bool RequiresSession { get; }
        public int MaxDeliveryCount { get; }
        public bool EnableDeadLetteringOnFilterEvaluationExceptions { get; }
        public bool EnableDeadLetteringOnMessageExpiration { get; }
    }
}
