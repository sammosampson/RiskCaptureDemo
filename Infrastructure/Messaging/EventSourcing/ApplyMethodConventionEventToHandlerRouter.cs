namespace AppliedSystems.Infrastucture.Messaging.EventSourcing
{
    public class ApplyMethodConventionEventToHandlerRouter : ConventionEventToHandlerRouter
    {
        public ApplyMethodConventionEventToHandlerRouter(object target) : base(target, "Apply")
        {
        }
    }
}