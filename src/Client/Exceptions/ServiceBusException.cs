namespace ServiceBus.Client.Exceptions
{
    using System;

    public class ServiceBusException
        : Exception
    {
        public ServiceBusException(string message, Exception exception)
            : base(message, exception)
        {
        }

        public ServiceBusException(string message)
            : base(message)
        {
        }
    }
}
