namespace AppliedSystems.RiskCapture
{
    using System;
    using System.Diagnostics;
    using SystemDot.Bootstrapping;
    using SystemDot.Ioc;
    using AppliedSystems.Messaging.EventStore.GES;
    using AppliedSystems.Messaging.EventStore.GES.Configuration;
    using AppliedSystems.Messaging.Infrastructure.Events.Streams;
    using AppliedSystems.RiskCapture.Messages;
    using Messaging.Infrastructure.Receiving;
    using Configuration;
    using Bootstrapping;
    using Core;
    using Messaging.Infrastructure.Bootstrapping;
    using Topshelf;

    class ServiceEntryPoint
    {
        static void Main()
        {
            var config = RiskCaptureConfiguration.FromAppConfig();
            var eventStorageConfig = EventStoreMessageStorageConfiguration.FromAppConfig();

            var eventStoreEndpoint = EventStoreEndpoint
                .OnUrl(EventStoreUrl.Parse(eventStorageConfig.Url))
                .WithCredentials(
                    EventStoreUserCredentials.Parse(
                        eventStorageConfig.UserCredentials.User,
                        eventStorageConfig.UserCredentials.Password))
                .WithEventTypeFromNameResolution(EventTypeFromNameResolver.FromTypesFromAssemblyContaining<NewRiskItemMapped>());

            HostFactory.Run(configurator =>
            {
                var container = new IocContainer(t => t.NameInCSharp());

                Bootstrap.Application()
                    .ResolveReferencesWith(container)
                    .ConfigureRiskCapture()
                    .SetupMessaging()
                        .ConfigureEventStoreEndpoint(eventStoreEndpoint)
                        .ConfigureMessageRouting().WireUpRouting()
                    .Initialise();

                Trace.TraceInformation(
                    "Risk Capture service has started up and the types registered into the container are {0}{1}",
                    Environment.NewLine,
                    container.Describe());

                configurator.Service<RiskCaptureController>(s =>
                {
                    s.ConstructUsing(name => new RiskCaptureController(config.Url, container.Resolve<IMessageReceiver>()));
                    s.WhenStarted(c => c.Start());
                    s.WhenStopped(c => c.Stop());
                });

                configurator.RunAsLocalSystem();
                configurator.SetDescription("Applied Systems Risk Capture Service");
                configurator.SetDisplayName("Applied Systems Risk Capture Service");
                configurator.SetServiceName("Applied Systems Risk Capture Service");
            });
        }
    }
}
