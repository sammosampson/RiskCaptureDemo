namespace AppliedSystems.RiskCapture.Service
{
    using AppliedSystems.Messaging.Infrastructure.Commands;
    using AppliedSystems.RiskCapture.Infrastucture.Messaging.EventSourcing;

    public class ProcessRiskCaptureRequestHandler : ICommandHandler<ProcessRiskCaptureRequest>
    {
        private readonly DomainRepository repository;

        public ProcessRiskCaptureRequestHandler(DomainRepository repository)
        {
            this.repository = repository;
        }

        public void Handle(ProcessRiskCaptureRequest message)
        {
            var capture = repository.Get<RiskCapture>(RiskCaptureId.Parse(1));
            capture.ProcessRequest(message.Request);
            repository.Save(capture);
        }
    }
}