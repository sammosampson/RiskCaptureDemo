namespace AppliedSystems.Infrastucture.Messaging.EventSourcing
{
    using System.Collections.Generic;

    public interface IProjectionStore
    {
        IEnumerable<TProjectionItem> GetProjection<TProjectionItem>(string streamId);
    }
}