namespace AppliedSystems.RiskCapture
{
    using System;
    using AppliedSystems.Domain.EventSourced;
    using AppliedSystems.Infrastucture;
    using AppliedSystems.RatingHub.Xml.Header;
    using AppliedSystems.RiskCapture.Mapping;
    using AppliedSystems.RiskCapture.Values;
    using AppliedSystems.Xml;
    using Messages;
    using Messaging.Infrastructure.Commands;
    using Nancy.ModelBinding;

    public class ProcessRiskCaptureHandler : ICommandHandler<ProcessRiskCapture>
    {
        private readonly DomainRepository repository;

        public ProcessRiskCaptureHandler(DomainRepository repository)
        {
            this.repository = repository;
        }

        public void Handle(ProcessRiskCapture message)
        {
            GreenLogger.Log("Processing risk capture");

            using (repository.StartUnitOfWork())
            {
                var mapId = new MapId();
                var map = repository.Get<Map>(mapId);
                map.ExtractMapFromRequest(message.Request);

                var captureId = RiskCaptureId.Parse(message.Request.ToXDocument().GetHeader().SequenceId.Value);
                var capture = repository.Get<RiskCapture>(captureId);
                capture.ExtractCaptureFromRequest(captureId, message.Request, map);
            }
        }
    }
}