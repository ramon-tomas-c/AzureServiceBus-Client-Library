namespace Client.Sample.EventConsumer
{
    using Common;
    using Client.Abstractions;
    using System;
    using System.Threading.Tasks;

    public class EventThreeHandler
        : IEventHandler<EventThree>
    {
        public async Task Handle(EventThree @event)
        {
            await Console.Out.WriteLineAsync($"{DateTime.UtcNow.ToLongTimeString()} Received Event Three");
            await Console.Out.WriteLineAsync($" - Id={@event.Id}");
            await Console.Out.WriteLineAsync($" - Name={@event.Name}");
            await Console.Out.WriteLineAsync($" - CreatedOn={@event.CreatedOn.ToLongTimeString()}");
        }
    }
}
