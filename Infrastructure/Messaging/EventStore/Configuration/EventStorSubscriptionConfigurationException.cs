namespace AppliedSystems.Infrastucture.Messaging.EventStore.Configuration
{
    using System;
    using System.Runtime.Serialization;
    using AppliedSystems.Configuration;

    [Serializable]
    public class EventStorSubscriptionConfigurationException : ConfigurationReadingException
    {
        public override Type ConfigurationType => typeof(EventStoreSubscriptionConfiguration);

        public EventStorSubscriptionConfigurationException()
        {
        }

        public EventStorSubscriptionConfigurationException(string message)
            : base(message)
        {
        }

        public EventStorSubscriptionConfigurationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected EventStorSubscriptionConfigurationException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}