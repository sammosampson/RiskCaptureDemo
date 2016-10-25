namespace AppliedSystems.Documents.Process.Bootstrapping
{
    using AppliedSystems.Documents.Messages;
    using AppliedSystems.Messaging.Infrastructure.Bootstrapping;
    using AppliedSystems.Messaging.Infrastructure.Commands.Outgoing;
    using AppliedSystems.Messaging.Infrastructure.Requests.Outgoing.InProcess;
    using AppliedSystems.RiskCapture.Messages;

    public static class MessageRoutingConfigurationExtensions
    {
        public static MessageRoutingConfiguration WireUpRouting(this MessageRoutingConfiguration config, ICommandDispatchingEndpoint documentsEndpoint, IRequestDispatchingEndpoint riskCaptureRequestEndpoint)
        {
            return config
                .Incoming.ForEvents
                    .Ignore<NewRiskProductLineMapped>()
                    .Ignore<NewRiskSectionMapped>()
                    .Ignore<NewRiskItemMapped>()
                    .Handle<RiskItemValueCaptured>()
                        .ByStartingSaga<DocumentMergeProcess, DocumentMergeProcessState>(@event => @event.ProductLine)
                        .WithInitialState((@event, state) => @event.ProductLine = state.ProductLine)
                .Outgoing.ForCommands
                    .Send<MergeFieldValueIntoDocument>()
                    .ViaEndpoint(documentsEndpoint)
                .Outgoing.ForRequests
                    .Handle<LookupRiskCaptureItemMapping, LookupRiskCaptureItemMappingResponse>()
                    .ViaEndpoint(riskCaptureRequestEndpoint);
        }
    }
}