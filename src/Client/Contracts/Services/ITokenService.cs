namespace ServiceBus.Client.Contracts.Services
{
    using System.Threading.Tasks;

    public interface ITokenService
    {
        Task<string> GetAuthToken();
    }
}