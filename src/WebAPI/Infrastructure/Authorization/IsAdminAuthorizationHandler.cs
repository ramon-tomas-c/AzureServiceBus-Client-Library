namespace SB.WebAPI.Infrastructure.Authorization
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.Extensions.Configuration;
    using System.Threading.Tasks;

    /// <summary>
    /// Handler for IsAdminRequirement policy
    /// </summary>
    public class IsAdminAuthorizationHandler : AuthorizationHandler<IsAdminRequirement>
    {
        private readonly string _claimsPrefix;

        /// <summary>
        /// Create a new instance of IsAdminAuthorizationHandler
        /// </summary>
        /// <param name="configuration">Configuration</param>
        public IsAdminAuthorizationHandler(IConfiguration configuration)
        {
            _claimsPrefix = configuration["IdentityServerClaimsPrefix"] ?? string.Empty;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, IsAdminRequirement requirement)
        {
            if (context.User.HasClaim(claim =>
                claim.Type == $"{_claimsPrefix}servicebusrole" && claim.Value == requirement.AdminName))
            {
                context.Succeed(requirement);
            }
            else
            {
                context.Fail();
            }

            return Task.CompletedTask;
        }
    }
}
