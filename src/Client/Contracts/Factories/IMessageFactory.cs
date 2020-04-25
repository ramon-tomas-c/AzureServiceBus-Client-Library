namespace ServiceBus.Client.Contracts.Factories
{
    using Microsoft.Azure.ServiceBus;

    public interface IMessageFactory
    {
        Message Create(object @event);
    }
}