using System;

namespace Client.Sample.Common
{
    public class EventUnknown
    {
        public Guid Id { get; }
        public string Name { get; }
        public DateTime CreatedOn { get; }

        public EventUnknown(Guid id, string name, DateTime createdOn)
        {
            Id = id;
            Name = name;
            CreatedOn = createdOn;
        }
    }
}