using System;
using System.Collections.Generic;
using System.Text;

namespace SB.WebAPI.Persistence.Entities
{
    public enum TopicAction
    {
        CreateTopic,
        DeleteTopic,
        CreateSubscription,
        DeleteSubscription,
        DeleteAuthenticationPolicy,
        CreateAuthenticationPolicy,
        CreateSubscriptionWithRuleBasedOnLabel
    }
}
