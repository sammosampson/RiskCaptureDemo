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

        public RiskCaptureController(string address)
        {
            this.address = address;
        }

        public void Start()
        {
            Trace.Information("Starting the risk capture service");
            webApp = WebApp.Start<WebAppStartup>(address);
            Trace.Information("Listening on {0}", address);
        }


        public void Stop()
        {
            webApp.Dispose();
            Trace.Information("Stopping the risk capture service");
        }
    }
}