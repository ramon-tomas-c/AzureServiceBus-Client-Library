namespace SB.WebAPI.Persistence.Configuration
{
    using Microsoft.Azure.ServiceBus.Management;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Newtonsoft.Json;
    using SB.WebAPI.Infrastructure.ServiceBus.Dtos;
    using SB.WebAPI.Persistence.Entities;
    using System;
    using System.Collections.Generic;

    class TopicLogConfiguration : IEntityTypeConfiguration<TopicLog>
    {
        public void Configure(EntityTypeBuilder<TopicLog> builder)
        {
            builder.ToTable("TopicLog", "Topics");

            builder.Property(e => e.TopicName)
                .IsRequired()
                .HasMaxLength(256);

            builder.Property(e => e.SubscriptionName)
                .HasMaxLength(256);

            builder.Property(e => e.PolicyName)
                .HasMaxLength(256);
            
            builder.Property(e => e.AccessRights)
                .HasConversion(
                    ar => JsonConvert.SerializeObject(ar),
                    ar => JsonConvert.DeserializeObject<List<AccessRights>>(ar)
                );
            builder.Property(e => e.Rule)
                .HasConversion(
                    ar => JsonConvert.SerializeObject(ar),
                    ar => JsonConvert.DeserializeObject<RuleDescriptionDto>(ar)
                );

            builder.Property(e => e.Action)
                .HasMaxLength(50)
                .HasConversion(
                    a => a.ToString(),
                    a => (TopicAction)Enum.Parse(typeof(TopicAction), a));

        }
    }
}
