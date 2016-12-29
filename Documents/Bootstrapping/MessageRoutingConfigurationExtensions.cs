namespace AppliedSystems.Documents.Bootstrapping
{
    using AppliedSystems.Documents.Process;
    using AppliedSystems.RiskCapture.Messages;
    using Messages;
    using Messaging.Infrastructure.Bootstrapping;

    public static class MessageRoutingConfigurationExtensions
    {
        public static MessageRoutingConfiguration WireUpRouting(this MessageRoutingConfiguration config)
        {
            return config
                .Internal.ForCommands
                    .Handle<MergeFieldValueIntoDocument>().With<MergeFieldValueIntoDocumentHandler>()
                .Incoming.ForEvents
                    .Ignore<NewRiskSectionMapped>()
                    .Ignore<NewRiskItemMapped>()
                    .Handle<NewRiskProductLineMapped>()
                        .ByStartingSaga<DocumentMergeProcess, DocumentMergeProcessState>(@event => @event.ProductLine)
                        .WithInitialState((@event, state) => @event.ProductLine = state.ProductLine)
                    .Handle<RiskItemValueCaptured>()
                        .ByContinuingSagaFoundBy<DocumentMergeProcess, DocumentMergeProcessState>((@event, state) => @event.ProductLine);
        }
    }
}