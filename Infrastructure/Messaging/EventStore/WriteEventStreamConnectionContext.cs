using AppliedSystems.RiskCapture.Infrastucture.Messaging.EventStore.Writing;

namespace AppliedSystems.RiskCapture.Infrastucture.Messaging.EventStore
{
    public static class WriteEventStreamConnectionContext
    {
        public static IWriteEventStreamConnection Current { get; set; }
    }
}