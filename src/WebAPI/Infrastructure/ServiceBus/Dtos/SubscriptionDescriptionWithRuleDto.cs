namespace SB.Infrastructure.ServiceBus.Dtos
{
    using Microsoft.Azure.ServiceBus.Management;
    using SB.WebAPI.Infrastructure.ServiceBus.Dtos;

    public class SubscriptionDescriptionWithRuleDto : SubscriptionDescriptionDto
    {
        public SubscriptionDescriptionWithRuleDto(SubscriptionDescription subscriptionDescription, RuleDescriptionDto rule) : base(subscriptionDescription)
        {
            Rule = rule;
        }

        public SubscriptionDescriptionWithRuleDto() { }
        


        public RuleDescriptionDto Rule { get; set; }

    }
}
