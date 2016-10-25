namespace AppliedSystems.Infrastucture.Messaging.EventSourcing
{
    using System.Collections.Generic;
    using AppliedSystems.Messaging.Infrastructure;

    public interface IEventStore
    {
        IEnumerable<Message> GetEvents(string streamId);

        void StoreEvents(IEnumerable<Message> toStore);
    }
}