using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SB.WebAPI.Infrastructure.Authorization
{
    public class IsTopicOrAdminRequirement : IAuthorizationRequirement
    {
        public string AdminName { get; set; }
        public IsTopicOrAdminRequirement(string adminName)
        {
            AdminName = adminName;
        }
    }
}
