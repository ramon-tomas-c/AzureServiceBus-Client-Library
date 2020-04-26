using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SB.WebAPI.Infrastructure.Authorization
{
    public class IsAdminRequirement : IAuthorizationRequirement
    {
        public string AdminName { get; set; }
        public IsAdminRequirement(string adminName)
        {
            AdminName = adminName;
        }
    }
}
