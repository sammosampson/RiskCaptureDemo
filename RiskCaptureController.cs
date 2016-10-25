namespace AppliedSystems.RiskCapture
{
    using System;
    using System.Diagnostics;
    using AppliedSystems.RiskCapture.Bootstrapping;
    using Core;
    using Core.Diagnostics;
    using Messaging.Infrastructure;
    using Messaging.Infrastructure.Receiving;
    using Microsoft.Owin.Hosting;

    public class RiskCaptureController
    {
        private static readonly TraceSource Trace = TraceSourceProvider.Provide();

        private readonly string address;
        private IDisposable webApp;
        private readonly IMessageReceiver receiver;

        public RiskCaptureController(string address, IMessageReceiver receiver)
        {
            this.address = address;
            this.receiver = receiver;
        }

        public void Start()
        {
            Trace.Information("Starting the risk capture service");
            receiver.StartReceiving(OnException);
            webApp = WebApp.Start<WebAppStartup>(address);
            Trace.Information("Listening on {0}", address);
        }

        private void OnException(Exception exception, NotRequired<Message> message)
        {
            if (message.HasValue)
            {
                Trace.Error("Exception occurred whilst receving message {0}. Exception was {1}", message.Value.PayloadType, exception.GetExceptionDetails());
            }
            else
            {
                Trace.Error("Exception occurred whilst receving messages. Exception was {0}", exception.GetExceptionDetails());
            }
        }

        public void Stop()
        {
            webApp.Dispose();
            receiver.StopReceiving();
            Trace.Information("Stopping the risk capture service");
        }
    }
}