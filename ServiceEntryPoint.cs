namespace AppliedSystems.RiskCapture
{
    using System;
    using System.Diagnostics;
    using System.Net.Http;
    using SystemDot.Bootstrapping;
    using SystemDot.Ioc;
    using AppliedSystems.Messaging.Http.Receiving;
    using AppliedSystems.Messaging.Http.Receiving.Bootstrapping;
    using AppliedSystems.Messaging.Http.Receiving.Configuration;
    using AppliedSystems.Messaging.Infrastructure.Receiving;
    using AppliedSystems.RiskCapture.Configuration;
    using Bootstrapping;
    using Core;
    using Data.Bootstrapping;
    using Infrastucture.Messaging.EventStore;
    using Infrastucture.Messaging.EventStore.Bootstrapping;
    using Infrastucture.Messaging.EventStore.Configuration;
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
                        eventStorageConfig.UserCredentials.Password));

            var receiverConfig = HttpMessageReceivingConfiguration.FromAppConfig();

            HttpReceivePoint receivingEndpoint = HttpReceivePoint.ListenOn(HttpMessagingReceiverUrl.Parse(receiverConfig.Url));

            HostFactory.Run(configurator =>
            {
                var container = new IocContainer(t => t.NameInCSharp());

                Bootstrap.Application()
                    .ResolveReferencesWith(container)
                    .ConfigureRiskCapture()
                    .SetupData()
                    .SetupMessaging()
                        .SetupHttpMessageReceiving()
                        .ConfigureReceivingEndpoint(receivingEndpoint)
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
