namespace AppliedSystems.Infrastucture.Messaging.EventStore.Bootstrapping
{
    using AppliedSystems.Messaging.Infrastructure.Bootstrapping;
    using EventSourcing;

    public static class MessagingConfigurationExtensions
    {
        public static MessagingConfiguration ConfigureEventStoreEndpoint<TEventStoreEndpoint>(this MessagingConfiguration config, TEventStoreEndpoint toConfigure)
            where TEventStoreEndpoint : IEventStoreEndpoint
        {
            config.RegisterBuildAction(c => c.RegisterInstance<IEventStore>(() => c.ResolveEventStoreEndpointBuilder(toConfigure).Build(toConfigure, c.GetEventSendingPipeline())));
            return config;
        }
    }
}
