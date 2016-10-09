namespace AppliedSystems.RiskCapture.Service
{
    using System;
    using System.Diagnostics;
    using System.Security.Policy;
    using SystemDot.Bootstrapping;
    using SystemDot.Ioc;
    using AppliedSystems.Core;
    using AppliedSystems.Data.Bootstrapping;
    using AppliedSystems.Messaging.Data.Bootstrapping;
    using AppliedSystems.Messaging.Http.Receiving;
    using AppliedSystems.Messaging.Infrastructure.Bootstrapping;
    using AppliedSystems.RiskCapture.Infrastucture.Messaging;
    using AppliedSystems.RiskCapture.Infrastucture.Messaging.EventStore;
    using AppliedSystems.RiskCapture.Infrastucture.Messaging.EventStore.Bootstrapping;
    using AppliedSystems.RiskCapture.Infrastucture.Messaging.EventStore.Configuration;
    using AppliedSystems.RiskCapture.Infrastucture.Messaging.Http;
    using AppliedSystems.RiskCapture.Infrastucture.Messaging.Http.Bootstrapping;
    using AppliedSystems.RiskCapture.Infrastucture.Messaging.Http.Configuration;
    using AppliedSystems.RiskCapture.Service.Bootstrapping;
    using Topshelf;

    class ServiceEntryPoint
    {
        static void Main()
        {
            var config = RiskCaptureHttpMessageReceivingConfiguration.FromAppConfig();

            var receivePoint = RiskCaptureHttpReceivePoint.ListenOn(HttpMessagingReceiverUrl.Parse(config.Url));

            var eventStorageConfig = MessageStorageConfiguration.FromAppConfig();
            var eventStoreEndpoint = new EventStoreEventDispatchingEndpoint();

            HostFactory.Run(configurator =>
            {
                var container = new IocContainer(t => t.NameInCSharp());

                Bootstrap.Application()
                    .ResolveReferencesWith(container)
                    .ConfigureRiskCapture()
                    .SetupData()
                    .SetupMessaging()
                        .ConfigureHttpMessaging<IncomingMessageConverter>()
                        .ConfigureEventStore(
                            MessageStorageMode.Parse(eventStorageConfig.StorageMode),
                            MessageStorageUrl.Parse(eventStorageConfig.Url),
                            MessageStorageUserCredentials.Parse(
                                eventStorageConfig.UserCredentials.User,
                                eventStorageConfig.UserCredentials.Password))
                        .ConfigureSagas().WithDatabasePersistence()
                        .ConfigureReceivingEndpoint(receivePoint)
                        .ConfigureEventDispatchingEndpoint(eventStoreEndpoint)
                        .ConfigureMessageRouting().WireUpRouting(eventStoreEndpoint)
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
