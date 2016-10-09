namespace AppliedSystems.Infrastucture.Messaging.EventSourcing
{
    using AppliedSystems.Messaging.Messages;

    public abstract class AggregateEntity<TRoot> where TRoot : AggregateRoot
    {
        readonly ConventionEventToHandlerRouter eventRouter;

        public TRoot Root { get; }

        protected AggregateEntity(TRoot root)
        {
            Root = root;
            Root.EventReplayed += OnAggregateRootEventAdded;
            eventRouter = new ConventionEventToHandlerRouter(this, "Apply");
        }

        protected void Then<TEvent>(TEvent @event) where TEvent : IEvent
        {
            Root.Then(@event);
        }

        void OnAggregateRootEventAdded(object sender, EventSourceEventArgs e)
        {
            eventRouter.RouteEventToHandlers(e.Event);
        }
    }
}