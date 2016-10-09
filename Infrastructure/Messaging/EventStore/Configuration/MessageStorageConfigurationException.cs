namespace AppliedSystems.RiskCapture.Infrastucture.Messaging.EventStore.Configuration
{
    using System;
    using System.Runtime.Serialization;
    using AppliedSystems.Configuration;

    [Serializable]
    public class MessageStorageConfigurationException : ConfigurationReadingException
    {
        public override Type ConfigurationType => typeof(MessageStorageConfiguration);

        public MessageStorageConfigurationException()
        {
        }

        public MessageStorageConfigurationException(string message)
            : base(message)
        {
        }

        public MessageStorageConfigurationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected MessageStorageConfigurationException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}