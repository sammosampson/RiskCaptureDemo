namespace AppliedSystems.Infrastucture.Messaging.EventStore
{
    using System.Text;

    public static class ByteExtensions
    {
        public static string ToUtf8(this byte[] toConvert)
        {
            return Encoding.UTF8.GetString(toConvert);
        }
    }
}