namespace AppliedSystems.Documents.Process
{
    using System.Linq;
    using AppliedSystems.Documents.Messages;
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

        }

        public void When(NewRiskSectionMapped message)
        {
            State.Sections.Add(new RiskCaptureSagaRiskSectionState { RiskSectionId = message.RiskSectionId, RiskSectionName = message.SectionName });
        }

        public void When(NewRiskItemMapped message)
        {
            RiskCaptureSagaRiskSectionState section = State.Sections.Single(s => s.RiskSectionId == message.RiskSectionId);
            section.Items.Add(new RiskCaptureSagaRiskItemState { RiskItemId = message.RiskItemId, RiskItemName = message.ItemName });
        }

        public void When(RiskItemValueCaptured message)
        {
            RiskCaptureSagaRiskSectionState section = State.Sections.Single(s => s.RiskSectionId == message.RiskSectionId);
            var itemName = section.Items.Single(i => i.RiskItemId == message.ItemId).RiskItemName;

            Then(new MergeFieldValueIntoDocument(message.RiskCaptureId, message.ProductLine, itemName, message.Value));
        }
    }
}