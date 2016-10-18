namespace AppliedSystems.RiskCapture
{
    using System;
    using System.Xml.Linq;
    using SystemDot.Core;
    using AppliedSystems.Messaging.Http.Receiving;
    using AppliedSystems.Messaging.Infrastructure;
    using AppliedSystems.RiskCapture.Messages;
    using Nancy;
    using Nancy.ModelBinding;

    public class ProcessRiskCaptureNancyModule : NancyModule
    {
        public ProcessRiskCaptureNancyModule()
        {
            Get["/risk/{id}"] = parameters => Response.AsJson(MessageSendingContext.Bus.Request<GetRisk, GetRiskResponse>(new GetRisk(parameters["id"])));

            Post["/risk", runAsync: true] = async (_, __) =>
            {
                string request = await Request.Body.ReadAsString();
                MessageSendingContext.Bus.Send(new ProcessRiskCapture(request));
                return HttpStatusCode.OK;
            };
        }
    }
}