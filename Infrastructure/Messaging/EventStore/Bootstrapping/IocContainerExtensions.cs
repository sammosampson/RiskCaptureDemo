
namespace AppliedSystems.RiskCapture.Infrastucture.Messaging.EventStore.Bootstrapping
{
    using SystemDot.Ioc;
    using Messaging.EventSourcing;
    using EventSourcing;
    using Reading;
    using Reading.NonPersistent;
    using Reading.Persistent;
    using Writing;
    using Writing.NonPersistent;
    using Writing.Persistent;

    public static class IocContainerExtensions
    {
        public static void RegisterEventStoreEventStorage(this IIocContainer container, MessageStorageUrl url,
            MessageStorageUserCredentials credentials)
        {
            container.RegisterInstance(() => url);
            container.RegisterInstance(() => credentials);
            container.RegisterInstance<IEventStoreConnectionFactory, EventStoreConnectionFactory>();
            container.RegisterInstance<IWriteEventStreamConnector, PersistentWriteEventStreamConnector>();
            container.RegisterInstance<IReadEventStreamConnector, PersistentReadEventStreamConnector>();
            container.RegisterInstance<IEventRetreiver, EventStoreEventRetreiver>();
            container.RegisterInstance<IMessageDeliverer, NullMessageDeliverer>();
        }

        public static void RegisterInMemoryEventStorage(this IIocContainer container)
        {
            container.RegisterInstance<IWriteEventStreamConnector, NonPersistentWriteEventStreamConnector>();
            container.RegisterInstance<IReadEventStreamConnector, NonPersistentReadEventStreamConnector>();
            container.Resolve<NonPersistentEventStore>().EventAppended +=
                (_, args) => container.Resolve<IMessageDeliverer>().Deliver(args.StreamName, args.Event);
        }
    }
}