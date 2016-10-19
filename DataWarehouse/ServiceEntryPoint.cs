namespace AppliedSystems.DataWarehouse
{
    using System;
    using System.Diagnostics;
    using SystemDot.Bootstrapping;
    using SystemDot.Ioc;
    using AppliedSystems.Data.Connections;
    using AppliedSystems.Infrastucture.Data;
    using AppliedSystems.Infrastucture.Messaging.EventStore;
    using AppliedSystems.Infrastucture.Messaging.EventStore.Bootstrapping;
    using AppliedSystems.Infrastucture.Messaging.EventStore.Configuration;
    using AppliedSystems.Infrastucture.Messaging.EventStore.Subscribing;
    using AppliedSystems.Infrastucture.Messaging.Sagas;
    using Bootstrapping;
    using Core;
    using Data.Bootstrapping;
    using Messaging.Infrastructure.Bootstrapping;
    using Topshelf;

    class ServiceEntryPoint
    {
        static void Main()
        {
            HostFactory.Run(configurator =>
            {
                var container = new IocContainer(t => t.NameInCSharp());

                var eventSubscriptionConfig = EventStoreSubscriptionConfiguration.FromAppConfig();

                var eventStoreSubscriptionEndpoint = EventStoreSubscriptionEndpoint
                    .ListenTo(EventStoreUrl.Parse(eventSubscriptionConfig.Url))
                    .WithCredentials(
                        EventStoreUserCredentials.Parse(
                            eventSubscriptionConfig.UserCredentials.User,
                            eventSubscriptionConfig.UserCredentials.Password));

                Bootstrap.Application()
                    .ResolveReferencesWith(container)
                    .RegisterBuildAction(c => c.RegisterInstance<IConnectionFactory, SqlConnectionFactory>())
                    .SetupData()
                    .SetupMessaging()
                        .ConfigureSagas().WithDatabasePersistence()
                        .ConfigureEventStoreSubscriber<SqlEventIndexStore>()
                        .ConfigureReceivingEndpoint(eventStoreSubscriptionEndpoint)
                        .ConfigureMessageRouting().WireUpRouting()
                    .Initialise();

                Trace.TraceInformation(
                    "Data warehouse service has started up and the types registered into the container are {0}{1}",
                    Environment.NewLine,
                    container.Describe());

                configurator.Service<DataWarehouseController>(s =>
                {
                    s.ConstructUsing(name => container.Resolve<DataWarehouseController>());
                    s.WhenStarted(c => c.Start());
                    s.WhenStopped(c => c.Stop());
                });

                configurator.RunAsLocalSystem();
                configurator.SetDescription("Applied Systems Data Warehouse Service");
                configurator.SetDisplayName("Applied Systems Data Warehouse Service");
                configurator.SetServiceName("Applied Systems Data Warehouse Service");
            });
        }

    }
}
