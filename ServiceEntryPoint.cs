namespace AppliedSystems.RiskCapture.Service
{
    using System;
    using System.Diagnostics;
    using SystemDot.Bootstrapping;
    using SystemDot.Ioc;
    using Core;
    using Data.Bootstrapping;
    using Messaging.Data.Bootstrapping;
    using Messaging.Http.Receiving;
    using Messaging.Infrastructure.Bootstrapping;
    using Bootstrapping;
    using Infrastucture.Messaging.EventStore;
    using Infrastucture.Messaging.EventStore.Bootstrapping;
    using Infrastucture.Messaging.EventStore.Configuration;
    using Infrastucture.Messaging.Http;
    using Infrastucture.Messaging.Http.Bootstrapping;
    using Infrastucture.Messaging.Http.Configuration;
    using Topshelf;

    class ServiceEntryPoint
    {
        static void Main()
        {
            var config = RiskCaptureHttpMessageReceivingConfiguration.FromAppConfig();
            var receivePoint = RiskCaptureHttpReceivePoint.ListenOn(HttpMessagingReceiverUrl.Parse(config.Url));
            var eventStorageConfig = MessageStorageConfiguration.FromAppConfig();

            var eventStoreEndpoint = EventStoreEndpoint
                .OnUrl(MessageStorageUrl.Parse(eventStorageConfig.Url))
                .WithCredentials(
                    MessageStorageUserCredentials.Parse(
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
                        .ConfigureSagas().WithDatabasePersistence()
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
