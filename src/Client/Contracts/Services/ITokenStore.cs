namespace ServiceBus.Client.Contracts.Services
{
    public interface ITokenStore
    {
        void Save(string token, int lifeInSeconds);
        bool TryGetToken(out string token);
    }
}