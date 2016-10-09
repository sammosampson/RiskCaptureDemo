namespace AppliedSystems.Infrastucture.Messaging.EventStore.Writing
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;
    using AppliedSystems.Messaging.Infrastructure;
    using Core;
    using Core.Diagnostics;
    using global::EventStore.ClientAPI;

    public class PersistentWriteEventStreamConnection : Disposable
    {
        private static readonly TraceSource Trace = TraceSourceProvider.Provide();

        private readonly IEventStoreConnection connection;
        private readonly MessageStorageUserCredentials credentials;

        public PersistentWriteEventStreamConnection(IEventStoreConnection connection, MessageStorageUserCredentials credentials)
        {
            this.connection = connection;
            this.credentials = credentials;
        }

        public async Task AppendToStream(string streamName, IEnumerable<Message> toStore)
        {
            Trace.Information("Appending the current message to the event store stream {0}", streamName);

            await connection.AppendToStreamAsync(
                streamName, 
                ExpectedVersion.Any, 
                credentials, 
                toStore.Select(m => m.ToEventStoreEventData()).ToArray());
        }
        
        
        protected override void DisposeOfManagedResources()
        {
            connection.Close();
            connection.Dispose();
            base.DisposeOfManagedResources();
        }
    }
}