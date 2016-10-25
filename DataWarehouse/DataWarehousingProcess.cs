namespace AppliedSystems.DataWarehouse
{
    using AppliedSystems.DataWarehouse.Messages;
    using AppliedSystems.Messaging.Infrastructure;
    using AppliedSystems.Messaging.Infrastructure.Sagas;
    using AppliedSystems.RiskCapture.Messages;

    public class DataWarehousingProcess : Saga<DataWarehousingProcessState>,
        ISagaEventHandler<NewRiskItemMapped>,
        ISagaEventHandler<NewRiskProductLineMapped>,
        ISagaEventHandler<NewRiskSectionMapped>,
        ISagaEventHandler<RiskItemValueCaptured>
    {
        public DataWarehousingProcess(MessageBus bus, ISagaStateRepository stateRepository)
            : base(bus, stateRepository)
        {
            State = new DataWarehousingProcessState();
        }
        
        public void When(NewRiskProductLineMapped message)
        {
            Then(new CreateProductLineSchema(message.ProductLine));
        }

        public void When(NewRiskSectionMapped message)
        {
            Then(new CreateRiskTable(message.ProductLine, message.SectionName));
        }

        public void When(NewRiskItemMapped message)
        {
            Then(new CreateRiskTableColumn(State.ProductLine, message.SectionName, message.ItemName));
        }

        public void When(RiskItemValueCaptured message)
        {
            var response = Then<LookupRiskCaptureItemMapping, LookupRiskCaptureItemMappingResponse>(
                new LookupRiskCaptureItemMapping(message.ProductLine, message.ItemId));
            
            Then(new UpdateRiskTableColumnValue(
              State.ProductLine,
              response.SectionName,
              message.RiskCaptureId,
              response.ItemName,
              message.Value));
        }
    }
}