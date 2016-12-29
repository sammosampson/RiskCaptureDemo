namespace AppliedSystems.Documents
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using SystemDot.Bootstrapping;
    using SystemDot.Ioc;
    using AppliedSystems.Core;
    using AppliedSystems.Data.Bootstrapping;
    using AppliedSystems.Documents.Bootstrapping;
    using AppliedSystems.Infrastucture;
    using AppliedSystems.Messaging.Data.Bootstrapping;
    using AppliedSystems.Messaging.EventStore.GES;
    using AppliedSystems.Messaging.EventStore.GES.Configuration;
    using AppliedSystems.Messaging.EventStore.GES.Subscribing;
    using AppliedSystems.Messaging.Infrastructure;
    using AppliedSystems.Messaging.Infrastructure.Bootstrapping;
    using AppliedSystems.Messaging.Infrastructure.Events;
    using AppliedSystems.Messaging.Infrastructure.Events.Streams;
    using AppliedSystems.Messaging.Infrastructure.Pipelines;
    using AppliedSystems.Messaging.Infrastructure.Receiving;
    using AppliedSystems.RiskCapture.Messages;
    using Microsoft.Owin.Cors;
    using Microsoft.Owin.Hosting;
    using Nancy;
    using Nancy.Owin;
    using Nancy.TinyIoc;
    using Owin;
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