using AppliedSystems.RiskCapture.Infrastucture.Messaging.EventStore.Reading;
using AppliedSystems.RiskCapture.Infrastucture.Messaging.EventStore.Writing;

namespace AppliedSystems.RiskCapture.Infrastucture.Messaging.EventStore
{
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using AppliedSystems.CodeAnalysis;
    using AppliedSystems.Core.Diagnostics;

    public class EventStreamConnectionManager
    {
        private static readonly TraceSource Trace = TraceSourceProvider.Provide();

        private readonly IWriteEventStreamConnector writeEventStreamConnector;
        private readonly IReadEventStreamConnector readEventStreamConnector;

        public EventStreamConnectionManager(IWriteEventStreamConnector writeEventStreamConnector, IReadEventStreamConnector readEventStreamConnector)
        {
            this.writeEventStreamConnector = writeEventStreamConnector;
            this.readEventStreamConnector = readEventStreamConnector;
        }

        public void Start()
        {
            Trace.Information("Opening the event stream writer connection");
            WriteEventStreamConnectionContext.Current = writeEventStreamConnector.Connect().Result;
            ReadEventStreamConnectionContext.Current = readEventStreamConnector.Connect().Result;
        }

        [SuppressMessage(FxCop.Performance, "CA1822:MarkMembersAsStatic", Justification = "Not making stop static in order to match the accessibility of start. It would be odd else.")]
        public void Stop()
        {
            Trace.Information("Closing the event stream writer connection");
            WriteEventStreamConnectionContext.Current.Close();
            WriteEventStreamConnectionContext.Current.Dispose();
            WriteEventStreamConnectionContext.Current = null;

            Trace.Information("Closing the event stream reader connection");
            ReadEventStreamConnectionContext.Current.Close();
            ReadEventStreamConnectionContext.Current.Dispose();
            ReadEventStreamConnectionContext.Current = null;
        }
    }
}