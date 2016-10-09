namespace AppliedSystems.RiskCapture.Bootstrapping
{
    using SystemDot.Ioc;
    using Messaging.Infrastructure.Receiving;

    public static class IocContainerExtensions
    {
        public static void RegisterRiskCapture(this IIocContainer container)
        {
            container.RegisterInstance<IMessageReceivedErrorHandlingPolicy, StopSleepAndRestartMessageReceivedErrorHandlingPolicy>();
        }
    }
}