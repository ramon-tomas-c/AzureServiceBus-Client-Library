namespace ServiceBus.Client.Abstractions
{
    using System.Threading.Tasks;

    public interface IEventBus
    {
        Task Publish<TEvent>(TEvent @event)
            where TEvent : class;
    }
}