namespace ServiceBus.Client.Contracts.Services
{
    using Models;
    using System.Threading.Tasks;

    public interface IOpenIdConnectService
    {
        Task<TokenResponse> GetAccessToken();
    }
}