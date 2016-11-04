namespace AppliedSystems.Documents.Process
{
    using System;
    using System.Diagnostics;
    using SystemDot.Bootstrapping;
    using SystemDot.Ioc;
    using AppliedSystems.Core;
    using AppliedSystems.Data.Bootstrapping;
    using AppliedSystems.Documents.Process.Bootstrapping;
    using AppliedSystems.Documents.Process.Configuration;
    using AppliedSystems.Infrastucture.Messaging.Http;
    using AppliedSystems.Messaging.Data.Bootstrapping;
    using AppliedSystems.Messaging.EventStore;
    using AppliedSystems.Messaging.EventStore.Configuration;
    using AppliedSystems.Messaging.EventStore.Subscribing;
    using AppliedSystems.Messaging.Infrastructure.Bootstrapping;
    using Topshelf;

    class ServiceEntryPoint
    {
        static void Main()
        {
            DocumentsProcessConfiguration config = DocumentsProcessConfiguration.FromAppConfig();
            HttpCommandDispatchingEndpoint documentsEndpoint = HttpCommandDispatchingEndpoint.FromUrl(config.DocumentsUrl);
            HttpRequestDispatcherEndpoint riskCaptureRequestEndpoint = HttpRequestDispatcherEndpoint.ForUrl(config.RiskCaptureUrl);

            var eventSubscriptionConfig = EventStoreSubscriptionConfiguration.FromAppConfig();

            var eventStorageConfig = EventStoreMessageStorageConfiguration.FromAppConfig();

            var eventStoreEndpoint = EventStoreEndpoint
                .OnUrl(EventStoreUrl.Parse(eventStorageConfig.Url))
                .WithCredentials(
                    EventStoreUserCredentials.Parse(
                        eventStorageConfig.UserCredentials.User,
                        eventStorageConfig.UserCredentials.Password));

            var eventStoreSubscriptionEndpoint = EventStoreSubscriptionEndpoint
                .ListenTo(EventStoreUrl.Parse(eventSubscriptionConfig.Url))
                .WithCredentials(
                    EventStoreUserCredentials.Parse(
                        eventSubscriptionConfig.UserCredentials.User,
                        eventSubscriptionConfig.UserCredentials.Password));


            HostFactory.Run(configurator =>
            {
                var container = new IocContainer(t => t.NameInCSharp());

                Bootstrap.Application()
                    .ResolveReferencesWith(container)
                    .SetupDataConnectivity().WithSqlConnection()
                    .SetupMessaging()
                        .ConfigureSagas().WithDatabasePersistence()
                        .ConfigureEventIndexStorage().WithEventStorePersistence()
                        .ConfigureEventStoreEndpoint(eventStoreEndpoint)
                        .ConfigureReceivingEndpoint(eventStoreSubscriptionEndpoint)
                        .ConfigureRequestDispatchingEndpoint(riskCaptureRequestEndpoint)
                        .ConfigureCommandDispatchingEndpoint(documentsEndpoint)
                        .ConfigureMessageRouting().WireUpRouting(documentsEndpoint, riskCaptureRequestEndpoint)
                    .Initialise();

                Trace.TraceInformation(
                    "Process service has started up and the types registered into the container are {0}{1}",
                    Environment.NewLine,
                    container.Describe());

                configurator.Service<ProcessController>(s =>
                {
                    s.ConstructUsing(name => container.Resolve<ProcessController>());
                    s.WhenStarted(c => c.Start());
                    s.WhenStopped(c => c.Stop());
                });

                configurator.RunAsLocalSystem();
                configurator.SetDescription("Applied Systems Documents Process Service");
                configurator.SetDisplayName("Applied Systems Documents Process Service");
                configurator.SetServiceName("Applied Systems Documents Process Service");
            });
        }
    }
}
