namespace AppliedSystems.Documents.Process
{
    using System;
    using System.Diagnostics;
    using SystemDot.Bootstrapping;
    using SystemDot.Ioc;
    using AppliedSystems.Core;
    using AppliedSystems.Data.Bootstrapping;
    using AppliedSystems.Data.Connections;
    using AppliedSystems.Documents.Process.Bootstrapping;
    using AppliedSystems.Documents.Process.Configuration;
    using AppliedSystems.Infrastucture.Data;
    using AppliedSystems.Infrastucture.Messaging.EventStore;
    using AppliedSystems.Infrastucture.Messaging.EventStore.Bootstrapping;
    using AppliedSystems.Infrastucture.Messaging.EventStore.Configuration;
    using AppliedSystems.Infrastucture.Messaging.EventStore.Subscribing;
    using AppliedSystems.Infrastucture.Messaging.Http;
    using AppliedSystems.Messaging.Infrastructure.Bootstrapping;
    using AppliedSystems.Messaging.Infrastructure.Sagas.Bootstrapping;
    using Topshelf;

    class ServiceEntryPoint
    {
        static void Main()
        {
            DocumentsProcessConfiguration config = DocumentsProcessConfiguration.FromAppConfig();
            HttpCommandDispatchingEndpoint documentsEndpoint = HttpCommandDispatchingEndpoint.FromUrl(config.DocumentsUrl);

            var eventSubscriptionConfig = EventStoreSubscriptionConfiguration.FromAppConfig();

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
                    .RegisterBuildAction(c => c.RegisterInstance<IConnectionFactory, SqlConnectionFactory>())
                    .SetupData()
                    .SetupMessaging()
                        .ConfigureSagas().WithInMemoryPersistence()
                        .ConfigureEventStoreSubscriber<SqlEventIndexStore>()
                        .ConfigureReceivingEndpoint(eventStoreSubscriptionEndpoint)
                        .ConfigureCommandDispatchingEndpoint(documentsEndpoint)
                        .ConfigureMessageRouting().WireUpRouting(documentsEndpoint)
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
                configurator.SetDescription("Applied Systems Risk Capture Process Service");
                configurator.SetDisplayName("Applied Systems Risk Capture Process Service");
                configurator.SetServiceName("Applied Systems Risk Capture Process Service");
            });
        }
    }
}
