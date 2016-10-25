using System;

namespace AppliedSystems.Infrastucture.Messaging.EventSourcing
{
    public class AggregateState
    {
        protected AggregateState()
        {
        }

        protected AggregateState(AggregateId id)
        {
            Id = id;
        }

        public AggregateId Id { get; set; }
    }
}