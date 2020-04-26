namespace SB.Infrastructure.ServiceBus.Dtos
{
    using Microsoft.Azure.ServiceBus.Primitives;
    using System;

    /// <summary>
    /// SAS Token to access ServiceBus
    /// </summary>
    public class SasTokenDto
    {
        /// <summary>
        /// Creates a new SasTokenDto
        /// </summary>
        /// <param name="securityToken">ServiceBus security token</param>
        /// <param name="endpoint">ServiceBus endpoint</param>
        public SasTokenDto(SecurityToken securityToken, string endpoint)
        {
            Audience = securityToken.Audience;
            ExpiresAtUtc = securityToken.ExpiresAtUtc;
            TokenType = securityToken.TokenType;
            TokenValue = securityToken.TokenValue;
            Endpoint = endpoint;
        }

        /// <summary>
        /// Gets security token Audience
        /// </summary>
        public string Audience { get; }

        /// <summary>
        /// Gets security token ExpiresAtUtc
        /// </summary>
        public DateTime ExpiresAtUtc { get; }

        /// <summary>
        /// Gets security token TokenValue
        /// </summary>
        public virtual string TokenValue { get; }

        /// <summary>
        /// Gets security token TokenType
        /// </summary>
        public virtual string TokenType { get; }

        /// <summary>
        /// Gets ServiceBus endpoint
        /// </summary>
        public string Endpoint { get; }
    }
}
