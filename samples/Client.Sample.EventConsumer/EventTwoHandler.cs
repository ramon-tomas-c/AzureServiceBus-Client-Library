namespace Client.Sample.EventConsumer
{
    using Common;
    using Client.Abstractions;
    using System;
    using System.Threading.Tasks;

    public class EventTwoHandler
        : IEventHandler<EventTwo>
    {
        public async Task Handle(EventTwo @event)
        {
            await Console.Out.WriteLineAsync($"{DateTime.UtcNow.ToLongTimeString()} Received Event Two");
            await Console.Out.WriteLineAsync($" - Id={@event.Id}");
            await Console.Out.WriteLineAsync($" - Name={@event.Name}");
            await Console.Out.WriteLineAsync($" - CreatedOn={@event.CreatedOn.ToLongTimeString()}");
        }
    }
}
