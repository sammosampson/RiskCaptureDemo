namespace AppliedSystems.Documents.Configuration
{
    using System;
    using System.Runtime.Serialization;
    using AppliedSystems.Configuration;

    [Serializable]
    public class DocumentsConfigurationException : ConfigurationReadingException
    {
        public override Type ConfigurationType => typeof(DocumentsConfiguration);

        public DocumentsConfigurationException()
        {
        }

        public DocumentsConfigurationException(string message)
            : base(message)
        {
        }

        public DocumentsConfigurationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected DocumentsConfigurationException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}