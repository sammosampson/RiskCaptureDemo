namespace AppliedSystems.DataWarehouse.Bootstrapping
{
    using AppliedSystems.DataWarehouse.Messages;
    using AppliedSystems.RiskCapture;
    using AppliedSystems.RiskCapture.Messages;
    using Messaging.Infrastructure.Bootstrapping;

    public static class MessageRoutingConfigurationExtensions
    {
        public static MessageRoutingConfiguration WireUpRouting(this MessageRoutingConfiguration config)
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
                .Internal.ForCommands
                    .Handle<CreateProductLineSchema>().With<CreateProductLineSchemaHandler>()
                    .Handle<CreateRiskTable>().With<CreateRiskTableHandler>()
                    .Handle<CreateRiskTableColumn>().With<CreateRiskTableColumnHandler>()
                    .Handle<UpdateRiskTableColumnValue>().With<UpdateRiskTableColumnValueHandler>();
        }
    }
}