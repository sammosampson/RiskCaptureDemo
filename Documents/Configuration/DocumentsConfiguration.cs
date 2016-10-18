namespace AppliedSystems.Documents.Configuration
{
    using AppliedSystems.Configuration;

    public class DocumentsConfiguration
    {
        public static DocumentsConfiguration FromAppConfig()
        {
            var configuration = ConfigurationReader.Read<DocumentsConfiguration>();

            if (string.IsNullOrEmpty(configuration.Url))
            {
                throw new DocumentsConfigurationException();
            }

            return configuration;
        }

        public string Url { get; set; }
    }
}