namespace AppliedSystems.Infrastucture.Messaging.EventStore.Bootstrapping
{
    using AppliedSystems.Infrastucture.Messaging.EventStore.Subscribing;
    using AppliedSystems.Messaging.Infrastructure.Bootstrapping;
    using EventSourcing;

    public static class MessagingConfigurationExtensions
    {
        public static MessagingConfiguration ConfigureEventStoreSubscriber<TEventIndexStore>(this MessagingConfiguration config) 
            where TEventIndexStore : class, IEventIndexStore
        {
            config.RegisterBuildAction(c => c.RegisterInstance<IEventIndexStore, TEventIndexStore>());
            return config;
        }

        public static MessagingConfiguration ConfigureEventStoreEndpoint<TEventStoreEndpoint>(this MessagingConfiguration config, TEventStoreEndpoint toConfigure)
            where TEventStoreEndpoint : IEventStoreEndpoint
        {
            config
                .RegisterBuildAction(c =>
                {
                    var builder = c.ResolveEventStoreEndpointBuilder(toConfigure);
                    c.RegisterInstance<IEventStore>(() => builder.BuildEventStore(toConfigure, c.GetEventSendingPipeline()));
                    c.RegisterInstance<IProjectionStore>(() => builder.BuildProjectionStore(toConfigure));
                });
            return config;
        }
    }
}
