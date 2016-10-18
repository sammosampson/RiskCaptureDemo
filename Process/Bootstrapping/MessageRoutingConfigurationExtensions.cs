namespace AppliedSystems.Documents.Process.Bootstrapping
{
    using AppliedSystems.Documents.Messages;
    using AppliedSystems.Messaging.Infrastructure.Bootstrapping;
    using AppliedSystems.Messaging.Infrastructure.Commands.Outgoing;
    using AppliedSystems.RiskCapture;
    using AppliedSystems.RiskCapture.Messages;

    public static class MessageRoutingConfigurationExtensions
    {
        public static MessageRoutingConfiguration WireUpRouting(this MessageRoutingConfiguration config, ICommandDispatchingEndpoint documentsEndpoint)
        {
            return config
                .Incoming.ForEvents
                    .Handle<NewRiskProductLineMapped>()
                        .ByStartingSaga<RiskCaptureSaga, RiskCaptureSagaState>(@event => @event.ProductLine)
                        .WithInitialState((@event, state) => @event.ProductLine = state.ProductLine)
                    .Handle<NewRiskSectionMapped>()
                        .ByContinuingSagaFoundBy<RiskCaptureSaga, RiskCaptureSagaState>((@event, state) => @event.ProductLine)
                    .Handle<NewRiskItemMapped>()
                        .ByContinuingSagaFoundBy<RiskCaptureSaga, RiskCaptureSagaState>((@event, state) => @event.ProductLine)
                    .Handle<RiskItemValueCaptured>()
                        .ByContinuingSagaFoundBy<RiskCaptureSaga, RiskCaptureSagaState>((@event, state) => @event.ProductLine)
                .Outgoing.ForCommands
                    .Send<MergeFieldValueIntoDocument>().ViaEndpoint(documentsEndpoint);
        }
    }
}