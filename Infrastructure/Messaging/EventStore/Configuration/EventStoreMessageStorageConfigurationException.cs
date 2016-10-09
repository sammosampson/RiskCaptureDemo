namespace AppliedSystems.Infrastucture.Messaging.EventStore.Configuration
{
    using System;
    using System.Runtime.Serialization;
    using AppliedSystems.Configuration;

    [Serializable]
    public class EventStoreMessageStorageConfigurationException : ConfigurationReadingException
    {
        public override Type ConfigurationType => typeof(EventStoreMessageStorageConfiguration);

        public EventStoreMessageStorageConfigurationException()
        {
        }

        public EventStoreMessageStorageConfigurationException(string message)
            : base(message)
        {
        }

        public EventStoreMessageStorageConfigurationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected EventStoreMessageStorageConfigurationException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}