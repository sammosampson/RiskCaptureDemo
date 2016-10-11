namespace AppliedSystems.RiskCapture
{
    using System;
    using AppliedSystems.RatingHub.Xml.Header;
    using AppliedSystems.Xml;
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
            var mapId = new RiskCaptureMapId();
            var map = repository.Get<RiskCaptureMap>(mapId);
            map.ExtractMapFromRequest(message.Request);
            repository.Save(map);

            var captureId = RiskCaptureId.Parse(message.Request.ToXDocument().GetHeader().SequenceId.Value);
            var capture = repository.Get<RiskCapture>(captureId);
            capture.ExtractCaptureFromRequest(captureId, message.Request, map);
            repository.Save(capture);
        }
    }
}