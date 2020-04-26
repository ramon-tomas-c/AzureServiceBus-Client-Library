using Microsoft.Azure.ServiceBus.Management;
using System;
using System.Collections.Generic;
using System.Text;

namespace SB.Infrastructure.ServiceBus.Dtos
{
    public class SharedAccessAuthorizationPolicyDetailsDto
    {
        public SharedAccessAuthorizationPolicyDetailsDto(
            SharedAccessAuthorizationRule sharedAccessAuthorizationRule)
        {
            KeyName = sharedAccessAuthorizationRule.KeyName;
            PrimaryKey = sharedAccessAuthorizationRule.PrimaryKey;
            SecondaryKey = sharedAccessAuthorizationRule.SecondaryKey;
            Rights = sharedAccessAuthorizationRule.Rights;
        }

        public string KeyName { get; }
        public string PrimaryKey { get; }
        public string SecondaryKey { get; }
        public List<AccessRights> Rights { get; }
    }
}
