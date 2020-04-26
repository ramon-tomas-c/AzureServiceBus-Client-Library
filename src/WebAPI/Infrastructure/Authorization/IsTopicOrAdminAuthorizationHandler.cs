namespace SB.WebAPI.Infrastructure.Authorization
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Routing;
    using Microsoft.Extensions.Configuration;
    using System.Threading.Tasks;

    /// <summary>
    /// Handler for IsTopicOrAdminRequirement
    /// </summary>
    public class IsTopicOrAdminAuthorizationHandler : AuthorizationHandler<IsTopicOrAdminRequirement>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string _claimsPrefix;

        /// <summary>
        /// Create a new instance of IsTopicOrAdminAuthorizationHandler
        /// </summary>
        /// <param name="configuration">Configuration</param>
        public IsTopicOrAdminAuthorizationHandler(IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration)
        {
            _httpContextAccessor = httpContextAccessor;
            _claimsPrefix = configuration["IdentityServerClaimsPrefix"] ?? string.Empty;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, IsTopicOrAdminRequirement requirement)
        {
            if (context.User.HasClaim(claim =>
                claim.Type == $"{_claimsPrefix}servicebusrole" && claim.Value == requirement.AdminName))
            {
                context.Succeed(requirement);
                return Task.CompletedTask;
            }

            var topicKeyName = string.Empty;

            var routes = _httpContextAccessor.HttpContext.GetRouteData();
            topicKeyName = routes?.Values["topicKeyName"]?.ToString() as string;

            if (context.User.HasClaim(claim =>
                claim.Type == $"{_claimsPrefix}servicebustopic" && claim.Value == topicKeyName))
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
