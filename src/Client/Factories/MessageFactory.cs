namespace ServiceBus.Client.Factories
{
    using Contracts.Factories;
    using Microsoft.Azure.ServiceBus;
    using Newtonsoft.Json;
    using System;
    using System.Text;

    public class MessageFactory
        : IMessageFactory
    {
        public Message Create(object @event)
        {
            var json = JsonConvert.SerializeObject(@event);
            var bytes = Encoding.UTF8.GetBytes(json);
            var message =
                new Message(bytes)
                {
                    ContentType = @event.GetType().FullName,
                    MessageId = Guid.NewGuid().ToString(),
                    SessionId = Guid.NewGuid().ToString()
                };
            return message;
        }
    }
}
