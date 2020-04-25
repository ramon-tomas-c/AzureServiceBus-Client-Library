namespace Client.Sample.EventConsumer
{
    using ServiceBus.Client.Abstractions;
    using Common;
    using System;
    using System.Threading.Tasks;

    public class EventOneHandler
        : IEventHandler<EventOne>
    {
        public async Task Handle(EventOne @event)
        {
            await Console.Out.WriteLineAsync($"{DateTime.UtcNow.ToLongTimeString()} Received Event One");
            await Console.Out.WriteLineAsync($" - Id={@event.Id}");
            await Console.Out.WriteLineAsync($" - Name={@event.Name}");
            await Console.Out.WriteLineAsync($" - CreatedOn={@event.CreatedOn.ToLongTimeString()}");
        }
    }
}
