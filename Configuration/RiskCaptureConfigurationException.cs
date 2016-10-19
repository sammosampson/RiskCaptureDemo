namespace AppliedSystems.RiskCapture.Configuration
{
    using System;
    using System.Runtime.Serialization;
    using AppliedSystems.Configuration;

    [Serializable]
    public class RiskCaptureConfigurationException : ConfigurationReadingException
    {
        public override Type ConfigurationType => typeof(RiskCaptureConfiguration);

        public RiskCaptureConfigurationException()
        {
        }

        public RiskCaptureConfigurationException(string message)
            : base(message)
        {
        }

        public RiskCaptureConfigurationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected RiskCaptureConfigurationException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}