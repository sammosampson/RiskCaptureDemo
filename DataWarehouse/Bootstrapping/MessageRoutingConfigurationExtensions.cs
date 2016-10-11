namespace AppliedSystems.DataWarehouse.Bootstrapping
{
    using AppliedSystems.Messaging.Infrastructure.Commands;
    using AppliedSystems.RiskCapture;
    using Messaging.Infrastructure.Bootstrapping;
    using RiskCapture.Messages;

    public static class MessageRoutingConfigurationExtensions
    {
        public static MessageRoutingConfiguration WireUpRouting(this MessageRoutingConfiguration config)
        {
            return config
                .Incoming.ForEvents
                    .Handle<NewRiskProductLineMapped>()
                        .ByStartingSaga<DataWarehouseSaga, DataWarehouseSagaState>(@event => @event.ProductLine)
                        .WithInitialState((@event, state) => @event.ProductLine = state.ProductLine)
                    .Handle<NewRiskSectionMapped>()
                        .ByContinuingSagaFoundBy<DataWarehouseSaga, DataWarehouseSagaState>((@event, state) => @event.ProductLine)
                    .Handle<NewRiskItemMapped>()
                        .ByContinuingSagaFoundBy<DataWarehouseSaga, DataWarehouseSagaState>((@event, state) => @event.ProductLine)
                    .Handle<RiskItemValueCaptured>()
                        .ByContinuingSagaFoundBy<DataWarehouseSaga, DataWarehouseSagaState>((@event, state) => @event.ProductLine)
               .Internal.ForCommands
                    .Handle<CreateProductLineSchema>().With<CreateProductLineSchemaHandler>()
                    .Handle<CreateRiskTable>().With<CreateRiskTableHandler>()
                    .Handle<CreateRiskTableColumn>().With<CreateRiskTableColumnHandler>()
                    .Handle<UpdateRiskTableColumnValue>().With<UpdateRiskTableColumnValueHandler>();
        }
    }
}