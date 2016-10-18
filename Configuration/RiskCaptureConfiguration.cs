namespace AppliedSystems.RiskCapture.Configuration
{
    using AppliedSystems.Configuration;

    public class RiskCaptureConfiguration
    {
        public static RiskCaptureConfiguration FromAppConfig()
        {
            var configuration = ConfigurationReader.Read<RiskCaptureConfiguration>();

            if (string.IsNullOrEmpty(configuration.Url))
            {
                throw new RiskCaptureConfigurationException();
            }

            return configuration;
        }

        public string Url { get; set; }
    }
}