namespace ServiceBus.Client.Managers
{
    using Configuration;
    using Contracts.Factories;
    using Contracts.Managers;
    using Exceptions;
    using global::Client.Abstractions;
    using Microsoft.Azure.ServiceBus;
    using Microsoft.Extensions.DependencyInjection;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Event Consumer Bus Manager that subscribes to a specific subscription, deserializes received messages and instantiates
    /// the correct event handler to pass the deserialized event.
    /// </summary>
    public class EventConsumerBusManager
        : IEventConsumerBusManager, IAsyncDisposable
    {
        private readonly AzureServiceBusConsumerConfiguration _azureServiceBusConsumerConfiguration;
        private readonly ISubscriptionClientFactory _subscriptionClientFactory;
        private readonly IEventFactory _eventFactory;
        private readonly IServiceProvider _serviceProvider;
        private ISubscriptionClient _subscriptionClient;

        public EventConsumerBusManager(
            AzureServiceBusConsumerConfiguration azureServiceBusConsumerConfiguration,
            ISubscriptionClientFactory subscriptionClientFactory,
            IEventFactory eventFactory,
            IServiceProvider serviceProvider)
        {
            _azureServiceBusConsumerConfiguration = azureServiceBusConsumerConfiguration;
            _subscriptionClientFactory = subscriptionClientFactory;
            _serviceProvider = serviceProvider;
            _eventFactory = eventFactory;
        }

        public async Task Start()
        {
            var topicName = _azureServiceBusConsumerConfiguration.TopicName;
            var policyName = _azureServiceBusConsumerConfiguration.PolicyName;
            var subscriptionName = _azureServiceBusConsumerConfiguration.SubscriptionName;
            _subscriptionClient = await _subscriptionClientFactory.Create(topicName, subscriptionName, policyName);
            RegisterHandlers();
        }
        
        public async Task Stop()
        {
            await DisposeAsync();
        }

        public async ValueTask DisposeAsync()
        {
            if (_subscriptionClient != null)
            {               
                await _subscriptionClient.CloseAsync();
                _subscriptionClient = null;
            } 
        }

        private async Task RefreshSubscriptionClient()
        {
            await DisposeAsync();       

            _subscriptionClient = await _subscriptionClientFactory
                .Create(_azureServiceBusConsumerConfiguration.TopicName, _azureServiceBusConsumerConfiguration.SubscriptionName, _azureServiceBusConsumerConfiguration.PolicyName);
            
            RegisterHandlers();
        }

        private void RegisterHandlers()
        {
            _subscriptionClient.RegisterMessageHandler(MessageHandler, MessageHandlerOptions);
        }

        private Func<Message, CancellationToken, Task> MessageHandler => async (message, handler) =>
        {
            var messageTypeName = message.ContentType;
            var @event = _eventFactory.Create(messageTypeName, message);
            var eventType = @event.GetType();
            var eventHandlerGenericType = typeof(IEventHandler<>);
            var eventHandlerType = eventHandlerGenericType.MakeGenericType(eventType);
            using var scope = _serviceProvider.CreateScope();
            var eventHandler = scope.ServiceProvider.GetService(eventHandlerType);
            var methodInfo = eventHandler.GetType().GetMethod("Handle");
            dynamic awaitable = methodInfo?.Invoke(eventHandler, new[] { @event });
            if (awaitable != null) await awaitable;
            await _subscriptionClient.CompleteAsync(message.SystemProperties.LockToken);
        };

        private async Task ExceptionReceivedHandler(ExceptionReceivedEventArgs exceptionReceivedEventArgs)
        {
            var exception = exceptionReceivedEventArgs.Exception;
            var context = exceptionReceivedEventArgs.ExceptionReceivedContext;
            var details = $"Endpoint={context.Endpoint}, EntityPath={context.EntityPath}, Action={context.Action}";
            if (exception is EventNotSupportedException)
            {
                await Console.Out.WriteLineAsync($"Warning: {exception.Message}. {details}");
            }
            else if (exceptionReceivedEventArgs.Exception.Message.Contains("status-code: 401"))
            {
                await Console.Out.WriteLineAsync($"Authentication Error. The serviceBus token has expired!");
                await Console.Out.WriteLineAsync($"Refreshing serviceBus token. Details. {details}");
                await RefreshSubscriptionClient();
            }
            else
            {
                throw new Exceptions.ServiceBusException($"Could not handle message. {details}", exception);
            }
        }

        private MessageHandlerOptions MessageHandlerOptions =>
            new MessageHandlerOptions(ExceptionReceivedHandler)
            {
                MaxConcurrentCalls = 1,
                AutoComplete = false
            };
    }
}