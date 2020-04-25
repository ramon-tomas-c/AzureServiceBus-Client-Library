namespace ServiceBus.Client.Exceptions
{
    using System;

    public class OpenIdConnectException
        : Exception
    {
        public OpenIdConnectException(string message)
            : base(message)
        {
        }

        public OpenIdConnectException(string message, Exception exception)
            : base(message, exception)
        {
        }
    }
}
