namespace AppliedSystems.DataWarehouse
{
    using System.Linq;
    using AppliedSystems.DataWarehouse.Messages;
    using AppliedSystems.Messaging.Infrastructure;
    using AppliedSystems.Messaging.Infrastructure.Sagas;
    using AppliedSystems.RiskCapture;
    using AppliedSystems.RiskCapture.Messages;

    public class RiskCaptureProcess : Saga<RiskCaptureProcessState>,
        ISagaEventHandler<NewRiskItemMapped>,
        ISagaEventHandler<NewRiskProductLineMapped>,
        ISagaEventHandler<NewRiskSectionMapped>,
        ISagaEventHandler<RiskItemValueCaptured>
    {
        public RiskCaptureProcess(MessageBus bus, ISagaStateRepository stateRepository)
            : base(bus, stateRepository)
        {
            State = new RiskCaptureProcessState();
        }
        
        public void When(NewRiskProductLineMapped message)
        {
            Then(new CreateProductLineSchema(message.ProductLine));

        }

        public void When(NewRiskSectionMapped message)
        {
            State.Sections.Add(new RiskCaptureProcessRiskSectionState { RiskSectionId = message.RiskSectionId, RiskSectionName = message.SectionName });

            Then(new CreateRiskTable(message.ProductLine, message.SectionName));
        }

        public void When(NewRiskItemMapped message)
        {
            RiskCaptureProcessRiskSectionState section = State.Sections.Single(s => s.RiskSectionId == message.RiskSectionId);
            section.Items.Add(new RiskCaptureProcessRiskItemState { RiskItemId = message.RiskItemId, RiskItemName = message.ItemName });

            Then(new CreateRiskTableColumn(
                State.ProductLine,
                section.RiskSectionName,
                message.ItemName));
        }

        public void When(RiskItemValueCaptured message)
        {
            RiskCaptureProcessRiskSectionState section = State.Sections.Single(s => s.RiskSectionId == message.RiskSectionId);
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