namespace AppliedSystems.Infrastucture.Messaging.EventStore
{
    using Newtonsoft.Json;

    public static class EventStoreJsonSerialiser
    {
        private static JsonSerializerSettings TypeNameHandlingSerializerSettings => new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };

        public static string SerialiseToJson(object toSerialise)
        {
            return JsonConvert.SerializeObject(toSerialise, TypeNameHandlingSerializerSettings);
        }

        public static object DeserialiseObjectFromJson(string toDeserialise)
        {
            return JsonConvert.DeserializeObject(toDeserialise, TypeNameHandlingSerializerSettings);
        }

        public static TRepresentation DeserialiseRepresentationFromJson<TRepresentation>(string toDeserialise)
        {
            return JsonConvert.DeserializeObject<TRepresentation>(toDeserialise);
        }
    }
}