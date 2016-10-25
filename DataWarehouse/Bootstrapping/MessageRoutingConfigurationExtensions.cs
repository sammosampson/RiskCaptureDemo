namespace AppliedSystems.DataWarehouse.Bootstrapping
{
    using Messages;
    using Messaging.Infrastructure.Requests.Outgoing.InProcess;
    using RiskCapture.Messages;
    using Messaging.Infrastructure.Bootstrapping;

    public static class MessageRoutingConfigurationExtensions
    {
        public static MessageRoutingConfiguration WireUpRouting(this MessageRoutingConfiguration config, IRequestDispatchingEndpoint riskCaptureRequestEndpoint)
        {
            return config
                .Incoming.ForEvents
                    .Handle<NewRiskProductLineMapped>()
                        .ByStartingSaga<DataWarehousingProcess, DataWarehousingProcessState>(@event => @event.ProductLine)
                        .WithInitialState((@event, state) => @event.ProductLine = state.ProductLine)
                    .Handle<NewRiskSectionMapped>()
                        .ByContinuingSagaFoundBy<DataWarehousingProcess, DataWarehousingProcessState>((@event, state) => @event.ProductLine)
                    .Handle<NewRiskItemMapped>()
                        .ByContinuingSagaFoundBy<DataWarehousingProcess, DataWarehousingProcessState>((@event, state) => @event.ProductLine)
                    .Handle<RiskItemValueCaptured>()
                        .ByContinuingSagaFoundBy<DataWarehousingProcess, DataWarehousingProcessState>((@event, state) => @event.ProductLine)
                .Internal.ForCommands
                    .Handle<CreateProductLineSchema>().With<CreateProductLineSchemaHandler>()
                    .Handle<CreateRiskTable>().With<CreateRiskTableHandler>()
                    .Handle<CreateRiskTableColumn>().With<CreateRiskTableColumnHandler>()
                    .Handle<UpdateRiskTableColumnValue>().With<UpdateRiskTableColumnValueHandler>()
                .Outgoing.ForRequests
                    .Handle<LookupRiskCaptureItemMapping, LookupRiskCaptureItemMappingResponse>()
                    .ViaEndpoint(riskCaptureRequestEndpoint);
        }
    }
}