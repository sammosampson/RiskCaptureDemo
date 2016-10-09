using System;
using System.Threading.Tasks;
using AppliedSystems.Messaging.Infrastructure;

namespace AppliedSystems.RiskCapture.Infrastucture.Messaging.EventStore.Writing
{
    public interface IWriteEventStreamConnection : IDisposable
    {
        void Close();

        Task AppendToStream(string streamName, Message toStore);
    }
}