namespace AppliedSystems.Documents
{
    using System;
    using System.Diagnostics;
    using SystemDot.Bootstrapping;
    using SystemDot.Ioc;
    using AppliedSystems.Core;
    using AppliedSystems.Data.Bootstrapping;
    using AppliedSystems.Data.Connections;
    using AppliedSystems.Documents.Bootstrapping;
    using AppliedSystems.Infrastucture.Data;
    using AppliedSystems.Infrastucture.Messaging.EventStore;
    using AppliedSystems.Infrastucture.Messaging.EventStore.Bootstrapping;
    using AppliedSystems.Infrastucture.Messaging.EventStore.Configuration;
    using AppliedSystems.Infrastucture.Messaging.EventStore.Subscribing;
    using AppliedSystems.Messaging.Infrastructure.Bootstrapping;
    using Topshelf;

    class ServiceEntryPoint
    {
        static void Main()
        {
            var config = EventStoreSubscriptionConfiguration.FromAppConfig();

            var eventStoreSubscriptionEndpoint = EventStoreSubscriptionEndpoint
                .ListenTo(EventStoreUrl.Parse(config.Url))
                .WithCredentials(
                    EventStoreUserCredentials.Parse(
                        config.UserCredentials.User,
                        config.UserCredentials.Password));

            HostFactory.Run(configurator =>
            {
                var container = new IocContainer(t => t.NameInCSharp());

                Bootstrap.Application()
                    .ResolveReferencesWith(container)
                    .RegisterBuildAction(c => c.RegisterInstance<IConnectionFactory, SqlConnectionFactory>())
                    .SetupData()
                    .SetupMessaging()
                        .ConfigureEventStoreSubscriber<SqlEventIndexStore>()
                        .ConfigureReceivingEndpoint(eventStoreSubscriptionEndpoint)
                        .ConfigureMessageRouting().WireUpRouting()
                    .Initialise();

                Trace.TraceInformation(
                    "Documents service has started up and the types registered into the container are {0}{1}",
                    Environment.NewLine,
                    container.Describe());

                configurator.Service<DocumentsController>(s =>
                {
                    s.ConstructUsing(name => container.Resolve<DocumentsController>());
                    s.WhenStarted(c => c.Start());
                    s.WhenStopped(c => c.Stop());
                });

                configurator.RunAsLocalSystem();
                configurator.SetDescription("Applied Systems Documents Service");
                configurator.SetDisplayName("Applied Systems Documents Service");
                configurator.SetServiceName("Applied Systems Documents Service");
            });
        }
    }
}
