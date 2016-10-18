namespace AppliedSystems.RiskCapture.Bootstrapping
{
    using Messages;
    using Messaging.Infrastructure.Bootstrapping;

    public static class MessageRoutingConfigurationExtensions
    {
        public static MessageRoutingConfiguration WireUpRouting(this MessageRoutingConfiguration config)
        {
            return config
                .Internal
                    .ForCommands.Handle<ProcessRiskCapture>().With<ProcessRiskCaptureHandler>()
                    .ForRequests.Handle<GetRisk, GetRiskResponse>().With<GetRiskHandler>();
        }
    }
}