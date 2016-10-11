namespace AppliedSystems.Infrastucture.Messaging.EventStore.Subscribing
{
    using System.Data;

    public static class DataReaderExtensions
    {
        public static int? GetLastIndex(this IDataReader reader)
        {
            int? index = null;
            while (reader.Read())
            {
                index = reader.GetInt32(0);
            }
            return index;
        }
    }
}