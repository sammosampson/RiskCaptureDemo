namespace AppliedSystems.DataWarehouse
{
    using System.Linq;
    using AppliedSystems.DataWarehouse.Messages;
    using AppliedSystems.Messaging.Infrastructure;
    using AppliedSystems.Messaging.Infrastructure.Sagas;
    using AppliedSystems.RiskCapture;
    using AppliedSystems.RiskCapture.Messages;

    public class RiskCaptureSaga : Saga<RiskCaptureSagaState>,
        ISagaEventHandler<NewRiskItemMapped>,
        ISagaEventHandler<NewRiskProductLineMapped>,
        ISagaEventHandler<NewRiskSectionMapped>,
        ISagaEventHandler<RiskItemValueCaptured>
    {
        public RiskCaptureSaga(MessageBus bus, ISagaStateRepository stateRepository)
            : base(bus, stateRepository)
        {
            State = new RiskCaptureSagaState();
        }
        
        public void When(NewRiskProductLineMapped message)
        {
            Then(new CreateProductLineSchema(message.ProductLine));

        }

        public void When(NewRiskSectionMapped message)
        {
            State.Sections.Add(new RiskCaptureSagaRiskSectionState { RiskSectionId = message.RiskSectionId, RiskSectionName = message.SectionName });

            Then(new CreateRiskTable(message.ProductLine, message.SectionName));
        }

        public void When(NewRiskItemMapped message)
        {
            RiskCaptureSagaRiskSectionState section = State.Sections.Single(s => s.RiskSectionId == message.RiskSectionId);
            section.Items.Add(new RiskCaptureSagaRiskItemState { RiskItemId = message.RiskItemId, RiskItemName = message.ItemName });

            Then(new CreateRiskTableColumn(
                State.ProductLine,
                section.RiskSectionName,
                message.ItemName));
        }

        public void When(RiskItemValueCaptured message)
        {
            RiskCaptureSagaRiskSectionState section = State.Sections.Single(s => s.RiskSectionId == message.RiskSectionId);
            var itemName = section.Items.Single(i => i.RiskItemId == message.ItemId).RiskItemName;
            
            Then(new UpdateRiskTableColumnValue(
              State.ProductLine,
              section.RiskSectionName,
              message.RiskCaptureId,
              itemName,
              message.Value));
        }
    }
}