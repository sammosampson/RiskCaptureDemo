namespace AppliedSystems.RiskCapture
{
    using System;
    using AppliedSystems.Messaging.Messages;

    public class GetRisk : IRequest
    {
        public Guid Id { get; }

        public GetRisk(Guid id)
        {
            Id = id;
        }
    }
}