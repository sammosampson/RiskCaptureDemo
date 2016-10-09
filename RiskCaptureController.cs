namespace AppliedSystems.RiskCapture.Service
{
    using System;
    using System.Diagnostics;
    using AppliedSystems.Core;
    using AppliedSystems.Core.Diagnostics;
    using AppliedSystems.Messaging.Infrastructure;
    using AppliedSystems.Messaging.Infrastructure.Receiving;

    public class RiskCaptureController
    {
        private static readonly TraceSource Trace = TraceSourceProvider.Provide();

        private readonly IMessageReceiver receiver;

        public RiskCaptureController(IMessageReceiver receiver)
        {
            this.receiver = receiver;
        }

        public void Start()
        {
            receiver.StartReceiving(OnException);
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
            Trace.Information("Stopping the risk capture service");
            receiver.StopReceiving();
        }
    }
}