namespace ServiceBus.Client.Factories
{
    using Configuration;
    using Contracts.Factories;
    using Exceptions;
    using global::Client.Abstractions;
    using Microsoft.Azure.ServiceBus;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;

    public class EventFactory
        : IEventFactory
    {
        private readonly Dictionary<string, Type> _eventTypes;

        public EventFactory(Assembly[] assemblies) 
        {
            _eventTypes = LoadEventTypes(assemblies);
        }

        public object Create(string qualifiedName, Message message)
        {
            var type = default(Type);
            if(!_eventTypes.TryGetValue(qualifiedName, out type))
            {
                throw new KeyNotFoundException();
            }
                        
            var json = Encoding.UTF8.GetString(message.Body);
            var @event = JsonConvert.DeserializeObject(json, type);
            return @event;
        }

        private Dictionary<string, Type> LoadEventTypes(Assembly[] assemblies)
        {
            var discoveredEventHandlerInterfaces = assemblies
                .SelectMany(a => a.DefinedTypes, (a, ti) => ti.GetInterfaces())
                .Where(t => t.Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEventHandler<>)));

            var discoveredEventTypes = discoveredEventHandlerInterfaces
                .SelectMany(i => i, (i, t) => t.GenericTypeArguments.First());

            return discoveredEventTypes
                .ToDictionary(k => k.FullName, v => v);
        }
    }
}
