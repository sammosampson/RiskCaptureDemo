namespace AppliedSystems.Documents.Process.Configuration
{
    using AppliedSystems.Configuration;

    public class DocumentsProcessConfiguration
    {
        public static DocumentsProcessConfiguration FromAppConfig()
        {
            var configuration = ConfigurationReader.Read<DocumentsProcessConfiguration>();

            if (string.IsNullOrEmpty(configuration.DocumentsUrl))
            {
                throw new DocumentsProcessConfigurationException();
            }

            return configuration;
        }

        public string DocumentsUrl { get; set; }
    }
}