namespace AppliedSystems.RiskCapture
{
    using AppliedSystems.Messaging.Http.Receiving;
    using AppliedSystems.Messaging.Infrastructure;
    using AppliedSystems.RiskCapture.Messages;
    using Nancy;

    public class ProcessRiskCaptureNancyModule : NancyModule
    {
        public ProcessRiskCaptureNancyModule()
        {
            Post["/risk", runAsync: true] = async (_, __) =>
            {
                string request = await Request.Body.ReadAsString();
                MessageSendingContext.Bus.Send(new ProcessRiskCapture(request));
                return HttpStatusCode.OK;
            };
        }
    }
}