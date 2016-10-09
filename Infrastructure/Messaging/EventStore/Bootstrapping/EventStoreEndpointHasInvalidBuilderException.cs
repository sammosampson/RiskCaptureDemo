namespace AppliedSystems.Infrastucture.Messaging.EventStore.Bootstrapping
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.Serialization;

    [Serializable]
    public class EventStoreEndpointHasInvalidBuilderException : Exception
    {
        [SuppressMessage("Microsoft.Globalization", "CA1305:SpecifyIFormatProvider", MessageId = "System.String.Format(System.String,System.Object,System.Object,System.Object)", Justification = "There are no international issues to be considered in this text")]
        public EventStoreEndpointHasInvalidBuilderException(IEventStoreEndpoint endpoint) : this(
            $"Error whilst configuring the endpoint {endpoint.GetType().Name}. The endpoint BuilderType {endpoint.EndpointBuilderType.Name} should implement IEventStoreEndpointBuilder<{endpoint.GetType().Name}>")
        {
        }

        public EventStoreEndpointHasInvalidBuilderException(string message)
            : base(message)
        {
        }
        
        public EventStoreEndpointHasInvalidBuilderException()
        {
        }

        public EventStoreEndpointHasInvalidBuilderException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected EventStoreEndpointHasInvalidBuilderException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}