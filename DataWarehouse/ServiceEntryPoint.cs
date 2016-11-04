namespace AppliedSystems.DataWarehouse
{
    using System;
    using System.Diagnostics;
    using SystemDot.Bootstrapping;
    using SystemDot.Ioc;
    using AppliedSystems.DataWarehouse.Configuration;
    using AppliedSystems.Infrastucture.Data;
    using AppliedSystems.Infrastucture.Messaging.Http;
    using AppliedSystems.Messaging.Data.Bootstrapping;
    using AppliedSystems.Messaging.EventStore;
    using AppliedSystems.Messaging.EventStore.Configuration;
    using AppliedSystems.Messaging.EventStore.Subscribing;
    using AppliedSystems.Messaging.Infrastructure.Sagas;
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

                var riskCaptureRequestEndpoint = HttpRequestDispatcherEndpoint.ForUrl(DataWarehousingConfiguration.FromAppConfig().RiskCaptureUrl);

                Bootstrap.Application()
                    .ResolveReferencesWith(container)
                    .SetupDataConnectivity().WithSqlConnection()
                    .SetupMessaging()
                    .ConfigureSagas().WithDatabasePersistence()
                    .ConfigureEventIndexStorage().WithDatabasePersistence()
                    .ConfigureReceivingEndpoint(eventStoreSubscriptionEndpoint)
                    .ConfigureRequestDispatchingEndpoint(riskCaptureRequestEndpoint)
                    .ConfigureMessageRouting().WireUpRouting(riskCaptureRequestEndpoint)
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
