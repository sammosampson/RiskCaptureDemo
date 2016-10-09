using System.Diagnostics.CodeAnalysis;
using AppliedSystems.CodeAnalysis;

namespace AppliedSystems.RiskCapture.Infrastucture.Messaging.EventStore.Reading
{
    public interface IReadEventStreamSubscription
    {
        [SuppressMessage(FxCop.Naming, "CA1716:IdentifiersShouldNotMatchKeywords", MessageId = "Stop", Justification = "Stop is the only word that makes sense in this context")]
        void Stop();
    }
}