namespace AppliedSystems.RiskCapture
{
    using Infrastucture.Messaging.EventSourcing;
    using Messages;
    using Messaging.Infrastructure.Commands;

    public class ProcessRiskCaptureRequestHandler : ICommandHandler<ProcessRiskCaptureRequest>
    {
        private readonly DomainRepository repository;

        public ProcessRiskCaptureRequestHandler(DomainRepository repository)
        {
            this.repository = repository;
        }

        public void Handle(ProcessRiskCaptureRequest message)
        {
            var capture = repository.Get<RiskCaptureMap>(new RiskCaptureMapId());
            capture.ExtractMapFromRequest(message.Request);
            repository.Save(capture);
        }
    }
}