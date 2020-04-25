using System;

namespace Client.Sample.Common
{
    public class EventThree
    {
        public Guid Id { get; }
        public string Name { get; }
        public DateTime CreatedOn { get; }

        public EventThree(Guid id, string name, DateTime createdOn)
        {
            Id = id;
            Name = name;
            CreatedOn = createdOn;
        }
    }
}