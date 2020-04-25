namespace ServiceBus.Client.Models
{
    using System.Collections.Generic;

    public class TokenResponse
    {
        public string Token { get; }
        public int LifeInSeconds { get; }

        public TokenResponse(string token, int lifeInSeconds)
        {
            Token = token;
            LifeInSeconds = lifeInSeconds;
        }

        public override bool Equals(object obj)
        {
            var response = (TokenResponse)obj;
            var isEqual =
                response != null &&
                Token == response.Token &&
                LifeInSeconds == response.LifeInSeconds;
            return isEqual;
        }

        public override int GetHashCode()
        {
            var hashCode = 2002799867;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Token);
            hashCode = hashCode * -1521134295 + LifeInSeconds.GetHashCode();
            return hashCode;
        }
    }
}
