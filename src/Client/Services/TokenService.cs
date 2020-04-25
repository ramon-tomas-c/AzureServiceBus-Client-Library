namespace ServiceBus.Client.Services
{
    using Contracts.Services;
    using System.Threading.Tasks;

    public class TokenService
        : ITokenService
    {
        private readonly ITokenStore _tokenStore;
        private readonly IOpenIdConnectService _openIdConnectService;

        public TokenService(
            ITokenStore tokenStore,
            IOpenIdConnectService openIdConnectService)
        {
            _tokenStore = tokenStore;
            _openIdConnectService = openIdConnectService;
        }

        public async Task<string> GetAuthToken()
        {
            var isFound = _tokenStore.TryGetToken(out var cachedToken);
            if (isFound)
            {
                return cachedToken;
            }
            var tokenResponse = await _openIdConnectService.GetAccessToken();
            var token = tokenResponse.Token;
            var lifeInSeconds = tokenResponse.LifeInSeconds;
            _tokenStore.Save(token, lifeInSeconds);
            return token;
        }
    }
}
