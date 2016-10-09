namespace AppliedSystems.RiskCapture.Infrastucture.Messaging.Http.Bootstrapping
{
    using SystemDot.Ioc;
    using AppliedSystems.Messaging.Http.Receiving;

    public static class IocContainerExtensions
    {
        public static void RegisterHttpMessaging<TIncomingMessageConverter>(this IIocContainer container)
             where TIncomingMessageConverter : class, IIncomingMessageConverter
        {
            container.RegisterInstance<IWebAppStarter, WebAppStarter>();
            container.RegisterInstance<IIncomingMessageConverter, TIncomingMessageConverter>();
        }
    }
}