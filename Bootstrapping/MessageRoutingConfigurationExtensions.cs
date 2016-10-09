namespace AppliedSystems.RiskCapture.Service.Bootstrapping
{
    using AppliedSystems.Messaging.Infrastructure.Bootstrapping;
    using AppliedSystems.Messaging.Infrastructure.Events.Outgoing;

    public static class MessageRoutingConfigurationExtensions
    {
        public static MessageRoutingConfiguration WireUpRouting(this MessageRoutingConfiguration config, IEventDispatchingEndpoint eventStoreEndpoint)
        {
            return config
                .Incoming.ForCommands
                    .Handle<ProcessRiskCaptureRequest>().With<ProcessRiskCaptureRequestHandler>()
                .Outgoing.ForEvents
                    .Send<RiskCaptureProcessed>().ViaEndpoint(eventStoreEndpoint).ToEventStream("Test");
        }
    }
}