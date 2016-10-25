namespace AppliedSystems.Infrastucture.Messaging.EventSourcing
{
    using System;
    using System.Security.Policy;
    using AppliedSystems.Messaging.Messages;

    public abstract class AggregateEntity<TState> where TState : AggregateState
    {
        readonly ConventionEventToHandlerRouter eventRouter;
        protected TState State { get; }
        
        protected AggregateEntity(TState state)
        {
            State = state;
            EventSourcingUnitOfWork.GetCurrent().EventReplayed += UnitOfWorkEventReplayed;
            EventSourcingUnitOfWork.GetCurrent().OnEnding += UnitOfWorkEnding;    
            eventRouter = new ApplyMethodConventionEventToHandlerRouter(State);
        }

        private void UnitOfWorkEventReplayed(object sender, EventSourceEventArgs args)
        {
            eventRouter.RouteEventToHandlers(args.Event);
        }

        private void UnitOfWorkEnding(object sender, EventArgs eventArgs)
        {
            EventSourcingUnitOfWork.GetCurrent().EventReplayed -= UnitOfWorkEventReplayed;
            EventSourcingUnitOfWork.GetCurrent().OnEnding -= UnitOfWorkEnding;
        }

        protected void Then<TEvent>(TEvent @event) where TEvent : IEvent
        {
            EventSourcingUnitOfWork.GetCurrent().AddEvent(State.Id.ConvertToStreamName(), @event);
        }
    }
}