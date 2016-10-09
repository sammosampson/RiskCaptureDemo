namespace AppliedSystems.Infrastucture.Messaging.EventSourcing
{
    using System.Collections.Generic;
    using AppliedSystems.Messaging.Messages;

    public interface IEventStore
    {
        IEnumerable<IEvent> GetEvents(string streamId);

        void StoreEvents(string streamId, IEnumerable<IEvent> toStore);
    }
}