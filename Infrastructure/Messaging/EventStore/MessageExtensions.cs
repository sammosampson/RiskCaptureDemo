namespace AppliedSystems.RiskCapture.Infrastucture.Messaging.EventStore
{
    using System;
    using AppliedSystems.Messaging.Http.Receiving;
    using AppliedSystems.Messaging.Infrastructure;
    using AppliedSystems.Messaging.Infrastructure.Headers;
    using global::EventStore.ClientAPI;

    public static class MessageExtensions
    {
        public static EventData ToEventStoreEventData(this Message toStore)
        {
            return new EventData(
                toStore.GetMessageId(),
                toStore.GetEventTypeName(),
                true,
                toStore.SerialisePayloadToJson().ToUtf8(),
                toStore.SerialiseHeadersToJson().ToUtf8());
        }

        private static string GetEventTypeName(this Message message)
        {
            return Type.GetType(message.PayloadType).Name.ToCamelCase();
        }

        private static Guid GetMessageId(this Message message)
        {
            return message.GetHeader(new MessageIdMessageHeaderKey(), raw => new Guid(raw));
        }

        private static string SerialisePayloadToJson(this Message message)
        {
            return EventStoreJsonSerialiser.SerialiseToJson(message.Payload);

        }
        private static string SerialiseHeadersToJson(this Message message)
        {
            return EventStoreJsonSerialiser.SerialiseToJson(message.Headers);

        }
    }
}