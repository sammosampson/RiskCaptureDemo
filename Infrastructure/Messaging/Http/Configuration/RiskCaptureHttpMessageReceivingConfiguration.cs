namespace AppliedSystems.Infrastucture.Messaging.Http.Configuration
{
    using AppliedSystems.Configuration;

    public class RiskCaptureHttpMessageReceivingConfiguration
    {
        public static RiskCaptureHttpMessageReceivingConfiguration FromAppConfig()
        {
            var configuration = ConfigurationReader.Read<RiskCaptureHttpMessageReceivingConfiguration>();

            if (string.IsNullOrEmpty(configuration.Url))
            {
                throw new RiskCaptureHttpMessageReceivingConfigurationException();
            }

            return configuration;
        }

        public string Url { get; set; }
    }
}