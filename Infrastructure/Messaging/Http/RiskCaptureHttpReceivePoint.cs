namespace AppliedSystems.Infrastucture.Messaging.Http
{
    using System;
    using AppliedSystems.Messaging.Http.Receiving;
    using AppliedSystems.Messaging.Infrastructure.Receiving;

    public class RiskCaptureHttpReceivePoint : IReceivingEndpoint
    {
        public static RiskCaptureHttpReceivePoint ListenOn(HttpMessagingReceiverUrl url)
        {
            return new RiskCaptureHttpReceivePoint(url);
        }

        public HttpMessagingReceiverUrl Url { get; }
        public Type EndpointBuilderType => typeof(RiskCaptureHttpReceivePointBuilder);

        private RiskCaptureHttpReceivePoint(HttpMessagingReceiverUrl url)
        {
            Url = url;
        }
    }
}