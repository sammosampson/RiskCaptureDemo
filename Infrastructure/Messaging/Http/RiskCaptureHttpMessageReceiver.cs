namespace AppliedSystems.RiskCapture.Infrastucture.Messaging.Http
{
    using System;
    using System.Diagnostics;
    using System.Net;
    using System.Threading.Tasks;
    using AppliedSystems.Core;
    using AppliedSystems.Core.Diagnostics;
    using AppliedSystems.Messaging.Http.Receiving;
    using AppliedSystems.Messaging.Infrastructure;
    using AppliedSystems.Messaging.Infrastructure.Pipelines;
    using AppliedSystems.Messaging.Infrastructure.Receiving;
    using AppliedSystems.Messaging.Infrastructure.Responses;
    using Microsoft.Owin;
    using Microsoft.Owin.Hosting;
    using Owin;

    public class RiskCaptureHttpMessageReceiver : MessageReceiver, IRequestProcessor
    {
        private static readonly TraceSource Trace = TraceSourceProvider.Provide();

        private IDisposable server;
        private readonly HttpMessagingReceiverUrl url;
        private readonly IWebAppStarter webAppStarter;
        private IIncomingMessageConverter incomingMessageConverter;

        public RiskCaptureHttpMessageReceiver(
            HttpMessagingReceiverUrl url,
            MessagePipeline messagePipeline,
            IWebAppStarter webAppStarter, 
            IIncomingMessageConverter incomingMessageConverter)
            : base(messagePipeline)
        {
            this.url = url;
            this.webAppStarter = webAppStarter;
            this.incomingMessageConverter = incomingMessageConverter;
        }

        protected override void StartReceiving()
        {
            Trace.Information("Starting http message receiving {0}", url);

            server = webAppStarter.Start(
                new StartOptions(url), app => app.Use<RequestProcessingMiddleware>(this));

            Trace.Information("Listening for messages over http on {0}", url);
        }

        public override void StopReceiving()
        {
            Trace.Information("Stopping the http message receiver on {0}", url);
            server.Dispose();
        }

        public async Task ProcessRequest(IOwinRequest request, IOwinResponse response)
        {
            try
            {
                string message = await request.Body.ReadAsString();
                ReceiveRawMessage(message);

                response.StatusCode = (int)HttpStatusCode.OK;
                await response.WriteAsync(HttpResponseContext.Current.PopResponse());
                response.ContentType = "text/plain; charset=UTF-8";
            }
            catch (Exception)
            {
                response.StatusCode = (int)HttpStatusCode.BadRequest;
            }
        }

        private void ReceiveRawMessage(string rawMessage)
        {
            var message = new NotRequired<Message>();

            try
            {
                message = incomingMessageConverter.Convert(rawMessage);
                Trace.Information("Received message of type {0}", message.Value.PayloadType);
                using (new ResponderContext(new HttpResponder()))
                {
                    PutMessageInPipeline(message.Value);
                }
            }
            catch (Exception ex)
            {
                OnExceptionCallback(ex, message);
                throw;
            }
        }
    }
}