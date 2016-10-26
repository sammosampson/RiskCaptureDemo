namespace AppliedSystems.RiskCapture.Mapping
{
    using System.Linq;
    using AppliedSystems.Infrastucture.Messaging.EventSourcing;
    using AppliedSystems.Messaging.Infrastructure.Requests;
    using AppliedSystems.RiskCapture.Messages;

    public class LookupItemMappingHandler : IRequestHandler<LookupRiskCaptureItemMapping, LookupRiskCaptureItemMappingResponse>
    {
        private readonly IProjectionStore store;

        public LookupItemMappingHandler(IProjectionStore store)
        {
            this.store = store;
        }

        public LookupRiskCaptureItemMappingResponse Handle(LookupRiskCaptureItemMapping message)
        {
            var projection = store.GetProjection<LookupRiskCaptureItemMappingResponse>(LookupItemMappingId.Parse(message.ProductLine));
            return projection.Single(i => i.ItemId == message.ItemId);
        }
    }
}