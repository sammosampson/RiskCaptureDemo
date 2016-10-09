namespace AppliedSystems.RiskCapture.Infrastucture.Messaging.Http
{
    using System.Collections.Generic;
    using AppliedSystems.Messaging.Http.Receiving;
    using AppliedSystems.Messaging.Infrastructure.Events;
    using AppliedSystems.Messaging.Infrastructure.Pipelines;
    using AppliedSystems.Messaging.Infrastructure.Receiving;

    public class RiskCaptureHttpReceivePointBuilder : IReceivingEndpointBuilder<RiskCaptureHttpReceivePoint>
    {
        private readonly IWebAppStarter webAppStarter;
        private readonly IIncomingMessageConverter incomingMessageConverter;

        public RiskCaptureHttpReceivePointBuilder(IWebAppStarter webAppStarter, IIncomingMessageConverter incomingMessageConverter)
        {
            this.webAppStarter = webAppStarter;
            this.incomingMessageConverter = incomingMessageConverter;
        }

        public IEnumerable<IMessageReceiver> BuildReceivers(
            RiskCaptureHttpReceivePoint endpoint, 
            MessagePipeline pipeline)
        {
            return new List<IMessageReceiver>
            {
                new RiskCaptureHttpMessageReceiver(endpoint.Url, pipeline, webAppStarter, incomingMessageConverter)
            };
        }

        public IEnumerable<ISubscriptionClient> BuildSubscriptionClients(RiskCaptureHttpReceivePoint endpoint)
        {
            return new List<ISubscriptionClient>();
        }
    }
}