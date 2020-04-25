namespace ServiceBus.Client.Abstractions
{
    using System.Threading.Tasks;

    public interface IEventHandler<in TEvent>
        where TEvent : class
    {
        Task Handle(TEvent @event);
    }
}