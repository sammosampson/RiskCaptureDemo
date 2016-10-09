namespace AppliedSystems.RiskCapture.Infrastucture.Messaging.EventStore
{
    using Newtonsoft.Json;

    public static class EventStoreJsonSerialiser
    {
        private static JsonSerializerSettings TypeNameHandlingSerializerSettings
        {
            get
            {
                return new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
            }
        }

        public static string SerialiseToJson(object toSerialise)
        {
            return JsonConvert.SerializeObject(toSerialise, TypeNameHandlingSerializerSettings);
        }

        public static object DeserialiseFromJson(string toDeserialise)
        {
            return JsonConvert.DeserializeObject(toDeserialise, TypeNameHandlingSerializerSettings);
        }
    }
}