namespace AppliedSystems.Documents.Process
{
    using AppliedSystems.Documents.Messages;
    using AppliedSystems.Messaging.Infrastructure;
    using AppliedSystems.Messaging.Infrastructure.Sagas;
    using AppliedSystems.RiskCapture.Messages;

    public class DocumentMergeProcess : Saga<DocumentMergeProcessState>,
        ISagaEventHandler<NewRiskProductLineMapped>,
        ISagaEventHandler<RiskItemValueCaptured>
    {
        public DocumentMergeProcess(MessageBus bus, ISagaStateRepository stateRepository)
            : base(bus, stateRepository)
        {
            State = new DocumentMergeProcessState();
        }

        public void When(NewRiskProductLineMapped message)
        {
            // just starts process (no-op)
        }

        public void When(RiskItemValueCaptured message)
        {
            Then(new MergeFieldValueIntoDocument(message.RiskCaptureId, message.ProductLine, message.Value));
        }
    }
}