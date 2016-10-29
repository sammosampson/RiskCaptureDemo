namespace AppliedSystems.Infrastucture.Messaging.EventSourcing
{
    using System.Collections.Generic;
    using AppliedSystems.Messaging.Messages;

    public interface IAggregateRoot
    {
        void Rehydrate(AggregateId toRehaydrateId, IEnumerable<IEvent> events);
    }
}