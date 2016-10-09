﻿namespace AppliedSystems.Infrastucture.Messaging.Http.Bootstrapping
{
    using AppliedSystems.Messaging.Infrastructure.Bootstrapping;

    public static class MessagingConfigurationExtensions
    {
        public static MessagingConfiguration ConfigureHttpMessaging<TIncomingMessageConverter>(this MessagingConfiguration config)
            where TIncomingMessageConverter : class, IIncomingMessageConverter
        {
            config.RegisterBuildAction(c => c.RegisterHttpMessaging<TIncomingMessageConverter>());
            return config;
        }
    }
}