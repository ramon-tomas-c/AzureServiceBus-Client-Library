namespace ServiceBus.Client.Contracts.Factories
{
    using Microsoft.Azure.ServiceBus;

    public interface IEventFactory
    {
        object Create(string qualifiedName, Message message);
    }
}