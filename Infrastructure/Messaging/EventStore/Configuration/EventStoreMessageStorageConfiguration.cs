namespace AppliedSystems.Infrastucture.Messaging.EventStore.Configuration
{
    using System.Diagnostics.CodeAnalysis;
    using AppliedSystems.Configuration;
    using CodeAnalysis;
    using Core;

    public class EventStoreMessageStorageConfiguration
    {
        public static EventStoreMessageStorageConfiguration FromAppConfig()
        {
            EventStoreMessageStorageConfiguration configuration = ConfigurationReader.Read<EventStoreMessageStorageConfiguration>();

            if (configuration.DoesNotExist()
                || configuration.Url.IsEmpty()
                || configuration.StorageMode.IsEmpty()
                || configuration.UserCredentials == null
                || !configuration.UserCredentials.IsValid())
            {
                throw new EventStoreMessageStorageConfigurationException();
            }

            return configuration;
        }

        [SuppressMessage(FxCop.Design, "CA1056:UriPropertiesShouldNotBeStrings", Justification = "Has to be string for config to work")]
        public string Url { get; set; }

        public string StorageMode { get; set; }

        public EventStoreUserCredentialsConfiguration UserCredentials { get; set; }
    }
}