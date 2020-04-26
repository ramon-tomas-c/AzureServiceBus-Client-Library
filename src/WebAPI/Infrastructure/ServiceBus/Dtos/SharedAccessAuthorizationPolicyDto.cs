using System.Collections.Generic;
using AccessRights = Microsoft.Azure.ServiceBus.Management.AccessRights;

namespace SB.Infrastructure.ServiceBus.Dtos
{
    public class SharedAccessAuthorizationPolicyDto
    {
        public string PolicyKeyName { get; }
        public IEnumerable<AccessRights> AccessRights { get; }
        public SharedAccessAuthorizationPolicyDto(string policyKeyName, IEnumerable<AccessRights> accessRights)
        {
            PolicyKeyName = policyKeyName;
            AccessRights = accessRights;
        }
    }
}
