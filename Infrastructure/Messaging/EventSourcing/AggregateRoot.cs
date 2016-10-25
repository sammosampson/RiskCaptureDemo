namespace AppliedSystems.Infrastucture.Messaging.EventSourcing
{
    using System;
    using System.Collections.Generic;
    using AppliedSystems.Messaging.Messages;
    using Collections;

    public abstract class AggregateRoot<TState> : AggregateEntity<TState>, IAggregateRoot
        where TState : AggregateState
    {
        protected AggregateRoot(TState state)
            : base(state)
        {
        }

        public void Rehydrate(AggregateId toRehaydrateId, IEnumerable<IEvent> events)
        {
            State.Id = toRehaydrateId;
            events.ForEach(EventSourcingUnitOfWork.GetCurrent().ReplayEvent);
        }
    }
}