namespace AppliedSystems.DataWarehouse
{
    using System.Linq;
    using AppliedSystems.Messaging.Infrastructure;
    using AppliedSystems.Messaging.Infrastructure.Sagas;
    using AppliedSystems.RiskCapture;
    using AppliedSystems.RiskCapture.Messages;

    public class DataWarehouseSaga : Saga<DataWarehouseSagaState>,
        ISagaEventHandler<NewRiskItemMapped>,
        ISagaEventHandler<NewRiskProductLineMapped>,
        ISagaEventHandler<NewRiskSectionMapped>,
        ISagaEventHandler<RiskItemValueCaptured>
    {
        public DataWarehouseSaga(MessageBus bus, ISagaStateRepository stateRepository)
            : base(bus, stateRepository)
        {
            State = new DataWarehouseSagaState();
        }
        
        public void When(NewRiskProductLineMapped message)
        {
            Then(new CreateProductLineSchema(message.ProductLine));
        }

        public void When(NewRiskSectionMapped message)
        {
            State.Sections.Add(new DataWarehouseSagaRiskSectionState { RiskSectionId = message.RiskSectionId, RiskSectionName = message.SectionName });

            Then(new CreateRiskTable(message.ProductLine, message.SectionName));
        }

        public void When(NewRiskItemMapped message)
        {
            DataWarehouseSagaRiskSectionState section = State.Sections.Single(s => s.RiskSectionId == message.RiskSectionId);
            section.Items.Add(new DataWarehouseSagaRiskItemState { RiskItemId = message.RiskItemId, RiskItemName = message.ItemName });    

            Then(new CreateRiskTableColumn(
                State.ProductLine,
                section.RiskSectionName, 
                message.ItemName));
        }

        public void When(RiskItemValueCaptured message)
        {
            DataWarehouseSagaRiskSectionState section = State.Sections.Single(s => s.RiskSectionId == message.RiskSectionId);

            Then(new UpdateRiskTableColumnValue(
                State.ProductLine,
                section.RiskSectionName,
                message.RiskCaptureId,
                section.Items.Single(s => s.RiskItemId == message.ItemId).RiskItemName,
                message.Value));
        }
    }
}