namespace AppliedSystems.DataWarehouse
{
    using System;
    using System.Diagnostics;
    using SystemDot.Bootstrapping;
    using SystemDot.Ioc;
    using AppliedSystems.Data.Connections;
    using AppliedSystems.Infrastucture.Data;
    using AppliedSystems.Infrastucture.Messaging.EventStore.Subscribing;
    using Bootstrapping;
    using Core;
    using Data.Bootstrapping;
    using Messaging.Data.Bootstrapping;
    using Messaging.Infrastructure.Bootstrapping;
    using Infrastucture.Messaging.EventStore;
    using Infrastucture.Messaging.EventStore.Bootstrapping;
    using Infrastucture.Messaging.EventStore.Configuration;
    using Messaging.Infrastructure.Commands;
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
                    "Data warehouse service has started up and the types registered into the container are {0}{1}",
                    Environment.NewLine,
                    container.Describe());

                configurator.Service<DataWareHouseController>(s =>
                {
                    s.ConstructUsing(name => container.Resolve<DataWareHouseController>());
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
