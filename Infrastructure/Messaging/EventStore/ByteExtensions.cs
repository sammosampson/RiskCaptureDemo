using System.Text;

namespace AppliedSystems.RiskCapture.Infrastucture.Messaging.EventStore
{
    public static class ByteExtensions
    {
        public static string ToUtf8(this byte[] toConvert)
        {
            return Encoding.UTF8.GetString(toConvert);
        }
    }
}