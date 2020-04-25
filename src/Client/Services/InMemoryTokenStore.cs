using System;

namespace ServiceBus.Client.Services
{
    using Contracts.Services;

    public class InMemoryTokenStore
        : ITokenStore
    {
        private string _token;
        private DateTime _expiresOn;

        public void Save(string token, int lifeInSeconds)
        {
            _token = token;
            _expiresOn = GetExpirationUtcDate(lifeInSeconds);
        }

        public bool TryGetToken(out string token)
        {
            var isValid = IsValid();
            token = isValid ? _token : default;
            return isValid;
        }

        private bool IsValid()
        {
            var now = DateTime.UtcNow;
            var isValid =
                !string.IsNullOrWhiteSpace(_token)
                && now < _expiresOn;
            return isValid;
        }

        private DateTime GetExpirationUtcDate(int lifeInSeconds)
        {
            var now = DateTime.UtcNow;
            var result = now.AddSeconds(lifeInSeconds);
            return result;
        }
    }
}
