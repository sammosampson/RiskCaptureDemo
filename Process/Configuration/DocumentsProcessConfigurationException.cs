namespace AppliedSystems.Documents.Process.Configuration
{
    using System;
    using System.Runtime.Serialization;
    using AppliedSystems.Configuration;

    [Serializable]
    public class DocumentsProcessConfigurationException : ConfigurationReadingException
    {
        public override Type ConfigurationType => typeof(DocumentsProcessConfiguration);

        public DocumentsProcessConfigurationException()
        {
        }

        public DocumentsProcessConfigurationException(string message)
            : base(message)
        {
        }

        public DocumentsProcessConfigurationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected DocumentsProcessConfigurationException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}