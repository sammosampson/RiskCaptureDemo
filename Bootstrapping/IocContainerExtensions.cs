namespace AppliedSystems.RiskCapture.Service.Bootstrapping
{
    using SystemDot.Ioc;
    using AppliedSystems.Messaging.Infrastructure.Receiving;

    public static class IocContainerExtensions
    {
        public static void RegisterRiskCapture(this IIocContainer container)
        {
            container.RegisterInstance<IMessageReceivedErrorHandlingPolicy, StopSleepAndRestartMessageReceivedErrorHandlingPolicy>();
        }
    }
}