namespace AppliedSystems.Infrastucture.Messaging.EventStore.Reading
{
    using System.Collections.ObjectModel;
    using AppliedSystems.Messaging.Infrastructure;
    using AppliedSystems.Messaging.Infrastructure.Headers;
    using Core;
    using global::EventStore.ClientAPI;

    public static class ResolvedEventExtensions
    {
        public static Message ToMessage(this ResolvedEvent toConvert)
        {
            return Message.Create(toConvert.DeserialiseDataFromJson())
                .AddHeaders(toConvert.DeserialiseMetaDataFromJson());
        }
        
        private static object DeserialiseDataFromJson(this ResolvedEvent toDeserialise)
        {
            return EventStoreJsonSerialiser.DeserialiseFromJson(toDeserialise.Event.Data.ToUtf8());
        }

        private static Collection<MessageHeader> DeserialiseMetaDataFromJson(this ResolvedEvent toDeserialise)
        {
            return EventStoreJsonSerialiser
                .DeserialiseFromJson(toDeserialise.Event.Metadata.ToUtf8())
                .As<Collection<MessageHeader>>();
        }
    }
}