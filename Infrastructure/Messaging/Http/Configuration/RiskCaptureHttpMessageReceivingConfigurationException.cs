namespace AppliedSystems.Infrastucture.Messaging.Http.Configuration
{
    using System;
    using System.Runtime.Serialization;
    using AppliedSystems.Configuration;

    [Serializable]
    public class RiskCaptureHttpMessageReceivingConfigurationException : ConfigurationReadingException
    {
        public override Type ConfigurationType => typeof(RiskCaptureHttpMessageReceivingConfiguration);

        public RiskCaptureHttpMessageReceivingConfigurationException()
        {
        }

        public RiskCaptureHttpMessageReceivingConfigurationException(string message)
            : base(message)
        {
        }

        public RiskCaptureHttpMessageReceivingConfigurationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected RiskCaptureHttpMessageReceivingConfigurationException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}