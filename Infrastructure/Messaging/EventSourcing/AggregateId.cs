namespace AppliedSystems.Infrastucture.Messaging.EventSourcing
{
    public abstract class AggregateId
    {
        public abstract string ConvertToStreamName();
    }
}