namespace AppliedSystems.Infrastucture.Messaging.EventStore.Bootstrapping
{
    using SystemDot.Ioc;
    using Core;

    public static class IocContainerExtensions
    {

        public static IEventStoreEndpointBuilder<TEventStoreEndpoint> ResolveEventStoreEndpointBuilder<TEventStoreEndpoint>(this IIocResolver container, TEventStoreEndpoint toConfigure)
            where TEventStoreEndpoint : IEventStoreEndpoint
        {
            var builder = container.Resolve(toConfigure.EndpointBuilderType).As<IEventStoreEndpointBuilder<TEventStoreEndpoint>>();
            if (builder == null)
            {
                throw new EventStoreEndpointHasInvalidBuilderException(toConfigure);
            }
            return builder;
        }
    }
}