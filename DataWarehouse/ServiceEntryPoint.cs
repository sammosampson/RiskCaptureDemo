namespace AppliedSystems.DataWarehouse
{
    using System;
    using System.Diagnostics;
    using SystemDot.Bootstrapping;
    using SystemDot.Ioc;
    using AppliedSystems.DataWarehouse.Bootstrapping;
    using AppliedSystems.Messaging.Data.Bootstrapping;
    using AppliedSystems.Messaging.EventStore.GES;
    using AppliedSystems.Messaging.EventStore.GES.Configuration;
    using AppliedSystems.Messaging.EventStore.GES.Subscribing;
    using AppliedSystems.Messaging.Infrastructure.Events.Streams;
    using AppliedSystems.RiskCapture.Messages;
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

                var eventStoreUrl = EventStoreUrl.Parse(eventSubscriptionConfig.Url);

                var eventStoreUserCredentials = EventStoreUserCredentials.Parse(
                    eventSubscriptionConfig.UserCredentials.User,
                    eventSubscriptionConfig.UserCredentials.Password);

                var eventTypeResolution = EventTypeFromNameResolver.FromTypesFromAssemblyContaining<NewRiskItemMapped>();

                var eventStoreEndpoint = EventStoreEndpoint
                    .OnUrl(eventStoreUrl)
                    .WithCredentials(eventStoreUserCredentials)
                    .WithEventTypeFromNameResolution(eventTypeResolution);

                var eventStoreSubscriptionEndpoint = EventStoreSubscriptionEndpoint
                    .ListenTo(eventStoreUrl)
                    .WithCredentials(eventStoreUserCredentials)
                    .WithEventTypeFromNameResolution(eventTypeResolution)
                    .WithSqlDatabaseEventIndexStorage();
                
                Bootstrap.Application()
                    .ResolveReferencesWith(container)
                    .SetupDataConnectivity().WithSqlConnection()
                    .SetupMessaging()
                    .ConfigureSagas().WithDatabasePersistence()
                    .ConfigureEventStoreEndpoint(eventStoreEndpoint)
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
