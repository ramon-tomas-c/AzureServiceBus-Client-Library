namespace ServiceBus.Client.Exceptions
{
    using System;

    public class ServiceBusApiException
        : Exception
    {
        public ServiceBusApiException(string message)
            : base(message)
        {
        }

        public ServiceBusApiException(string message, Exception exception)
            : base(message, exception)
        {
        }
    }
}
