namespace ServiceBus.Client.Exceptions
{
    using System;

    public class EventNotSupportedException
        : Exception
    {
        public EventNotSupportedException(string message)
            : base(message)
        {
        }
    }
}
