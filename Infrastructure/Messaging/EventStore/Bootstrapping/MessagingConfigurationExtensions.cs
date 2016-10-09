namespace AppliedSystems.RiskCapture.Infrastucture.Messaging.EventStore.Bootstrapping
{
    using AppliedSystems.Messaging.Infrastructure.Bootstrapping;

    public static class MessagingConfigurationExtensions
    {
        public static MessagingConfiguration ConfigureEventStore(this MessagingConfiguration config, MessageStorageMode @in, MessageStorageUrl url, MessageStorageUserCredentials credentials)
        {
            if (@in == MessageStorageMode.Persistent)
            {
                config.RegisterBuildAction(c => c.RegisterEventStoreEventStorage(url, credentials));
            }
            if (@in == MessageStorageMode.NonPersistent)
            {
                config.RegisterBuildAction(c => c.RegisterInMemoryEventStorage());
            }
            return config;
        }
    }
}