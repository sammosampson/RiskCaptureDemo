namespace AppliedSystems.DataWarehouse
{
    using System;
    using System.Diagnostics;
    using SystemDot.Bootstrapping;
    using SystemDot.Ioc;
    using AppliedSystems.Data.Connections;
    using AppliedSystems.DataWarehouse.Configuration;
    using AppliedSystems.Infrastucture.Data;
    using AppliedSystems.Infrastucture.Messaging.EventStore;
    using AppliedSystems.Infrastucture.Messaging.EventStore.Bootstrapping;
    using AppliedSystems.Infrastucture.Messaging.EventStore.Configuration;
    using AppliedSystems.Infrastucture.Messaging.EventStore.Subscribing;
    using AppliedSystems.Infrastucture.Messaging.Http;
    using AppliedSystems.Infrastucture.Messaging.Sagas;
    using AppliedSystems.Messaging.Http;
    using AppliedSystems.Messaging.Infrastructure.Pipelines;
    using AppliedSystems.Messaging.Infrastructure.Requests;
    using AppliedSystems.Messaging.Infrastructure.Requests.Outgoing.InProcess;
    using AppliedSystems.Messaging.Infrastructure.Sagas.Bootstrapping;
    using AppliedSystems.Messaging.Messages;
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
                    .RegisterBuildAction(c => c.RegisterInstance<IConnectionFactory, SqlConnectionFactory>())
                    .SetupData()
                    .SetupMessaging()
                        .ConfigureSagas().WithDatabasePersistence()
                        .ConfigureEventStoreSubscriber<SqlEventIndexStore>()
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
