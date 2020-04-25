namespace ServiceBus.Client.Contracts.Managers
{
    using System.Threading.Tasks;

    public interface IEventConsumerBusManager
    {
        Task Start();
        Task Stop();
    }
}