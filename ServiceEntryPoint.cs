namespace AppliedSystems.RiskCapture
{
    using System;
    using System.Diagnostics;
    using SystemDot.Bootstrapping;
    using SystemDot.Ioc;
    using Bootstrapping;
    using Core;
    using Data.Bootstrapping;
    using Infrastucture.Messaging.EventStore;
    using Infrastucture.Messaging.EventStore.Bootstrapping;
    using Infrastucture.Messaging.EventStore.Configuration;
    using Infrastucture.Messaging.Http;
    using Infrastucture.Messaging.Http.Bootstrapping;
    using Infrastucture.Messaging.Http.Configuration;
    using Messaging.Http.Receiving;
    using Messaging.Infrastructure.Bootstrapping;
    using Topshelf;

    class ServiceEntryPoint
    {
        static void Main()
        {
            var config = RiskCaptureHttpMessageReceivingConfiguration.FromAppConfig();
            var receivePoint = RiskCaptureHttpReceivePoint.ListenOn(HttpMessagingReceiverUrl.Parse(config.Url));
            var eventStorageConfig = EventStoreMessageStorageConfiguration.FromAppConfig();

            var eventStoreEndpoint = EventStoreEndpoint
                .OnUrl(EventStoreUrl.Parse(eventStorageConfig.Url))
                .WithCredentials(
                    EventStoreUserCredentials.Parse(
                        eventStorageConfig.UserCredentials.User,
                        eventStorageConfig.UserCredentials.Password));

            HostFactory.Run(configurator =>
            {
                var container = new IocContainer(t => t.NameInCSharp());

                Bootstrap.Application()
                    .ResolveReferencesWith(container)
                    .ConfigureRiskCapture()
                    .SetupData()
                    .SetupMessaging()
                        .ConfigureHttpMessaging<IncomingMessageConverter>()    
                        .ConfigureReceivingEndpoint(receivePoint)
                        .ConfigureEventStoreEndpoint(eventStoreEndpoint)
                        .ConfigureMessageRouting().WireUpRouting()
                    .Initialise();

                Trace.TraceInformation(
                    "Risk Capture service has started up and the types registered into the container are {0}{1}",
                    Environment.NewLine,
                    container.Describe());

                configurator.Service<RiskCaptureController>(s =>
                {
                    s.ConstructUsing(name => container.Resolve<RiskCaptureController>());
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
