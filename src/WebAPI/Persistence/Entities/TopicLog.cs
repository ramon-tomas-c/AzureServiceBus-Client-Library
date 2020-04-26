namespace SB.WebAPI.Persistence.Entities
{
    using Microsoft.Azure.ServiceBus.Management;
    using SB.WebAPI.Infrastructure.ServiceBus.Dtos;
    using System;
    using System.Collections.Generic;

    /// <summary>
    ///  Class to store the actions done on topics
    /// </summary>
    public class TopicLog 
    {
        /// <summary>
        /// Constructor of TopicLog
        /// </summary>
        public TopicLog()
        {
            
        }

        /// <summary>
        /// Primary key
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Action executed
        /// </summary>
        public TopicAction Action { get; set; }

        /// <summary>
        /// Name of the Topic
        /// </summary>
        public string TopicName { get; set; }

        /// <summary>
        /// Name of the Subscription
        /// </summary>
        public string SubscriptionName { get; set; }

        /// <summary>
        /// JSON associated to the rule assigned to the subscription
        /// </summary>
        public RuleDescriptionDto Rule { get; set; }


        /// <summary>
        /// Name of the Access Policy
        /// </summary>
        public string PolicyName { get; set; }

        /// <summary>
        /// Access Rights of a policy
        /// </summary>
        public List<AccessRights> AccessRights { get; set; }

        /// <summary>
        /// UTC date and time of the action
        /// </summary>
        public DateTime Timestamp { get; set; }
    }
}
