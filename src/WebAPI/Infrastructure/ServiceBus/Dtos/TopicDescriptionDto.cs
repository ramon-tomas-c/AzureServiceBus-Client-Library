using Microsoft.Azure.ServiceBus.Management;
using System;
using System.Collections.Generic;
using System.Text;

namespace SB.Infrastructure.ServiceBus.Dtos
{
    public class TopicDescriptionDto
    {
        public TopicDescriptionDto(TopicDescription topicDescription)
        {
            Status = topicDescription.Status;
            Path = topicDescription.Path;
            SupportOrdering = topicDescription.SupportOrdering;
            EnablePartitioning = topicDescription.EnablePartitioning;            
        }

        public EntityStatus Status { get; }
        public string Path { get; }
        public bool SupportOrdering { get; }
        public bool EnablePartitioning { get; }
    }
}
