using System.Diagnostics;
using System.Threading.Tasks;
using AppliedSystems.Core;
using AppliedSystems.Core.Diagnostics;
using AppliedSystems.Messaging.Infrastructure;
using EventStore.ClientAPI;

namespace AppliedSystems.RiskCapture.Infrastucture.Messaging.EventStore.Writing.Persistent
{
    public class PersistentWriteEventStreamConnection : Disposable, IWriteEventStreamConnection
    {
        private static readonly TraceSource Trace = TraceSourceProvider.Provide();

        private readonly IEventStoreConnection connection;
        private readonly MessageStorageUserCredentials credentials;

        public PersistentWriteEventStreamConnection(IEventStoreConnection connection, MessageStorageUserCredentials credentials)
        {
            this.connection = connection;
            this.credentials = credentials;
        }

        public async Task AppendToStream(string streamName, Message toStore)
        {
            Trace.Information("Appending the current message to the event store stream {0}", streamName);
            await connection.AppendToStreamAsync(
                streamName, 
                ExpectedVersion.Any, 
                credentials, 
                toStore.ToEventStoreEventData());
        }
        
        public void Close()
        {
            connection.Close();
        }

        protected override void DisposeOfManagedResources()
        {
            connection.Dispose();
            base.DisposeOfManagedResources();
        }
    }
}